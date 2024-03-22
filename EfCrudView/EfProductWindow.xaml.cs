﻿using Entity_Framework_Core.Models;
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
    /// Interaction logic for EfProductWindow.xaml
    /// </summary>
    public partial class EfProductWindow : Window
    {
        public ProductModel Model { get; init; }
        public CrudActions Action { get; private set; }

        public EfProductWindow(ProductModel model)
        {
            this.Model = model;
            this.DataContext = this;
            InitializeComponent();
            this.Action = CrudActions.None;
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
    }
}
