using System;

namespace Products.Client.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public byte[] Image { get; set; }
    }
}
