using Products.Client.ViewModels;
using System.Windows;

namespace Products.Client.Views
{
    /// <summary>
    /// Interaction logic for ProductPropertiesView.xaml
    /// </summary>
    public partial class ProductPropertiesView : Window
    {
        public ProductPropertiesView()
        {
            InitializeComponent();
            this.DataContext = new ProductPropertiesViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
