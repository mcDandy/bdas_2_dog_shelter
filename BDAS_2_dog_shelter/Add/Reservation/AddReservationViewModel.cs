using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
namespace BDAS_2_dog_shelter.Add.Reservation
{
    internal class AddReservationViewModel
    {
        private Tables.Reservation d;
        
        RelayCommand okCommand;
        private DateTime dot;
        private DateTime dor;
        private int? iD;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok );

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
           d.DateOfTransfer = dot;
            d.DateOfReceipt = dor;
            d.id = iD;
            OkClickFinished?.Invoke();
        }

        public DateTime DateTransfer { get => dot;  set { dot = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public DateTime DateReceipt { get => dor;  set { dor = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public Tables.Reservation Reservation => d;
        public List<Tables.Dog> Dogs { get; set; }

        public AddReservationViewModel(Tables.Reservation d)
        {
            this.d = d;
            DateTransfer=d.DateOfTransfer;
            DateReceipt = d.DateOfReceipt;
        }
    }
}