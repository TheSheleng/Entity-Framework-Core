using Entity_Framework_Core.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Entity_Framework_Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        { 
            App.EfDataContext.Database.Migrate();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayStaticstic();
        }
        
        private void DisplayStaticstic()
        {
            DepartmentCounterLabel.Content = App.EfDataContext.Departments.Count();
            ManageCounterLabel.Content = App.EfDataContext.Managers.Count();
            ProductCounterLabel.Content = App.EfDataContext.Products.Count();
            SaleCounterLabel.Content = App.EfDataContext.Sales.Count();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            var YoungerManager_query = App.EfDataContext.Managers
                                        .Where(r => r.Birthday != null)
                                        .OrderByDescending(r => r.Birthday)
                                        .FirstOrDefault();

            ResultLabel.Content += $"YoungerManager: {YoungerManager_query?.Name}\n";


            ResultLabel.Content += "- - - - - - - - - - -\n";

            var ShortestProduct_query = App.EfDataContext.Products
                                        .OrderBy(r => r.Name.Length)
                                        .First();

            ResultLabel.Content += $"YoungerManager: {ShortestProduct_query.Name}\n";


            ResultLabel.Content += "- - - - - - - - - - -\n";

            var rProduct_query = App.EfDataContext.Products
                                    .OrderBy(r => Guid.NewGuid())
                                    .First();

            ResultLabel.Content += $"{rProduct_query.Name}\n";

            var rManager_query = App.EfDataContext.Managers
                                    .OrderBy(r => Guid.NewGuid())
                                    .First();

            ResultLabel.Content += $"{rManager_query.Name}\n";

            var rSales_query = App.EfDataContext.Sales
                                    .OrderBy(r => Guid.NewGuid())
                                    .First();

            ResultLabel.Content += $"{rSales_query.Id}\n";

            ResultLabel.Content += "- - - - - - - - - - -\n";

            DateTime start = new DateTime(2023, 1, 1);

            DateTime randomDate = start
                .AddDays(App.Random.Next((DateTime.Today - start).Days))
                .AddHours(App.Random.Next(9, 20))
                .AddMinutes(App.Random.Next(0, 59))
                .AddSeconds(App.Random.Next(0, 59));

            ResultLabel.Content += randomDate.ToString();
        }

        private void RandSelesButton_Click(object sender, RoutedEventArgs e)
        {
            SaleCounterLabel.Content = "Undating...";
            Task.Run(AddSales);
        }

        private async Task AddSales()
        {
            for (int i = 0; i < 100000; i++)
            {
                App.EfDataContext.Sales.Add(new EfContext.Sale()
                {
                    Id = Guid.NewGuid(),
                    Quantity = App.Random.Next(1, 10),
                    ProductId = App.EfDataContext.Products.OrderBy(r => Guid.NewGuid()).First().Id,
                    ManagerId = App.EfDataContext.Managers.OrderBy(r => Guid.NewGuid()).First().Id,
                    SaleDt = new DateTime(2023, 1, 1)
                        .AddDays(App.Random.Next(365))
                        .AddHours(App.Random.Next(9, 20))
                        .AddMinutes(App.Random.Next(0, 59))
                        .AddSeconds(App.Random.Next(0, 59)),
                    DeleteDt = null,
                });
            }

            await App.EfDataContext.SaveChangesAsync();
            Dispatcher.Invoke(DisplayStaticstic);
        }

        private void ProductSalesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            DateTime date = new(2023, 02, 23);

            var query = App.EfDataContext.Products
                .GroupJoin(
                    App.EfDataContext.Sales,
                    p => p.Id,
                    s => s.ProductId,
                    (product, sales) => new { 
                        Name = product.Name,
                        Pcs = sales.Where(s => s.SaleDt == date).Sum(s => s.Quantity)
                    }
                );

            foreach ( var product in query )
            {
                ResultLabel.Content += $"{product.Name} - {product.Pcs}\n";
            }
        }

        private void ManagersStatButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime today = new DateTime(2023, DateTime.Now.Month, DateTime.Now.Day);

            ResultLabel.Content = $"- - - {today.ToString("yyyy-MM-dd")} - - -\n";

            var query = from M in App.EfDataContext.Managers
                        join S in App.EfDataContext.Sales on M.Id equals S.ManagerId into Ss
                        from sales in Ss.DefaultIfEmpty()
                        where sales.SaleDt.Date == today.Date.Date
                        group M by new { M.Id, M.Name } into G
                        select new
                        {
                            Name = G.Key.Name,
                            SaleCount = G.Count(s => true) - 1
                        };

            foreach (var manager in query)
            {
                ResultLabel.Content += $"{manager.Name} - {manager.SaleCount}\n";
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            foreach (var man in 
                App.EfDataContext
                .Managers
                .Include(m => m.MainDepartment))
            {
                ResultLabel.Content += $"{man.Name} {man.MainDepartment.Name}\n";
            }

            ResultLabel.Content += "\n";

            foreach (var str in 
                App.EfDataContext
                .Departments
                .Include(d => d.MainManagers)
                .Select(d => $"{d.Name} {d.MainManagers.Count}"))
            {
                ResultLabel.Content += $"{str}\n";
            }
        }

        private void Nav2Button_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            var managers = App.EfDataContext
                .Managers
                .Include(m => m.SecondaryDepartment)
                .ToList();

            foreach (var m in managers
                .Select((manager, index) => new
                {
                    Index = index + 1,
                    Manager = manager
                }))
            {
                string Department = m.Manager.SecondaryDepartment == null ? "--" : m.Manager.SecondaryDepartment.Name;
                ResultLabel.Content += $"{m.Index}. {m.Manager.Surname}: {Department}\n";
            }

            ResultLabel.Content += "\n";

            foreach (var str in
                App.EfDataContext
                    .Departments
                    .Include(d => d.SecondaryManagers)
                    .Select(d => $"{d.Name} {d.SecondaryManagers.Count()}"))
            {
                ResultLabel.Content += $"{str}\n";
            }
        }

        private void ChiefButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            foreach (var manager in 
                App.EfDataContext
                .Managers
                .Include(m =>m.Chierf))
            {
                ResultLabel.Content += $"{manager.Name} -- {manager.Chierf?.Secname ?? "no chief"}\n";
            }
        }

        private void SubsButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            foreach (var manager in
                App.EfDataContext
                .Managers
                .Include(m => m.Subordinates))
            {
                string subs = string.Join(", ", manager.Subordinates.Select(m => m.Surname));
                ResultLabel.Content += $"{manager.Surname} -- {(subs == "" ? "nu subs" : subs)}\n";
            }
        }

        private void SalesProdButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content = "";

            DateTime date = new DateTime(2023, 02, 28);

            foreach (var p in
                App.EfDataContext
                .Products
                .Include(p => p.Sales))
            {
                int totalToday = p.Sales.Where(s => s.SaleDt.Date == date).Count();
                ResultLabel.Content += $"{p.Name} -- {totalToday}\n";
            }
        }

        private void SalesStatsButton_Click(object sender, RoutedEventArgs e)
        {
            ResultLabel.Content += "";

            DateTime today = new DateTime(2023, DateTime.Now.Month, DateTime.Now.Day);

            var query = from s in App.EfDataContext.Sales
                        where s.SaleDt.Date == today.Date.Date
                        join p in App.EfDataContext.Products on s.ProductId equals p.Id
                        group new { s, p } by new { p.Id, p.Name, p.Price } into g
                        select $"{g.Key.Name} - {g.Sum(sp => sp.s.Quantity)} шт. - {g.Count(sp => true)} шт. - {g.Key.Price * g.Sum(sp => sp.s.Quantity)} грн.\n";
            foreach (var str in query)
            {
                ResultLabel.Content += str;
            }
        }

        private void CrudButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new EfCrudWindow().ShowDialog();
            this.Show();
        }
    }
}