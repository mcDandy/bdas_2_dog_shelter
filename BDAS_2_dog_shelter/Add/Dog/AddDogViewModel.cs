using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace BDAS_2_dog_shelter.Add.Dog
{
    internal partial class AddDogViewModel : ObservableObject
    {
        public List<Tables.Dog> Psi { get; private set; }
        public List<Tables.Quarantine> Karanteny { get; internal set; }
        public List<Tables.Owner> Majitele { get; internal set; }
        public AddDogViewModel(Tables.Dog d, List<Tables.Dog> psi, List<Tables.Owner> owners, List<Tables.Quarantine> quarantines) {
            Psi = psi;
            Majitele = owners;
            Majitele.Prepend(null);
            Karanteny = quarantines;
            Name = d.Name;
            BodyColor = d.BodyColor;
            Duvod = d.DuvodPrijeti;
            Stav = d.StavPes;
            Age = d.Age;
            Obrazek = d.Obrazek;
            Filename = d.FileName;
            Majtel = d.Majitel;
            int i = 0;
            SelectedUT = Utulek.Select(a => new Tuple<int?, int>(a.Item1, i++)).FirstOrDefault(a => a.Item1 == d.UtulekId).Item2;
            Dog = d;
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

        public List<Tuple<int?, string>> Utulek {
            get 
            {
                if (ulek == null)
                {
                    ulek = [new(null, "<Žádný>")];

                    OracleConnection con = new OracleConnection(ConnectionString);
                    if (con.State == System.Data.ConnectionState.Closed) con.Open();
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = "select id_utulek,nazev from utulek";
                            OracleDataReader v = cmd.ExecuteReader();
                            if (v.HasRows)
                            {
                                while (v.Read())
                                {
                                    ulek.Add(new(v.GetInt32(0), v.GetString(1)));
                                }
                            }
                        }
                        catch (Exception ex)//something went wrong
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                   
                }
                return ulek;
            }
        }

        private List<Tuple<int?, string>> ulek;

        private int selectedUT;

        public int SelectedUT { 
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
            Dog.UtulekId = SelectedUT;
            Dog.StavPes = Stav;
            Dog.Name = Name;
            Dog.Age = Age;
            Dog.BodyColor = BodyColor;
            Dog.DatumPrijeti = Date??DateTime.Now;
            Dog.DuvodPrijeti = Duvod;
            Dog.FileName = Filename;
            Dog.MatkaId = SelectedM?.ID;
            Dog.OtecId = SelectedO?.ID;
            Dog.MajtelId = Majtel?.id;
            Dog.KarantenaId = Karantena?.id;

            OkClickFinished?.Invoke();
        }

        private DateTime? date;

        public DateTime? Date { get => date; set => SetProperty(ref date, value); }
        public BitmapSource Obrazek { get; set; }
        public string? Filename { get; set; }
        public Tables.Owner Majtel { get; set; }
        public Tables.Owner Karantena { get; set; }
    }
}
