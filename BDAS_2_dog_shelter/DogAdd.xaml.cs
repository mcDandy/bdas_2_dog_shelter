using BDAS_2_dog_shelter.Tables;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interakční logika pro DogAdd.xaml
    /// </summary>
    public partial class DogAdd : Window
    {
        public DogAdd()
        {
            Dog = new();
            InitializeComponent();
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
            }
            catch (NotSupportedException ex)
            {
                MessageBox.Show("Je očekáván obrázek. Podporované typy mohou záviset na nainstalovaných aplikacích", "Neplatný typ souboru.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        
        //((Image)sender).Source = e.Data.GetData(DataFormats.FileDrop); //e.Data.GetDataPresent(DataFormats.FileDrop, true);
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Dog.Obrazek= (BitmapSource)image.Source;
            this.DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void age_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void age_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(a => !Char.IsDigit(a))) e.Handled = true;
        }
    }
}
