using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Shelter : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Name)));
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        private string _telephone;
        public string Telephone
        {
            get => _telephone;
            set
            {
                if (_telephone != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Telephone)));
                    _telephone = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Telephone)));
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Email)));
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Email)));
                }
            }
        }

        public Shelter()
        {
            _name = "";
            _telephone = "";
            _email = "";
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}