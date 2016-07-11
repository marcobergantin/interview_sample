using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Products.WebApi.Models
{
    public class ProductModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
    }
}