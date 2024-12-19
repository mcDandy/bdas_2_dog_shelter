using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Medical_Equipment
{
    internal class MedicalEquipmentViewModelAdd
    {
        private Tables.Medical_Equipment d;
        private string name;
        private int pocet;
        private int? iD;
        private Tables.Storage sklad;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && sklad is not null and not < 0 && pocet is not <0);

        public delegate void OkMedicalEquipmentAddEditDone();
        public event OkMedicalEquipmentAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.MedicalName = name;
            d.Count = pocet;
            d.SkladID = sklad?.id??0;
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name;  set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Pocet { get => pocet;  set => pocet = value; }
        public int? ID { get => iD;  set => iD = value; }
        public Tables.Storage? Sklad { get => sklad;  set { sklad = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } } 
        public Tables.Medical_Equipment Medical_equipment => d;

        public MedicalEquipmentViewModelAdd(Tables.Medical_Equipment d, List<Tables.Storage> storages)
        {
            this.d = d;
            Nazev = d.MedicalName;
            Pocet = d.Count;
            ID = d.id;
            Sklady = storages.Where(s => s.Type == "z");
            Sklad = d.Sklad;
        }
    }
}