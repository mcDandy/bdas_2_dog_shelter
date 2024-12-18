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
        private int? sklad;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && sklad is not null and not < 0 && pocet is not < 0);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Nazev = name;
            d.Pocet= pocet;
            d.SkladID= sklad ?? 0;
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public int Pocet { get => pocet; set => pocet = value; }
        public int? ID { get => iD; set => iD = value; }
        public int? SkladID { get => sklad; set { sklad = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public Feed feed => d;

        public Feed Krmiva { get; set; }

        public AddFeedViewModel(Feed d, List<Tables.Storage> storages)
        {
            this.d = d;
            Nazev = d.Nazev;
            Pocet = d.Pocet;
            ID = d.id;
            SkladID = d.SkladID;
        }
        public AddFeedViewModel(Feed d)
        {
            this.d = d;
            Nazev = d.Nazev;
            Pocet = d.Pocet;
            ID = d.id;
            SkladID = d.SkladID;
        }
    }
}