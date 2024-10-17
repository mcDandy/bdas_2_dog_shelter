using System.ComponentModel;

namespace BDAS_2_dog_shelter
{
    public class Reservation : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private string _date_of_receipt;
        public string DateOfReceipt
        {
            get => _date_of_receipt;
            set
            {
                if (_date_of_receipt != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("DateOfReceipt"));
                    _date_of_receipt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateOfReceipt"));
                }
            }
        }
        private string _date_of_transfer;
        public string DateOfTransfer
        {
            get => _date_of_transfer;
            set
            {
                if (_date_of_transfer != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs("DateOfTransfer"));
                    _date_of_transfer = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateOfTransfer"));
                }
            }
        }
        public Reservation() { _date_of_receipt = ""; _date_of_transfer = ""; }
        public Reservation(string dateofreceipt, string dateoftransfer) { _date_of_receipt = dateofreceipt; _date_of_transfer = dateoftransfer; }
        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
