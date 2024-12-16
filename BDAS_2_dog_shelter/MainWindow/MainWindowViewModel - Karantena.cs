using BDAS_2_dog_shelter.Add.Karantena;
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
        private RelayCommand KarantenaadhCMD;
        private RelayCommand<object> KarantenarmhCMD;
        private RelayCommand<object> KarantenaedhCMD;
        private int _karantenaSelectedIndex = -1;
        public ICommand cmdKarantenaAdd => KarantenaadhCMD ??= new RelayCommand(CommandKarantenaAdd, () => Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_INSERT));
        public ICommand cmdKarantenaRm => KarantenarmhCMD ??= new RelayCommand<object>(CommandKarantenaRemove, (p) => p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_DELETE)&& KarantenaSI>0);
        public ICommand cmdKarantenaEd => KarantenaedhCMD ??= new RelayCommand<object>(CommandKarantenaEdit, (p) => p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_UPDATE) && KarantenaSI > 0);
        public ObservableCollection<Quarantine> Karanteny { get; set; } = new();
        

        public int KarantenaSI { get => _karantenaSelectedIndex; set { if (_karantenaSelectedIndex != value) { _karantenaSelectedIndex = value; KarantenaedhCMD?.NotifyCanExecuteChanged(); KarantenarmhCMD?.NotifyCanExecuteChanged(); } } }


        private void CommandKarantenaEdit(object? obj)
        {
            KarantenaAdd s = new(((IEnumerable)obj).Cast<Quarantine>().First());
            s.ShowDialog();
        }

        private void CommandKarantenaRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.KARANTENA_DELETE) > 0)
            {
                List<Quarantine> e = new List<Quarantine>();
                foreach (Quarantine d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Quarantine shelter in e)
                {
                    Karanteny.Remove(shelter);
                }
            }
        }

        private void CommandKarantenaAdd()
        {
            KarantenaAdd s = new KarantenaAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Karanteny.Add(((AddKarantenaViewModel)s.DataContext).karantena);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_DELETE)) Karanteny.Last().PropertyChanged += KarantenaChanged;
            }
        }

        private void LoadKarantena(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KARANTENA_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_Karantena,DATUM_ZAC_KARANTENY,DATUM_KON_KARANTENY from W_KARANTENA";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Karanteny.Add(new(v.GetInt32(0), v.GetDateTime(1), v.GetDateTime(2)));
                            }
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void KarantenaChanged(object? sender, PropertyChangedEventArgs e)
        {
            Quarantine? dog = sender as Quarantine;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveKarantena(dog);
            }
        }

        private async Task SaveKarantena(Quarantine utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_KARANTENA", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_KARANTENA", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_DATUM_ZAC_KARANTENY", OracleDbType.Date, utulek.BeginOfDate, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_DATUM_KON_KARANTENY", OracleDbType.Date, utulek.EndOfDate, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_Karantena";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Karanteny.CollectionChanged -= Karantena_CollectionChanged;
                Karanteny.Clear();
                LoadKarantena(permissions);
                Karanteny.CollectionChanged += Karantena_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Karantena_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Quarantine dog in e.NewItems ?? new List<Quarantine>())
            {
                await SaveKarantena(dog);

            }

            foreach (Quarantine dog in e.OldItems ?? new List<Quarantine>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from Karantena where id_Karantena=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Karanteny.CollectionChanged -= Karantena_CollectionChanged;
                        Karanteny.Clear();
                        LoadKarantena(permissions);
                        Karanteny.CollectionChanged += Karantena_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}