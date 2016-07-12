using Products.Client.Models;
using Products.Client.Utils;
using Products.Client.Views;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:51160/")
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
                Price = this.SelectedProduct.Price
            };

            ProductPropertiesView v = new ProductPropertiesView();
            ((ProductPropertiesViewModel)(v.DataContext)).Product = p;
            if (v.ShowDialog() == true)
            {
                try
                {
                    string uri = string.Format("Products/{0}", this.SelectedProduct.Id);
                    var response = await _client.PutAsJsonAsync(uri, p);
                    if (response.IsSuccessStatusCode)
                    {
                        // Get the URI of the created resource.
                        Uri productUrl = response.Headers.Location;
                    }
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
                    if (response.IsSuccessStatusCode)
                    {
                        // Get the URI of the created resource.
                        Uri productUrl = response.Headers.Location;
                    }
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
            try
            {
                string uri = string.Format("Products/{0}", this.SelectedProduct.Id);
                var response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.
                    Uri productUrl = response.Headers.Location;
                }
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
