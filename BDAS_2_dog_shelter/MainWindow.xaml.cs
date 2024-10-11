using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection <Dog> Dogs { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new[] { Dogs };
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
