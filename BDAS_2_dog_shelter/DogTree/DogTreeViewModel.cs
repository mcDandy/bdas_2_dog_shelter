using BDAS_2_dog_shelter.Tables;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.DogTree
{
    internal class DogTreeViewModel : INotifyPropertyChanged
    {
        private Dog _dog;

        public DogTreeViewModel(Dog dog)
        {
            Dog = dog;
            LoadDogTree(); // Načíst data
        }

        public Dog Dog
        {
            get => _dog;
            set
            {
                if (_dog != value)
                {
                    _dog = value;
                    OnPropertyChanged(nameof(Dog));
                    OnPropertyChanged(nameof(DogName));
                    OnPropertyChanged(nameof(FatherName));
                    OnPropertyChanged(nameof(MotherName));
                    // Další vlastnosti, pokud potřebujete
                }
            }
        }

        public string DogName => Dog?.Name ?? "No name";
        public string FatherName => Dog?.Otec?.Name ?? "No father";
        public string MotherName => Dog?.Matka?.Name ?? "No mother";

        public BitmapSource DogImage => Dog?.Obrazek; // Přidáno pro obrázek

        private void LoadDogTree()
        {
            // Může se implementovat další logika pro načítání dat
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}