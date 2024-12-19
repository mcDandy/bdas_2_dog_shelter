using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BDAS_2_dog_shelter.Tables;


namespace BDAS_2_dog_shelter.Add.Dog
{
    internal partial class AddDogViewModel : ObservableObject
    {
        public List<Tables.Dog> Psi { get; private set; }
        public List<Quarantine> Karanteny { get; internal set; }
        public List<Tables.Owner> Majitele { get; internal set; }
        public AddDogViewModel(Tables.Dog d, List<Tables.Shelter> utulky, List<Tables.Dog> psi, List<Tables.Owner> owners, List<Quarantine> quarantines) {
            Psi = psi;
            Majitele = owners;
            Majitele = Majitele.Prepend(new("<bez majitele>","","","")).ToList();
            Psi = Psi.Prepend(new()).ToList();
            Psi[0].Name = "Bez rodiče";
            Karanteny = quarantines;
            Name = d.Name;
            BodyColor = d.BodyColor;
            Duvod = d.DuvodPrijeti;
            Stav = d.StavPes;
            Age = d.Age;
            Obrazek = d.Obrazek;
            Filename = d.FileName;
            Majtel = d.Majitel;
            Utulek = utulky;
            int i = 0;
            SelectedUT = utulky.Where(a => d?.UtulekId == a.id).FirstOrDefault();
            Dog = d;
            Karantena = Karanteny.Where(a => d?.KarantenaId == a.id).FirstOrDefault();
            selectedM = Psi.Where(a => d?.MatkaId == a?.ID).FirstOrDefault();
            selectedO = Psi.Where(a => d?.OtecId == a?.ID).FirstOrDefault();
            //if (Obrazek is null) Obrazek = new();
        }
        public Tables.Dog Dog { get; }
        public delegate void OkDogAddEditDone();
        public event OkDogAddEditDone? OkClickFinished;
        private RelayCommand okCommand;
        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && bodycolor is not null and not "" && duvod is not null and not "" && stav is not null and not "");


        private string name;

        public string Name { get => name; set { SetProperty(ref name, value); if(okCommand is not null)okCommand.NotifyCanExecuteChanged(); } }

        private string bodycolor;

        public string BodyColor { get => bodycolor; set { SetProperty(ref bodycolor, value); if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        private string duvod;

        public string Duvod { get => duvod; set { SetProperty(ref duvod, value); if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        private string stav;

        public string Stav { get => stav; set { SetProperty(ref stav, value); if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        private int age = 0;

        public int Age { get => age; set { SetProperty(ref age, value); } }

        public List<Tables.Shelter> Utulek {
            get; init;
        }

        private Tables.Shelter selectedUT;

        public Tables.Shelter SelectedUT { 
            get => selectedUT;
            set => SetProperty(ref selectedUT, value); 
        }
        private Tables.Dog selectedM;

        public Tables.Dog SelectedM { 
            get => selectedM;
            set => SetProperty(ref selectedM, value); 
        }
        
        private Tables.Dog selectedO;

        public Tables.Dog SelectedO { 
            get => selectedO;
            set => SetProperty(ref selectedO, value); 
        }


 

        private void Ok()
        {
            Dog.Obrazek = Obrazek;
            Dog.UtulekId = SelectedUT.id;
            Dog.Utulek = SelectedUT;
            Dog.StavPes = Stav;
            Dog.Name = Name;
            Dog.Age = Age;
            Dog.BodyColor = BodyColor;
            Dog.DatumPrijeti = Date??DateTime.Now;
            Dog.DuvodPrijeti = Duvod;
            Dog.FileName = Filename;
            Dog.MatkaId = SelectedM?.ID;
            Dog.Matka = selectedM;
            Dog.Otec = selectedO;
            Dog.OtecId = SelectedO?.ID;
            Dog.MajtelId = Majtel?.id;
            Dog.KarantenaId = Karantena?.id;
            Dog.Karantena = Karantena;

            OkClickFinished?.Invoke();
        }

        private DateTime? date;
        private Quarantine kar;

        public DateTime? Date { get => date; set => SetProperty(ref date, value); }
        public BitmapSource Obrazek { get; set; }
        public string? Filename { get; set; }
        public Tables.Owner Majtel { get; set; }
        public Quarantine Karantena { get => kar; set => SetProperty(ref kar, value); }
    }
    
}
