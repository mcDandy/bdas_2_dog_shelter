using System.ComponentModel;

namespace BDAS_2_dog_shelter.Tables
{
    public class Reservation : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private DateTime _date_of_receipt;
        public DateTime DateOfReceipt
        {
            get => _date_of_receipt;
            set
            {
                if (_date_of_receipt != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DateOfReceipt)));
                    _date_of_receipt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfReceipt)));
                }
            }
        }
        private DateTime _date_of_transfer;
        public DateTime DateOfTransfer
        {
            get => _date_of_transfer;
            set
            {
                if (_date_of_transfer != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DateOfTransfer)));
                    _date_of_transfer = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfTransfer)));
                }
            }
        }
        private int? dogId;
        public int? DogId
        {
            get => dogId;
            set
            {
                if (dogId != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(DogId)));
                    dogId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DogId)));
                }
            }
        }
        public Dog Pes { get; set; }
        public Reservation() { _date_of_receipt = DateTime.Now; _date_of_transfer = DateTime.Now; }
        public Reservation(DateTime dateofreceipt, DateTime dateoftransfer) { _date_of_receipt = dateofreceipt; _date_of_transfer = dateoftransfer; }
        public Reservation(int? id,DateTime dateofreceipt, DateTime dateoftransfer) { _date_of_receipt = dateofreceipt; _date_of_transfer = dateoftransfer; }
        public Reservation(int? id, DateTime dateofreceipt, DateTime dateoftransfer, int dogid) { _date_of_receipt = dateofreceipt; _date_of_transfer = dateoftransfer; dogId = dogid; }

        public Reservation(object value1, DateTime dateTime1, DateTime dateTime2, object value2)
        {
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
