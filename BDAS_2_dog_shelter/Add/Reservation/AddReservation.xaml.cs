using BDAS_2_dog_shelter.Add.Shelter;
using BDAS_2_dog_shelter.Tables;
using System.Collections.Generic;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.Reservation
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class AddReservation : Window
    {
        public AddReservation()
        {
        }

        public AddReservation(Tables.Reservation d)
        {
            InitializeComponent();
            DataContext = new AddReservationViewModel(d);
            ((AddReservationViewModel)DataContext).OkClickFinished += () => DialogResult = true;

        }
        public AddReservation(List < Tables.Dog > storages)
        {
            InitializeComponent();
            Tables.Reservation d = new();
            DataContext = new AddReservationViewModel(d, storages);
            ((AddReservationViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public AddReservation(Tables.Reservation d, List < Tables.Dog > storages)
        {
            InitializeComponent();
            DataContext = new AddReservationViewModel(d, storages);
            ((AddReservationViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
