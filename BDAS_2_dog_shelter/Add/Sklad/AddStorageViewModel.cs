using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Storage
{
    internal class AddStorageViewModel
    {
        private Tables.Storage d;
        RelayCommand okCommand;
        private string stype;
        private int cap;
        private string name;
        private int? iD;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => street is not null and not "" /*&& psc is not null and not < 0*/ );

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
           d.Type = sType;
            d.Capacity = Capacity;
            d.Name = Name;
            d.id = ID;
            OkClickFinished?.Invoke();
        }

        public string sType { get => stype;  set { stype = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Capacity { get => cap;  set { cap = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Name { get => name;  set => name = value; }
        public int? ID { get => iD;  set => iD = value; }
        public Tables.Storage Storage => d;

        public AddStorageViewModel(Tables.Storage d)
        {
            this.d = d;
            sType = d.Type;
            Capacity = d.Capacity;
            Name = d.Name;
            ID = d.id;
        }
    }
}