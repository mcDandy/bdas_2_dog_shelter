using BDAS_2_dog_shelter.Add.Reservation;
using BDAS_2_dog_shelter.Add.Shelter;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private RelayCommand radCMD;
        private RelayCommand<object> RezervaceRemoveCMD;
        private RelayCommand<object> RezervaceEditCMD;

        private int _reservationselectedIndex = -1;


        public ICommand cmdRAdd => radCMD ??= new RelayCommand(CommandRezervaceAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_INSERT)));
        public ICommand cmdR0Rm => RezervaceRemoveCMD ??= new RelayCommand<object>(CommandRezervaceRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_DELETE)) && ReservationSI > -1);
        public ICommand cmdREd => RezervaceEditCMD ??= new RelayCommand<object>(CommandRezervaceEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_UPDATE)) && ReservationSI > -1);
        public ObservableCollection<Reservation> Rezervace { get; set; } = new();

        public int ReservationSI { get => _reservationselectedIndex; set { if (_reservationselectedIndex != value) { _reservationselectedIndex = value; RezervaceEditCMD.NotifyCanExecuteChanged(); RezervaceRemoveCMD.NotifyCanExecuteChanged(); } } }

        private void CommandRezervaceEdit(object? obj)
        {
            ReservationAdd s = new(((IEnumerable)obj).Cast<Reservation>().First());
            s.ShowDialog();
        }

        private void CommandRezervaceRemove(object? SelectedShelters)
        {
            if ((Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_DELETE)))
            {
                List<Reservation> e = new List<Reservation>();
                foreach (Reservation d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Reservation shelter in e)
                {
                    Rezervace.Remove(shelter);
                }
            }
        }



        private void CommandRezervaceAdd()
        {
            ReservationAdd s = new ReservationAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Adresses.Add(((AddAdressViewModel)s.DataContext).Adresa);
                if ((Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_INSERT)))
                    Adresses.Last().PropertyChanged += ReservationChanged;
            }
        }

        private void LoadReservations(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_REZERVACE,DATUM_REZERVACE,PREVZETI_PSA from w_REZERVACE";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Rezervace.Add
                                (
                                new(
                                    v.IsDBNull(0) ? null : v.GetInt32(0),
                                    v.GetDateTime(1),
                                    v.GetDateTime(2)
                                ));

                            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.REZERVACE_UPDATE))
                                Adresses.Last().PropertyChanged += ReservationChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void ReservationChanged(object? sender, PropertyChangedEventArgs e)
        {
            Reservation? dog = sender as Reservation;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveReservation(dog);
            }
        }

        private async Task SaveReservation(Reservation utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_REZERVACE", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_REZERVACE", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_DATUM_REZERVACE", OracleDbType.Date, utulek.DateOfReceipt, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_PREVZETI_PSA", OracleDbType.Date, utulek.DateOfTransfer, ParameterDirection.Input));
                    cmd.CommandText = "INS_SET.IU_REZERVACE";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Rezervace.CollectionChanged -= Reservation_CollectionChanged;
                Rezervace.Clear();
                LoadReservations(permissions);
                Rezervace.CollectionChanged += Reservation_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Reservation_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Reservation dog in e.NewItems ?? new List<Reservation>())
            {
                await SaveReservation(dog);

            }

            foreach (Reservation dog in e.OldItems ?? new List<Reservation>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from rezervace where id_rezervace=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Rezervace.CollectionChanged -= Reservation_CollectionChanged;
                        Rezervace.Clear();
                        LoadReservations(permissions);
                        Rezervace.CollectionChanged += Reservation_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}
