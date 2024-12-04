using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Reservation;
using BDAS_2_dog_shelter.Add.Shelter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

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
            this.DataContext = new AddReservationViewModel(d);
            ((AddAdressViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public ReservationAdd(Tables.Reservation d)
        {
            InitializeComponent();
            this.DataContext = new AddReservationViewModel(d);
            ((AddAdressViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
