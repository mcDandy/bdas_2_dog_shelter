using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Medical_Record
{
    internal class AddMedicalRecViewModel
    {
        private Tables.Medical_Record d;
        private int? iD;
        private DateTime date;
        private int? typProc;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => date >= DateTime.Now);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.id = iD;
            d.DateRec = date;
            d.TypeProc = typProc;
            OkClickFinished?.Invoke();
        }

        public DateTime DateRec { get => date;  set => date = value; }
        public int? ID { get => iD;  set => iD = value; }
        public int? TypeProc { get => typProc;  set { typProc = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } } 
        public Tables.Medical_Record MedicalRec => d;

        public AddMedicalRecViewModel(Tables.Medical_Record d)
        {
            this.d = d;
            date = d.DateRec;
            typProc = d.TypeProc;
            ID = d.id;


        }
    }
}