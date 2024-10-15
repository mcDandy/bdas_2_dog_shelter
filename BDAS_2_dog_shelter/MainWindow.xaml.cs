using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            Dog d = new Dog("test");
            Dogs.Add(d);
            d.PropertyChanged += DogChanged;
        }

        private void DogChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog dog = (Dog)sender;
            MessageBox.Show(e.PropertyName);
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
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
