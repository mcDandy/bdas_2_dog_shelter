using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace BDAS_2_dog_shelter.Add.Hracka
{
    internal class AddHrackaViewModel
    {
        private Tables.Hracka d;
        private string name = string.Empty; // Inicializace dynamickým způsobem.
        private int pocet;
        private int? iD;
        private int? sklad;
        private Tables.Storage storage;
        private RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, CanExecuteOk);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Nazev = name;
            d.Pocet = pocet;
            d.SkladID = storage?.id ?? 0; // Přiřazení s výchozí hodnotou, pokud storage je nulové
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public string Nazev
        {
            get => name;
            set
            {
                name = value;
                okCommand?.NotifyCanExecuteChanged();
            }
        }

        public int Pocet
        {
            get => pocet;
            set => pocet = value;
        }

        public int? ID
        {
            get => iD;
            set => iD = value;
        }

        public int? SkladID
        {
            get => sklad;
            set
            {
                sklad = value;
                okCommand?.NotifyCanExecuteChanged();
            }
        }

        public List<Tables.Storage> Sklady { get; set; }
        public Tables.Hracka Hracka => d;

        public Tables.Storage Storage
        {
            get => storage;
            set => storage = value;
        }

        public AddHrackaViewModel(Tables.Hracka d, List<Tables.Storage> storages)
        {
            this.d = d ?? new Tables.Hracka(); // Zajištění, že d není null
            Nazev = d.Nazev;
            Pocet = d.Pocet;
            ID = d.id;
            SkladID = d.SkladID;
            storage = d.Sklad;
            Sklady = storages;
            okCommand = new RelayCommand(Ok, CanExecuteOk);
        }

        public AddHrackaViewModel(Tables.Hracka d)
        {
            this.d = d;
            Nazev = d.Nazev;
            Pocet = d.Pocet;
            ID = d.id;
            SkladID = d.SkladID;
            storage = d.Sklad;
            okCommand = new RelayCommand(Ok, CanExecuteOk);
        }

        private bool CanExecuteOk()
        {
            return !string.IsNullOrWhiteSpace(name) && sklad >= 0 && pocet >= 0;
        }
    }
}