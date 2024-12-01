using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.Tables
{
    public class Dog : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _name;
        internal int? ID;
        private BitmapSource _obrazek;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    Debug.WriteLine($"{_name} => ${value} ");
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
            Debug.WriteLine($"{_age} => ${value} ");
                if (_age != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Age)));
                    _age = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Age)));
                }
            }
        }
        private string _body_color;
        public string BodyColor
        {
            get => _body_color;
            set
            {
            Debug.WriteLine($"{_body_color} => ${value} ");
                if (_body_color != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(BodyColor)));
                    _body_color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BodyColor)));
                }
            }
        }
        private DateTime _datum_prijeti;
        public DateTime DatumPrijeti
        {
    get => _datum_prijeti;
            set
            {
    Debug.WriteLine($"{_datum_prijeti} => ${value} ");
                if (_datum_prijeti != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DatumPrijeti)));
                    _datum_prijeti = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DatumPrijeti)));
                }
            }
        }
        private string _duvod_prijeti;
        public string DuvodPrijeti
        {
    get => _duvod_prijeti;
            set
            {
    Debug.WriteLine($"{_duvod_prijeti} => ${value} ");
                if (_duvod_prijeti != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DuvodPrijeti)));
                    _duvod_prijeti = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DuvodPrijeti)));
                }
            }
        }
        private string _stav_pes;
        public string StavPes
        {
            get => _stav_pes;
            set
            {
                if (_stav_pes != value)
                {
                    Debug.WriteLine($"{_stav_pes} => ${value} ");
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(StavPes)));
                    _stav_pes = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StavPes)));
                }
            }
        }
        private int? _utulekId;
        public int? UtulekId
        {
            get => _utulekId;
            set
            {
                if (_utulekId != value)
                {Debug.WriteLine($"{_utulekId} => ${value} ");
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(UtulekId)));
                    _utulekId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UtulekId)));
                }
            }
        } 
        private int? _obrazek_id;
        public int? Obrazek_Id
        {
            get => _obrazek_id;
            set
            {
                if (_obrazek_id != value)
                {Debug.WriteLine($"{_obrazek_id} => ${value} ");
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Obrazek_Id)));
                    _obrazek_id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Obrazek_Id)));
                }
            }
        }
        private int? _karatnenaID;
        public int? KarantenaId
        {
            get => _karatnenaID;
            set
            {
                Debug.WriteLine($"{_karatnenaID} => ${value} ");
                if (_karatnenaID != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(KarantenaId)));
                    _karatnenaID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KarantenaId)));
                }
            }
        }
        private int? _majtelID;
        public int? MajtelId
        {
            get => _majtelID;
            set
            {
                if (_majtelID != value)
                {Debug.WriteLine($"{_majtelID} => ${value} ");
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(MajtelId)));
                    _majtelID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MajtelId)));
                }
            }
        }private int? _otecID;
        public int? OtecId
        {
            get => _otecID;
            set
            {
                if (_otecID != value)
                {Debug.WriteLine($"{_otecID} => ${value} ");
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(OtecId)));
                    _otecID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OtecId)));
                }
            }
        }
        private int? _matkaID;
        public int? MatkaId
        {
            get => _matkaID;
            set
            {
                Debug.WriteLine($"{_matkaID} => ${value} ");
                if (_matkaID != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(MatkaId)));
                    _matkaID = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MatkaId)));
                }
            }
        }
        public Dog() { _name = ""; _age = 0; _body_color = ""; _datum_prijeti = DateTime.Now; _duvod_prijeti = ""; _stav_pes = ""; byte[] b = new byte[256 * 256 * 4]; new Random().NextBytes(b); _obrazek = BitmapSource.Create(256, 256, 96, 96, PixelFormats.Bgra32, null, b, 256*4); }
        public Dog(string name, int age, string bodycolor, DateTime datumPrijeti, string duvodPrijeti, string stavPes, int? utulekid, int? karantenaid, int? majitelid, int? otecid, int? matkaid, BitmapSource OBRAZEK ,int? OBRAZEK_ID) 
        { 
            _name = name;
            _age = age;
            _body_color = bodycolor;
            _datum_prijeti = datumPrijeti;
            _duvod_prijeti = duvodPrijeti;
            _stav_pes = stavPes;
            _obrazek = OBRAZEK;
            _utulekId = utulekid;
            _karatnenaID = karantenaid;
            _majtelID = majitelid;
            _otecID = otecid;
            _majtelID = matkaid;
            _obrazek_id = OBRAZEK_ID;

        }
        public Dog(int? id,string name, int age, string bodycolor, DateTime datumPrijeti, string duvodPrijeti,string stavPes, int? utulekid, int? karantenaid, int? majitelid,int? otecid, int? matkaid, BitmapSource? OBRAZEK, int? OBRAZEK_ID) 
        { 
            ID = id;
            _name = name;
            _age = age;
            _body_color = bodycolor;
            _datum_prijeti = datumPrijeti;
            _duvod_prijeti = duvodPrijeti;
            _stav_pes = stavPes;
            _obrazek = OBRAZEK;
            _utulekId = utulekid;
            _karatnenaID = karantenaid;
            _majtelID = majitelid;
            _otecID = otecid;
            _majtelID = matkaid;
            _obrazek_id = OBRAZEK_ID;
        }

        public string? FileName { get; set; }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
        public override string ToString()
        {
            return $"{Name} ({Age})";
        }
    }
}