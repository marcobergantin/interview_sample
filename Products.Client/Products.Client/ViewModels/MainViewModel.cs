using Products.Client.Models;
using Products.Client.Utils;
using Products.Client.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;

namespace Products.Client.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Fields

        private HttpClient _client;

        #endregion

        #region Properties

        public IEnumerable<Product> Products
        {
            get;
            set;
        }

        public Product SelectedProduct
        {
            get;
            set;
        }

        #endregion

        #region Commands

        private ICommand cmdGetProducts;
        public ICommand CmdGetProducts
        {
            get
            {
                return this.cmdGetProducts ?? (this.cmdGetProducts = new RelayCommand(this.GetProducts));
            }
        }

        private ICommand cmdInsertProducts;
        public ICommand CmdInsertProducts
        {
            get
            {
                return this.cmdInsertProducts ?? (this.cmdInsertProducts = new RelayCommand(this.InsertProducts));
            }
        }

        private ICommand cmdModifyProduct;
        public ICommand CmdModifyProduct
        {
            get
            {
                return this.cmdModifyProduct ?? (this.cmdModifyProduct = 
                    new RelayCommand(this.ModifyProduct));
            }
        }

        private ICommand cmdDeleteProduct;
        public ICommand CmdDeleteProduct
        {
            get
            {
                return this.cmdDeleteProduct ?? (this.cmdDeleteProduct =
                    new RelayCommand(this.DeleteProduct));
            }
        }

        private ICommand cmdInsertImage;
        public ICommand CmdInsertImage
        {
            get
            {
                return this.cmdInsertImage ?? (this.cmdInsertImage =
                    new RelayCommand(this.InsertImage));
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["ServerURI"])
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        #region Methods

        private async void ModifyProduct(object obj)
        {
            ProductViewModel p = new ProductViewModel()
            {
                Name = this.SelectedProduct.Name,
                Price = this.SelectedProduct.Price,
                Image = this.SelectedProduct.Image
            };

            ProductPropertiesView v = new ProductPropertiesView();
            ((ProductPropertiesViewModel)(v.DataContext)).Product = p;
            if (v.ShowDialog() == true)
            {
                try
                {
                    string uri = string.Format("Products/{0}", this.SelectedProduct.Id);
                    var response = await _client.PutAsJsonAsync(uri, p);
                    this.GetProducts(null); //refresh   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name);
                }
            }
        }

        private async void InsertProducts(object obj)
        {
            ProductViewModel p = new ProductViewModel();
            ProductPropertiesView v = new ProductPropertiesView();
            ((ProductPropertiesViewModel)(v.DataContext)).Product = p;
            if (v.ShowDialog() == true)
            {
                try
                {
                    var response = await _client.PostAsJsonAsync("Products", p);
                    this.GetProducts(null); //refresh               
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name);
                }
            }
        }

        private async void GetProducts(object obj)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("products");
                if (response.IsSuccessStatusCode)
                {
                    this.Products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                    this.NotifyPropertyChanged(nameof(this.Products));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }

        private async void DeleteProduct(object obj)
        {
            if( MessageBox.Show(string.Format("Delete {0}?", this.SelectedProduct.Name), "Products",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            { 
                try
                {
                    string uri = string.Format("Products/{0}", this.SelectedProduct.Id);
                    var response = await _client.DeleteAsync(uri);
                    this.GetProducts(null); //refresh   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name);
                }
            }
        }

        private async void InsertImage(object obj)
        {
            ProductViewModel p = new ProductViewModel()
            {
                Name = this.SelectedProduct.Name,
                Price = this.SelectedProduct.Price
            };

            p.CmdInsertImage.Execute(null);
            try
            {
                string uri = string.Format("Products/Images/{0}", this.SelectedProduct.Id);
                var response = await _client.PutAsJsonAsync(uri, p.Image);
                this.GetProducts(null); //refresh   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }   
        }


        public void Dispose()
        {
            this._client.Dispose();
        }

        #endregion
    }
}
