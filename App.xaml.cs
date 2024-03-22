using System.Configuration;
using System.Data;
using System.Windows;

namespace Entity_Framework_Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EfContext.EfContext EfDataContext { get; } = new();
        public static Random Random { get; } = new Random();
    }

}
