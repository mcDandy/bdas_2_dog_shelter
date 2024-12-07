using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Hracka
{
    internal class AddHrackaViewModel
    {
        private Tables.Hracka d;
        private string name;
        private int pocet;
        private int? iD;
        private int? sklad;
        Tables.Storage storage;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && sklad is not null and not < 0 && pocet is not <0);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Nazev = name;
            d.Pocet = pocet;
            d.SkladID = storage?.id;
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name;  set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Pocet { get => pocet;  set => pocet = value; }
        public int? ID { get => iD;  set => iD = value; }
        public int? SkladID { get => sklad;  set { sklad = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        public List<Tables.Storage> Sklady { get; set; }

        public Tables.Hracka Hracka => d;

        public Tables.Storage Storage { get => storage; set => storage = value; }

        public AddHrackaViewModel(Tables.Hracka d, List<Tables.Storage> storages)
        {
            this.d = d;
            Nazev = d.Nazev;
            this.Pocet = d.Pocet;
            ID = d.id;
            SkladID = d.SkladID;
            storage = d.Sklad;
            this.Sklady = storages;
        }
    }
}