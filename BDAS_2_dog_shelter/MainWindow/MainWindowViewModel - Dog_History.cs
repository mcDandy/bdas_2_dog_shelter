using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Dog_Historie;
using BDAS_2_dog_shelter.Add.Hracka;
using BDAS_2_dog_shelter.Add.Shelter;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static BDAS_2_dog_shelter.Secrets;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private RelayCommand HistoryadCMD;
        private RelayCommand<object> HistoryrmCMD;
        private RelayCommand<object> HistoryedCMD;
        public ICommand cmdHistoryAdd => HistoryadCMD ??= new RelayCommand(CommandHrackaAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_INSERT)));
        public ICommand cmdHistoryRm => HistoryrmCMD ??= new RelayCommand<object>(CommandPesHistoryRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_DELETE)));
        public ICommand cmdHistoryEd => HistoryedCMD ??= new RelayCommand<object>(CommandPesHistoryEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)));
        public ObservableCollection<Dog_History> Historie { get; set; } = new();

        private void CommandPesHistoryEdit(object? obj)
        {
            Dog_Historie_Add s = new Dog_Historie_Add((Dog_History)obj);
            s.ShowDialog();
        }

        private void CommandPesHistoryRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.HISTORIE_PSA_DELETE) > 0)
            {
                List<Dog_History> e = new List<Dog_History>();
                foreach (Dog_History d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Dog_History history in e)
                {
                    Historie.Remove(history);
                }
            }
        }

        private void CommandDogHistoryAdd()
        {
            HrackaAdd s = new HrackaAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Hracky.Add(((AddHrackaViewModel)s.DataContext).Hracka);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_UPDATE)) Hracky.Last().PropertyChanged += HrackaChanged;
            }
        }

        private void LoadPesHistory(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HISTORIE_PSA_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_hracka,nazev,pocet,id_sklad from HRACKA";
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

        private async void HistorieChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog_History? dog = sender as Dog_History;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveHracka(dog);
            }
        }

        private async Task Savehistory(Dog_History utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_HRACKA", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_HRACKA", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.Nazev, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_POCET", OracleDbType.Decimal, utulek.Pocet, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ID_SKLAD", OracleDbType.Decimal, utulek.SkladID, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_HISTORIE_PSA";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Hracky.CollectionChanged -= DogHistory_CollectionChanged;
                LoadPesHistory(permissions);
                Hracky.CollectionChanged += DogHistory_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void DogHistory_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Dog_History dog in e.NewItems ?? new List<Dog_History>())
            {
                await SaveHracka(dog);

            }

            foreach (Dog_History dog in e.OldItems ?? new List<Dog_History>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from hracka where id_hracka=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Hracky.CollectionChanged -= Hracka_CollectionChanged;
                        LoadDogs(permissions);
                        Hracky.CollectionChanged += Hracka_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}
