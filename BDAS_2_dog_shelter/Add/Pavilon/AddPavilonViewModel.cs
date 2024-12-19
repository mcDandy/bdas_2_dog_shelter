using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Pavilon
{
    internal class AddPavilonViewModel
    {
        private Tables.Pavilion d;
        private int? iD;
        private string name;
        private int capacity;
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => true);
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
            Pocet = d.CapacityPav;
            ID = d.id;
        }
    }
}