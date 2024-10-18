using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    //ulice,mesto,psc,cislopopisne
    public class Adress : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                if (_street != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Street)));
                    _street = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Street)));
                }
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(City)));
                    _city = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
                }
            }
        }
        private string _psc;
        public string Psc
        {
            get => _psc;
            set
            {
                if (_psc != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Psc)));
                    _psc = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Psc)));
                }
            }
        }
        private int _number;
        public int Number
        {
            get => _number;
            set
            {
                if (_number != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Number)));
                    _number = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
                }
            }
        }
        public Adress() { _street = ""; _city = ""; _psc = ""; _number = 0; }
        public Adress(string street, string city, string psc, int number) { _street = street; _city = city; _psc = psc; _number = number; }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
