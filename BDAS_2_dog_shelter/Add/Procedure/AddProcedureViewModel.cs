using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Procedure
{
    internal class AddProcedureViewModel
    {
        private Tables.Procedure d;
        private int? iD;
        private string name;
        private string description;
        private int? zdrzaznam;
        private int? pesID;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && description is not null and not "");

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.id = iD;
            d.ProcName = name;
            d.DescrName = description;
            OkClickFinished?.Invoke();
        }

        public string ProcName { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string DescrName { get => description; set { description = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int? id { get => iD;  set => iD = value; }
        public int? ZdrZaznam { get => zdrzaznam;  set { zdrzaznam = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } } 
        public List<Tables.Medical_Record> ZdrZaznamy { get;  set; }
        public Tables.Medical_Record Vzaznam { get;  set; }
        public List<Tables.Dog> dogs { get; set; }

        public Tables.Procedure procedure => d;
        public Tables.Dog Pes { get; set; }

        public AddProcedureViewModel(Tables.Procedure d, List<Tables.Medical_Record>? zaznamy, List <Tables.Dog> dogs)
        {
            this.d = d;
            iD = d.id;
            name = d.ProcName;
            description = d.DescrName;
            zdrzaznam = d.ZdrZaznamid;
            ZdrZaznamy = zaznamy;
            this.dogs = dogs;

        }
    }
}