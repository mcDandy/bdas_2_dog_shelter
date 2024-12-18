using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interakční logika pro DogAdd.xaml
    /// </summary>
    public partial class DogAdd : Window
    {
        public DogAdd(List<Dog> psi, List<Owner> owners, List<Quarantine> quarantines, List<Shelter> utulky)
        {

            InitializeComponent();
            Dog d = new();
            DataContext = new AddDogViewModel(d,utulky,psi,owners,quarantines);
            image.Source = ((AddDogViewModel)DataContext).Obrazek;
            ((AddDogViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public DogAdd (Dog d, List<Dog> psi, List<Owner> owners, List<Quarantine> quarantines, List<Shelter> utulky)
        {
            InitializeComponent();
            DataContext = new AddDogViewModel(d,utulky,psi,owners,quarantines);
            ((AddDogViewModel)DataContext).OkClickFinished += () => DialogResult = true;
            byte[] b = new byte[256*256*4];
            new Random().NextBytes(b); 
            image.Source = ((AddDogViewModel)DataContext).Obrazek?? BitmapSource.Create(256, 256, 96, 96, PixelFormats.Bgra32, null, b, 256 * 4);
        }

        public Dog Dog { get; internal set; }

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
                ((AddDogViewModel)DataContext).Obrazek = image;
                ((AddDogViewModel)DataContext).Filename = Path.GetFileName(image.UriSource.LocalPath); 
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
