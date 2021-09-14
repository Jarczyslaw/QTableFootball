using QTableFootball.App.ViewModels;
using System.Windows;

namespace QTableFootball.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MinWidth =
                Width = 1000;
            MinHeight =
                Height = 600;
            DataContext = new MainViewModel();
        }
    }
}