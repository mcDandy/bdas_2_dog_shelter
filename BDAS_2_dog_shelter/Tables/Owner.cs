using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Owner : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Name"));
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        private string _surname;
        public string Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Surname"));
                    _surname = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Surname"));
                }
            }
        }
        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Phone"));
                    _phone = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Phone"));
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
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("Email"));
                    _email = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
                }
            }
        }
        public Owner() { _name = ""; _surname = ""; _phone = ""; _email = ""; }
        public Owner(string name, string surname, string phone, string email) { _name = name; _surname = surname; _phone = phone; _email = email; }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
