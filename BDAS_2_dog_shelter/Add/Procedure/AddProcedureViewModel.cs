using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Procedure
{
    internal class AddProcedureViewModel
    {
        private Tables.Procedure d;
        private int? iD;
        private string name;
        private string description;
        private int? zdrzaznam;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && description is not null and not "");

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.id = iD;
            d.ProcName = name;
            d.DescrName = description;
            d.ZdrZaznam = zdrzaznam;
            OkClickFinished?.Invoke();
        }

        public string ProcName { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string DescrName { get => description; set { description = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int? id { get => iD;  set => iD = value; }
        public int? ZdrZaznam { get => zdrzaznam;  set { zdrzaznam = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } } 
        public Tables.Procedure procedure => d;

        public AddProcedureViewModel(Tables.Procedure d)
        {
            this.d = d;
            iD = d.id;
            name = d.ProcName;
            description = d.DescrName;
            zdrzaznam = d.ZdrZaznam;

        }
    }
}