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
        private int? iD;
        private Tables.Storage? sklad;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && sklad is not null && pocet is not < 0);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.MedicalName = name;
            d.Count = pocet;
            d.Sklad = sklad;
            d.SkladID = sklad?.id ?? 0;
            d.id = iD;
            MedEquip = d;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Pocet { get => pocet; set => pocet = value; }
        public int? ID { get => iD; set => iD = value; }
        public Tables.Storage? Sklad { get => sklad; set { sklad = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        public List<Tables.Storage> Sklady { get; private set; }

        public Medical_Equipment medicalEquipment => d;



        public Medical_Equipment MedEquip { get; set; }

      public MedicalEquipmentViewModelAdd(Medical_Equipment s, List<Tables.Storage> storages)
{
    this.d = s ?? throw new ArgumentNullException(nameof(s));
    Nazev = s.MedicalName;
    Pocet = s.Count;
    ID = s.id;
    Sklad = s.Sklad;
    Sklady = storages?.Where(e => e.Type == "m").ToList() ?? new List<Tables.Storage>();
}
    }
}