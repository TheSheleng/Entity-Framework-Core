using Entity_Framework_Core.Models;
using System;
using System.Collections.Generic;
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

namespace Entity_Framework_Core.EfCrudView
{
    /// <summary>
    /// Interaction logic for EfManagerWindow.xaml
    /// </summary>
    public partial class EfManagerWindow : Window
    {
        public ManagerModel Model { get; init; }
        public CrudActions Action { get; private set; }

        public EfManagerWindow(ManagerModel model)
        {
            this.Model = model;
            this.DataContext = this;
            InitializeComponent();
            this.Action = CrudActions.None;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainDepComboBox.SelectedItem = this.Model.Departments.First(n => n.Id == Model.MainDep.Id);
            
            if(Model.SecDep != null)
            {
                SecDepComboBox.SelectedItem = this.Model.Departments.First(n => n.Id == Model.SecDep.Id);
            }

            if (Model.Chief != null)
            {
                ChiefComboBox.SelectedItem = this.Model.Chiefs.First(n => n.Id == Model.Chief.Id);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Action = CrudActions.None;
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Action = CrudActions.Update;
            this.Close();
        }

        private void DeletelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Action = CrudActions.Delete;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SecDepComboBox.SelectedItem = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChiefComboBox.SelectedItem = null;
        }
    }
}
