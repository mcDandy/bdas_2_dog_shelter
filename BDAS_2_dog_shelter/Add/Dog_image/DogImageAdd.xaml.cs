using BDAS_2_dog_shelter.Tables;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
            DataContext = new AddDogImageViewModel(new Dog_Images());
            InitializeComponent();
            ((AddDogImageViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }

        public DogImageAdd(Dog_Images feed)
        {
            DataContext = new AddDogImageViewModel(feed);
            InitializeComponent();
            ((AddDogImageViewModel)DataContext).OkClickFinished += () => DialogResult = true;

        }
        private void image_Drop(object sender, DragEventArgs e)
        {
            string o = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            try
            {
                BitmapImage image;
                image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(o);
                image.EndInit();
                ((Image)sender).Source = image;
                ((AddDogImageViewModel)DataContext).Obrazek = image;
                ((AddDogImageViewModel)DataContext).Filename = Path.GetFileName(image.UriSource.LocalPath);
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show("Je očekáván obrázek. Podporované typy mohou záviset na nainstalovaných aplikacích", "Neplatný typ souboru.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //((Image)sender).Source = e.Data.GetData(DataFormats.FileDrop); //e.Data.GetDataPresent(DataFormats.FileDrop, true);
        }


    }
}
