using BDAS_2_dog_shelter.Add.feed;
using BDAS_2_dog_shelter.Add.Hracka;
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
        private RelayCommand uadfCMD;
        private RelayCommand<object> urmfCMD;
        private RelayCommand<object> uedfCMD;
        private int _foodSelectedIndex=-1;

        public ICommand cmdFAdd => uadfCMD ??= new RelayCommand(CommandFoodAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_INSERT)));
        public ICommand cmdFRm => urmfCMD ??= new RelayCommand<object>(CommandFoodRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_DELETE)) && FoodSI > -1);
        public ICommand cmdFEd => uedfCMD ??= new RelayCommand<object>(CommandFoodEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_UPDATE)) && FoodSI > -1);
        public ObservableCollection<Feed> Krmiva { get; set; } = new();


        public int FoodSI { get => _foodSelectedIndex; set { if (_foodSelectedIndex != value) { _foodSelectedIndex = value; urmfCMD.NotifyCanExecuteChanged(); uedfCMD.NotifyCanExecuteChanged(); } } }



        private void CommandFoodEdit(object? obj)
        {
            FeedAdd s = new(((IEnumerable)obj).Cast<Feed>().First(),Storages.ToList());
            s.ShowDialog();
        }

        private void CommandFoodRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.KRMIVO_DELETE) > 0)
            {
                List<Hracka> e = new List<Hracka>();
                foreach (Hracka d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Hracka shelter in e)
                {
                    Hracky.Remove(shelter);
                }
            }
        }

        private void CommandFoodAdd()
        {
            HrackaAdd s = new HrackaAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Hracky.Add(((AddHrackaViewModel)s.DataContext).Hracka);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_UPDATE)) Hracky.Last().PropertyChanged += FoodChanged;
            }
        }

        private void LoadFood(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.KRMIVO_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_krmivo,nazev,user,id_sklad from W_KRMIVO";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Hracky.Add(new(v.GetInt32(0), v.GetString(1), v.GetInt32(2), v.GetInt32(3)));
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

        private async void FoodChanged(object? sender, PropertyChangedEventArgs e)
        {
            Feed? dog = sender as Feed;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveFood(dog);
            }
        }

        private async Task SaveFood(Feed utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_KRMIVO", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_KRMIVO", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.Nazev, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_POCET", OracleDbType.Decimal, utulek.Pocet, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ID_SKLAD", OracleDbType.Decimal, utulek.SkladID, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_KRMIVO";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Krmiva.CollectionChanged -= Food_CollectionChanged;
                Krmiva.Clear();
                LoadFood(permissions);
                Krmiva.CollectionChanged += Food_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Food_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Feed dog in e.NewItems ?? new List<Feed>())
            {
                await SaveFood(dog);

            }

            foreach (Feed dog in e.OldItems ?? new List<Feed>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from krmivo where id_krmivo=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();
                    }

                    catch (Exception ex)//something went wrong
                    {
                        Krmiva.CollectionChanged -= Food_CollectionChanged;
                        Krmiva.Clear();
                        LoadFood(permissions);
                        Krmiva.CollectionChanged += Food_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}
