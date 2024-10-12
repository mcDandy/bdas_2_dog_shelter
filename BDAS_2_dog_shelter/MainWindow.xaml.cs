using System.Collections.ObjectModel;
using System.Windows;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection <Dog> Dogs { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Dogs.Add(new Dog("test"));
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in dataGrid.SelectedItems)
            {
                ((Dog)item).Name = "Removed";
            }
        }
    }
}
