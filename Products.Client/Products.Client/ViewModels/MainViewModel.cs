using Products.Client.Utils;
using Products.Client.Views;
using Products.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Input;


namespace Products.Client.ViewModels
{
    public class MainViewModel
    {
        private ICommand cmdGetProducts;
        public ICommand CmdGetProducts
        {
            get
            {
                return this.cmdGetProducts ?? (this.cmdGetProducts = new RelayCommand(this.getProducts));
            }
        }

        private ICommand cmdInsertProducts;
        public ICommand CmdInsertProducts
        {
            get
            {
                return this.cmdInsertProducts ?? (this.cmdInsertProducts = new RelayCommand(this.insertProducts));
            }
        }

        private async void insertProducts(object obj)
        {
            ProductViewModel p = new ProductViewModel();
            ProductPropertiesView v = new ProductPropertiesView();
            ((ProductPropertiesViewModel)(v.DataContext)).Product = p;
            if (v.ShowDialog() == true)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:51160/");
                        var response = await client.PostAsJsonAsync("Products", p);
                        if (response.IsSuccessStatusCode)
                        {
                            // Get the URI of the created resource.
                            Uri productUrl = response.Headers.Location;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name);
                }
            }
        }

        private async void getProducts(object obj)
        {
            try
            { 
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:51160/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync("products");
                    if (response.IsSuccessStatusCode)
                    {
                        //ok
                        List<Product> p = await response.Content.ReadAsAsync<List<Product>>();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
            }
        }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
