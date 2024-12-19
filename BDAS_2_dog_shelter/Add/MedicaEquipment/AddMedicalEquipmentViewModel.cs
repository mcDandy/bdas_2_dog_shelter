using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.MedicaEquipment
{
    internal class MedicalEquipmentViewModelAdd
    {
        private Medical_Equipment d;
        RelayCommand okCommand;
        private string name;
        private int pocet;
        private int? sklad;
        private int? iD;
        private Tables.Storage storage;


        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => true);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.MedicalName = name;
            d.Count = pocet;
            d.SkladID = storage?.id ?? 0;
            d.Sklad = storage;
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
            set
            {
                pocet = value;
                okCommand?.NotifyCanExecuteChanged();
            }
        }

        public int? ID
        {
            get => iD;
            set
            {
                iD = value;
                okCommand?.NotifyCanExecuteChanged();
            }
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

        public Medical_Equipment medicalEquipment => d;
          public Tables.Storage Storage
        {
            get => storage;
            set { storage = value; sklad = storage.id; }
        }



        public Medical_Equipment MedEquip { get; set; }

        public MedicalEquipmentViewModelAdd(Tables.Medical_Equipment d, List<Tables.Storage> storages)
        {
            this.d = d ?? new Tables.Medical_Equipment(); // Zajištění, že d není null
            Nazev = d.MedicalName;
            Pocet = d.Count;
            ID = d.id;
            SkladID = d.SkladID;
            storage = d.Sklad;
            Sklady = storages.Where(a=>a.Type == "z").ToList();
        }
    }
}