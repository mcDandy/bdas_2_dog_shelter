using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Shelter;
using BDAS_2_dog_shelter.Add.Storage;
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
        private RelayCommand sadCMD;
        private RelayCommand<object> srmCMD;
        private RelayCommand<object> sedCMD;
        public ICommand cmdSAdd => uadCMD ??= new RelayCommand(CommandSkladAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_INSERT)));
        public ICommand cmdSEd => uedCMD ??= new RelayCommand<object>(CommandSkladEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN,   Permissions.SKLAD_UPDATE)));
        public ICommand cmdSRm => urmCMD ??= new RelayCommand<object>(CommandSkladRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_DELETE)));
        public ObservableCollection<Tables.Storage> Storages { get; set; } = new();

        private void CommandSkladEdit(object? obj)
        {
            StorageAdd da = new(((IEnumerable)obj).Cast<Tables.Storage>().First());
            da.ShowDialog();
        }

        private void CommandSkladRemove(object? SelectedShelters)
        {
            if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.SKLAD_DELETE))
            {
                List<Tables.Storage> e = new List<Tables.Storage>();
                foreach (Tables.Storage d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Tables.Storage shelter in e)
                {
                    Storages.Remove(shelter);
                }
            }
        }
        

        private void CommandSkladAdd()
        {
            ShelterAdd s = new ShelterAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                //Storages.Add(((AddStorageViewModel)s.DataContext).Sklad);
                if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.SKLAD_UPDATE)) Storages.Last().PropertyChanged += StorageChanged;
            }
        }

        private void LoadStorages(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_sklad,kapacita,nazev_skladu,typ_skladu from sklad";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Storages.Add
                                (
                                new(
                                    v.GetInt32(0),
                                    v.GetInt32(1),
                                    v.GetString(2),
                                    v.IsDBNull(3) ? null : v.GetString(3)
                                ));

                            if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.SKLAD_UPDATE)) Storages.Last().PropertyChanged += ShelterChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void StorageChanged(object? sender, PropertyChangedEventArgs e)
        {
            Tables.Storage? dog = sender as Tables.Storage;
            using (OracleCommand cmd = con.CreateCommand())
            {

                    await SaveStorage(dog);
            }
        }

        private async Task SaveStorage(Tables.Storage utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_SKLAD", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_SKLAD", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_KAPACITA", OracleDbType.Decimal, utulek.Capacity,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_NAZEV_SKLADU", OracleDbType.Varchar2, utulek.Name, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_TYP_SKLADU", OracleDbType.Varchar2, utulek.Type, ParameterDirection.Input));
                    
                    cmd.CommandText = "INS_SET.IU_SKLAD";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Shelters.CollectionChanged -= Sklad_CollectionChanged;
                LoadShelters(permissions);
                Shelters.CollectionChanged += Sklad_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Sklad_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Tables.Storage dog in e.NewItems ?? new List<Tables.Storage>())
            {
                await SaveStorage(dog);

            }

            foreach (Tables.Storage dog in e.OldItems ?? new List<Tables.Storage>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from sklad where id_usklad=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Storages.CollectionChanged -= Sklad_CollectionChanged;
                        LoadStorages(permissions);
                        Storages.CollectionChanged += Sklad_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    } 
}
