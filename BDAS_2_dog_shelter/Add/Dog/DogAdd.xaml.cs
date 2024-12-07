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

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interakční logika pro DogAdd.xaml
    /// </summary>
    public partial class DogAdd : Window
    {
        public DogAdd(List<Tables.Dog> psi, List<Owner> owners, List<Quarantine> quarantines)
        {

            InitializeComponent();
            Dog d = new();
            this.DataContext = new AddDogViewModel(d,psi,owners,quarantines);
            image.Source = ((AddDogViewModel)this.DataContext).Obrazek;
            ((AddDogViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public DogAdd (Dog d, List<Tables.Dog> psi, List<Owner> owners, List<Quarantine> quarantines)
        {
            InitializeComponent();
            this.DataContext = new AddDogViewModel(d,psi,owners,quarantines);
            ((AddDogViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
            byte[] b = new byte[256*256*4];
            new Random().NextBytes(b); 
            image.Source = ((AddDogViewModel)this.DataContext).Obrazek?? BitmapSource.Create(256, 256, 96, 96, PixelFormats.Bgra32, null, b, 256 * 4);
        }

        public Dog Dog { get; internal set; }

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
                ((AddDogViewModel)this.DataContext).Obrazek = image;
                ((AddDogViewModel)this.DataContext).Filename = Path.GetFileName(image.UriSource.LocalPath); 
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show("Je očekáván obrázek. Podporované typy mohou záviset na nainstalovaných aplikacích", "Neplatný typ souboru.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        //((Image)sender).Source = e.Data.GetData(DataFormats.FileDrop); //e.Data.GetDataPresent(DataFormats.FileDrop, true);
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
        //    

        }
    }
}
