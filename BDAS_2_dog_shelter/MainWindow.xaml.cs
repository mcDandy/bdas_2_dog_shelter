using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Windows;
using BDAS_2_dog_shelter.Tables;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection <Dog> Dogs { get; set; } = new();
        private long permissions=0;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        public MainWindow(long permissions) : this() {
            this.permissions = permissions;
        }
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            Dog d = new("test", 10, "Cyan", DateTime.Now,"Mirku, musíme si domluvit, co kdo dělá, jinak to nestihnem.","Naživu");
            Dogs.Add(d);
            d.PropertyChanged += DogChanged;
        }

        private void DogChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog? dog = sender as Dog;
            MessageBox.Show(e.PropertyName);
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if ((permissions & (long)Permissions.DOGS_DELETE) > 0)
            {
                List<Dog> selectedDogs = new List<Dog>();

                foreach (Dog dog in dataGrid.SelectedItems)
                {
                    selectedDogs.Add(dog);
                }

                foreach (Dog dog in selectedDogs)
                {
                    Dogs.Remove(dog);
                } 
            }
        }
    }
}
