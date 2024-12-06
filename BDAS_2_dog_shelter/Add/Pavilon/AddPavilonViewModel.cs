using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Pavilon
{
    internal class AddPavilonViewModel
    {
        private Tables.Pavilion d;
        private int? iD;
        private string name;
        private int capacity;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && iD !=0);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.PavName = name;
            d.CapacityPav = capacity;
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name;  set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Pocet { get => capacity;  set => capacity = value; }
        public int? ID { get => iD;  set => iD = value; }
        public Tables.Pavilion pavilon => d;

        public AddPavilonViewModel(Tables.Pavilion d)
        {
            this.d = d;
            Nazev = d.PavName;
            this.Pocet = d.CapacityPav;
            ID = d.id;
        }
    }
}