using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.feed
{
    internal class AddFeedViewModel
    {
        private Feed d;

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
            d.Nazev = name;
            d.Pocet= pocet;
            d.Sklad = sklad;
            d.SkladID= sklad?.id??0;
            d.id = iD;
            Krmiva = d;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Pocet { get => pocet; set => pocet = value; }
        public int? ID { get => iD; set => iD = value; }
        public Tables.Storage? Sklad { get => sklad; set { sklad = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        public List<Tables.Storage> Sklady { get; private set; }

        public Feed feed => d;



        public Feed Krmiva { get; set; }

        public AddFeedViewModel(Feed d, List<Tables.Storage> storages)
        {
            this.d = d;
            Nazev = d.Nazev;
            Pocet = d.Pocet;
            ID = d.id;
            Sklad = d.Sklad;
            Sklady = storages.Where(e => e.Type == "k").ToList();
        }
    }
}