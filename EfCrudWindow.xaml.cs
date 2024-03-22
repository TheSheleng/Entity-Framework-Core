using Entity_Framework_Core.EfContext;
using Entity_Framework_Core.EfCrudView;
using Entity_Framework_Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Entity_Framework_Core
{
    /// <summary>
    /// Interaction logic for EfCrudWindow.xaml
    /// </summary>
    public partial class EfCrudWindow : Window
    {
        private ICollectionView departmentsView;
        private ICollectionView productsView;
        private ICollectionView managersView;
        private ICollectionView salesView;

        readonly Predicate<Object> departmentsFilter = obj => (obj as Department)?.DeleteDt == null;
        readonly Predicate<Object> productsFilter = obj => (obj as Product)?.DeleteDt == null;
        readonly Predicate<Object> managersFilter = obj => (obj as Manager)?.DeleteDt == null;
        readonly Predicate<Object> salesFilter = obj => (obj as Sale)?.DeleteDt == null;

        public EfCrudWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DateTime today = new DateTime(2023, DateTime.Now.Month, DateTime.Now.Day);

            DepartamentsListView.ItemsSource = null;
            ProductsListView.ItemsSource = null;
            ManagersListView.ItemsSource = null;
            SalesListView.ItemsSource = null;

            App.EfDataContext.Departments.Load();
            App.EfDataContext.Products.Load();
            App.EfDataContext.Managers.Load();
            App.EfDataContext.Sales.Load();

            DepartamentsListView.ItemsSource = App.EfDataContext.Departments.Local.ToObservableCollection();
            ProductsListView.ItemsSource = App.EfDataContext.Products.Local.ToObservableCollection();
            ManagersListView.ItemsSource = App.EfDataContext.Managers.Local.ToObservableCollection();
            SalesListView.ItemsSource = new ObservableCollection<Sale>(
                App.EfDataContext.Sales.Where(s => s.SaleDt.Date == today.Date.Date));

            departmentsView = productsView = managersView = salesView = 
                CollectionViewSource.GetDefaultView(DepartamentsListView.ItemsSource);

            departmentsView.Filter = departmentsFilter;
            productsView.Filter = productsFilter; 
            managersView.Filter = managersFilter;
            salesView.Filter = salesFilter;
        }

        private void DepartamentsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item &&
                item.Content is Department department)
            {
                EfDepartamentWindew dialog =
                    new EfDepartamentWindew(DepartamentModel.FromEntity(department));

                dialog.ShowDialog();

                if (dialog.Action == CrudActions.Update)
                {
                    department.Name = dialog.Model.Name;
                    department.InternationalName = dialog.Model.InternationalName;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    department.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
            }
        }

        private void AllDepartamentButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentsView.Filter == null)
            {
                departmentsView.Filter = departmentsFilter;
                AllDepartamentButton.Content = "All";
            }
            else
            {
                departmentsView.Filter = null;
                AllDepartamentButton.Content = "Hide";
            }
        }

        private void CreateDepartamentButton_Click(object sender, RoutedEventArgs e)
        {
            Department department = new Department() { Id = Guid.NewGuid() };

            EfDepartamentWindew dialog = new(DepartamentModel.FromEntity(department));

            dialog.ShowDialog();

            if (dialog.Action == CrudActions.Update) 
            {
                department.Name = dialog.Model.Name;
                department.InternationalName = dialog.Model.InternationalName;
                App.EfDataContext.Add(department);
                App.EfDataContext.SaveChanges();
                LoadData();
            }
        }

        private void ProductsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item &&
                item.Content is Product product)
            {
                EfProductWindow dialog =
                    new EfProductWindow(ProductModel.FromEntity(product));

                dialog.ShowDialog();

                if (dialog.Action == CrudActions.Update)
                {
                    product.Name = dialog.Model.Name;
                    product.Price = dialog.Model.Price;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    product.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
            }
        }

        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product() { Id = Guid.NewGuid() };

            EfProductWindow dialog = new(ProductModel.FromEntity(product));

            dialog.ShowDialog();

            if (dialog.Action == CrudActions.Update)
            {
                product.Name = dialog.Model.Name;
                product.Price = dialog.Model.Price;
                App.EfDataContext.Add(product);
                App.EfDataContext.SaveChanges();
                LoadData();
            }
        }

        private void AllProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (productsView.Filter == null)
            {
                productsView.Filter = productsFilter;
                AllProductButton.Content = "All";
            }
            else
            {
                productsView.Filter = null;
                AllProductButton.Content = "Hide";
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item &&
                item.Content is Manager manager)
            {
                EfManagerWindow dialog =
                    new EfManagerWindow(ManagerModel.FromEntity(manager));

                dialog.ShowDialog();

                if (dialog.Action == CrudActions.Update)
                {
                    manager.Surname = dialog.Model.Surname;
                    manager.Name = dialog.Model.Name;
                    manager.Secname = dialog.Model.Secname;
                    //manager.Manager = App.EfDataContext.Managers.First(m => m.Id == dialog.Model.Manager.Id);
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    manager.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
            }
        }

        private void AllManagersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateManagersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreateSalesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AllSalesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item &&
                item.Content is Sale sale)
            {
                EfSaleWindow dialog =
                    new EfSaleWindow(SaleModel.FromEntity(sale));

                dialog.ShowDialog();

                if (dialog.Action == CrudActions.Update)
                {
                    sale.Quantity = dialog.Model.Quantity;
                    sale.ProductId = dialog.Model.Product.Id;
                    sale.ManagerId = dialog.Model.Manager.Id; 
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    sale.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();
                    LoadData();
                }
            }
        }
    }
}
