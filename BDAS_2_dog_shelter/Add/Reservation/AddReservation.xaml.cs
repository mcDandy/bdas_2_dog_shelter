using System.Windows;

namespace BDAS_2_dog_shelter.Add.Reservation
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class ReservationAdd : Window
    {
        public ReservationAdd()
        {
            InitializeComponent();
            Tables.Reservation d = new();
            DataContext = new AddReservationViewModel(d);
            ((AddReservationViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public ReservationAdd(Tables.Reservation d)
        {
            InitializeComponent();
            DataContext = new AddReservationViewModel(d);
            ((AddReservationViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
