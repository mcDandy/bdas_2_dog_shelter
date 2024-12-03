using System;

    using BDAS_2_dog_shelter.Tables;
    using CommunityToolkit.Mvvm.Input;
    using System.Windows.Input;
    using System.Xml.Linq;
using System.Windows;
namespace BDAS_2_dog_shelter.Add.Dog_Historie
{
        internal class AddDogHistorieViewModel
        {
        private Tables.Dog_History d;
        private string name;
        private DateTime? pocet;
        private int? iD;
        private int? sklad;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && sklad is not null and not < 0 && pocet is not < 0);

        public delegate void OkHistorieAddEditDone();
        public event OkHistorieAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public string Nazev { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public DateTime Pocet { get => pocet; set => pocet = value; }
        public int? ID { get => iD; set => iD = value; }
        public int? SkladID { get => sklad; set { sklad = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public Tables.Dog_History historie => d;

        public AddDogHistorieViewModel(Tables.Dog_History d)
        {
            Nazev = d.EventDescription;
            this.Pocet = d.DateOfEvent;
            ID = d.id;
            SkladID = d.typid;
        }
    }
}
