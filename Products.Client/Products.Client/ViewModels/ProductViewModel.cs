using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Client.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.NotifyPropertyChanged(nameof(this.Name));
            }
        }

        private float price;
        public float Price
        {
            get { return this.price; }
            set
            {
                this.price = value;
                this.NotifyPropertyChanged(nameof(this.Price));
            }
        }
    }
}
