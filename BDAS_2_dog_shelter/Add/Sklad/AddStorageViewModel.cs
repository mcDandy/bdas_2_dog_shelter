using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Storage
{
    internal class AddStorageViewModel
    {
        private Tables.Adress d;
        private int number;
        private string street;
        private string city;
        private int? iD;
        private string psc;
        RelayCommand okCommand;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => street is not null and not "" /*&& psc is not null and not < 0*/ );

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
           d.Number = number;
            d.Street = street;
            d.City = city;
            d.Psc = psc;
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public int CP { get => number;  set { number = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string Street { get => street;  set { street = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string City { get => city;  set => city = value; }
        public int? ID { get => iD;  set => iD = value; }
        public string PSC { get => psc;  set => psc = value; }
        public Tables.Adress Adresa => d;

        public AddStorageViewModel(Tables.Adress d)
        {
            this.d = d;
            Street = d.Street;
            CP = d.Number;
            City = d.City;
            ID = d.id;
            PSC = d.Psc;
        }
    }
}