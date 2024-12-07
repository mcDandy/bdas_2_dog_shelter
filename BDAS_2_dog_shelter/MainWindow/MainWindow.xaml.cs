using System.Windows;
using System.Windows.Controls;

namespace BDAS_2_dog_shelter.MainWindow
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ulong permissions)
        {

            DataContext = new MainWindowViewModel(permissions);
            InitializeComponent();
        }

        private void utulekDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void adresaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void hrackaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dhDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MedicalDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
