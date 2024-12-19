using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Medical_Record
{
    internal class AddMedicalRecViewModel
    {
        private Tables.Medical_Record d;
        private int? iD;
        private DateTime date;
        private int? typProc;
        private List<KeyValueUS> types;
        RelayCommand okCommand;
        private KeyValueUS? typ;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => typ is not null);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.id = iD;
            d.DateRec = date;
            d.Type = Type;
            d.TypeProcId = typ?.Id;
            OkClickFinished?.Invoke();
        }

        public DateTime DateRec { get => date;  set => date = value; }
        public int? ID { get => iD;  set => iD = value; }
        public int? TypeProc { get => typProc;  set { typProc = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } } 
        public Tables.KeyValueUS? Type { get => typ;  set { typ = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } } 
        public Tables.Medical_Record MedicalRec => d;

        public List<KeyValueUS> Types { get => types; set => types = value; }

        public AddMedicalRecViewModel(Tables.Medical_Record d, List<KeyValueUS> types)
        {
            this.d = d;
            date = d.DateRec;
            typProc = d.TypeProcId;
            Types = types;
            ID = d.id;


        }
    }
}