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
            ((MainWindowViewModel)DataContext).OnCloaseRequest += CloseWindow;

            InitializeComponent();
        }

        private void CloseWindow()
        {
            this.CloseWindow();
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
        private void KarantenaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void MajitelDataGrid_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
        }
        private void PavilonDataGrid_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
        }
        private void ProceduraDataGrid_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
        }
        private void MedicalRecDataGrid_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
        }
        private void ReservaceDataGrid_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
        }

        private void MajitelDataGrid_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
