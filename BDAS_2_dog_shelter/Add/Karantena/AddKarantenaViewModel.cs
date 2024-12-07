using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Karantena
{
    internal class AddKarantenaViewModel
    {
        private Tables.Quarantine d;
        private int? iD;
        private DateTime zacatek;
        private DateTime konec;
       
        RelayCommand okCommand;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => zacatek < konec );

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.BeginOfDate = zacatek;
            d.EndOfDate = konec;
            d.id = iD;
            OkClickFinished?.Invoke();
        }
        public DateTime BeginOfDate { get => zacatek; set => zacatek = value; }
        public DateTime EndOfDate { get => zacatek; set => zacatek = value; }
        public int? ID { get => iD;  set => iD = value; }
        public Tables.Quarantine karantena => d;

        public AddKarantenaViewModel(Tables.Quarantine d)
        {
            this.d = d;
            zacatek = d.BeginOfDate;
            konec = d.EndOfDate;
            ID = d.id;
        }
    }
}