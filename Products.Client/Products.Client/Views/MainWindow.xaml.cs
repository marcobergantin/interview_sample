using Products.Client.ViewModels;
using System.Windows;

namespace Products.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();

            this.Closing -= MainWindow_Closing;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((MainViewModel)(this.DataContext)).Dispose();
        }
    }
}
