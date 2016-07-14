using Microsoft.Win32;
using Products.Client.Utils;
using System.IO;
using System.Windows.Input;

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

        private byte[] image;
        public byte[] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        private ICommand cmdChooseImage;
        public ICommand CmdInsertImage
        {
            get
            {
                return this.cmdChooseImage ?? (this.cmdChooseImage =
                    new RelayCommand(this.BrowseForImage));
            }
        }

        private void BrowseForImage(object obj)
        {
            var ofd = new OpenFileDialog()
            {
                Multiselect = false,
                Title = "Choose an image for " + this.name,
                Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };
            var result = ofd.ShowDialog();
            if (result == false)
                return;

            this.image = File.ReadAllBytes(ofd.FileName);
        }
    }
}
