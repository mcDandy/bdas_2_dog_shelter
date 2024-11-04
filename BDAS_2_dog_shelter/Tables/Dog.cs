using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.Tables
{
    public class Dog : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _name;
        internal readonly int ID;
        static int sequence = 0;
        private BitmapSource _obrazek;
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
        public BitmapSource Obrazek
        {
            get => _obrazek;
            set
            {
                if (_obrazek != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Obrazek)));
                    _obrazek = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Obrazek)));
                }
            }
        }
        private int _age;
        public int Age
        {
            get => _age;
            set
            {
                if (_age != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Age)));
                    _age = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Age)));
                }
            }
        }
        public string _body_color;
        public string BodyColor
        {
            get => _body_color;
            set
            {
                if (_body_color != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(BodyColor)));
                    _body_color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BodyColor)));
                }
            }
        }
        public DateTime _datum_prijeti;
        public DateTime DatumPrijeti
        {
            get => _datum_prijeti;
            set
            {
                if (_datum_prijeti != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DatumPrijeti)));
                    _datum_prijeti = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DatumPrijeti)));
                }
            }
        }
        public string _duvod_prijeti;
        public string DuvodPrijeti
        {
            get => _duvod_prijeti;
            set
            {
                if (_duvod_prijeti != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DuvodPrijeti)));
                    _duvod_prijeti = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DuvodPrijeti)));
                }
            }
        }
        public string _stav_pes;
        public string StavPes
        {
            get => _stav_pes;
            set
            {
                if (_stav_pes != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(StavPes)));
                    _stav_pes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StavPes)));
                }
            }
        }
        public int _utulekId;
        public int UtulekId
        {
            get => _utulekId;
            set
            {
                if (_utulekId != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(UtulekId)));
                    _utulekId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UtulekId)));
                }
            }
        }
        public Dog() { _name = ""; _age = 0; _body_color = ""; _datum_prijeti = DateTime.Now; _duvod_prijeti = ""; _stav_pes = ""; }
        public Dog(string name, int age, string bodycolor, DateTime datumPrijeti, string duvodPrijeti,string stavPes) 
        { 
            _name = name;
            _age = age;
            _body_color = bodycolor;
            _datum_prijeti = datumPrijeti;
            _duvod_prijeti = duvodPrijeti;
            _stav_pes = stavPes;
            ID = sequence++;
        }public Dog(int ID,string name, int age, string bodycolor, DateTime datumPrijeti, string duvodPrijeti,string stavPes) 
        { 
            _name = name;
            _age = age;
            _body_color = bodycolor;
            _datum_prijeti = datumPrijeti;
            _duvod_prijeti = duvodPrijeti;
            _stav_pes = stavPes;
            this.ID = ID;
            if(sequence<=ID)sequence = ID+1;
        }


        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}