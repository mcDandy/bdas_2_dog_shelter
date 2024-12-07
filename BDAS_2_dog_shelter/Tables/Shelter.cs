using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Shelter : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
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
        internal int? ID { get; set; } = null;
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

        private string? _email;
        public string? Email
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
        private int? _addrID;
        public int? AddressID
        {
            get => _addrID;
            set
            {
                if (_addrID != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(AddressID)));
                    _addrID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AddressID)));
                }
            }
        }
        public Adress? Adresa { get; set; }
        public Shelter()
        {
            _name = "";
            _telephone = "";
            _email = "";
        }

        public Shelter(int utId, string name, string telephone, string? email, int adrID)
        {
            ID = utId;
            _name = name;
            _telephone = telephone;
            _email = email;
            _addrID= adrID;
            
        }
        public override string ToString()
        {
            return Name;
        }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}