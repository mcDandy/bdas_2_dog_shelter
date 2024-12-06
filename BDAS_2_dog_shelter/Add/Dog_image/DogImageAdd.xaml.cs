using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.Add.Dog_Image
{
    /// <summary>
    /// Interakční logika pro Feed_Add.xaml
    /// </summary>
    public partial class DogImageAdd : Window
    {
        public DogImageAdd()
        {
            InitializeComponent();
        }

        public DogImageAdd(Dog_Images feed) : this() 
        {
            this.DataContext = new AddDogImageViewModel(feed);
        }
        private void image_Drop(object sender, DragEventArgs e)
        {
            string o = ((string[])e.Data.GetData(System.Windows.DataFormats.FileDrop))[0];
            try
            {
                BitmapImage image;
                image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(o);
                image.EndInit();
                ((Image)sender).Source = image;
                ((AddDogImageViewModel)this.DataContext).Obrazek = image;
                ((AddDogImageViewModel)this.DataContext).Filename = Path.GetFileName(image.UriSource.LocalPath);
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show("Je očekáván obrázek. Podporované typy mohou záviset na nainstalovaných aplikacích", "Neplatný typ souboru.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //((Image)sender).Source = e.Data.GetData(DataFormats.FileDrop); //e.Data.GetDataPresent(DataFormats.FileDrop, true);
        }


    }
}
