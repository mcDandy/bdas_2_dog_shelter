using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
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
        private RelayCommand sadCMD;
        private RelayCommand<object> srmCMD;
        private RelayCommand<object> sedCMD;
        public ICommand cmdSAdd => uadCMD ??= new RelayCommand(CommandSkladAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_INSERT)));
        public ICommand cmdSRm => urmCMD ??= new RelayCommand<object>(CommandSkladRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_DELETE)));
        public ICommand cmdSEd => uedCMD ??= new RelayCommand<object>(CommandSkladEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_UPDATE)));
        public ObservableCollection<Storage> Storages { get; set; } = new();

        private void CommandSkladEdit(object? obj)
        {
            StorageAdd s = new StorageAdd((Storage)obj);
            s.ShowDialog();
        }

        private void CommandSkladRemove(object? SelectedShelters)
        {
            if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.SKLAD_DELETE))
            {
                List<Storage> e = new List<Storage>();
                foreach (Storage d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Storage shelter in e)
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
                Storages.Add(((AddStorageViewModel)s.DataContext).Sklad);
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
                        cmd.CommandText = "select id_utulek,nazev,telefon,EMAIL,id_adresa from utulek";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Shelters.Add
                                (
                                new(
                                    v.GetInt32(0),
                                    v.GetString(1),
                                    v.GetString(2),
                                    v.IsDBNull(3) ? null : v.GetString(3),
                                    v.GetInt32(4)
                                ));

                            if ((permissions & (ulong)Permissions.PES_UPDATE) != 0) Shelters.Last().PropertyChanged += ShelterChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void ShelterChanged(object? sender, PropertyChangedEventArgs e)
        {
            Shelter? dog = sender as Shelter;
            using (OracleCommand cmd = con.CreateCommand())
            {

                    await SaveUtulek(dog);
            }
        }

        private async Task SaveUtulek(Shelter utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.ID is null ? new("V_ID_UTULEK", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_UTULEK", OracleDbType.Decimal, utulek.ID, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.Name,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_TELEFON", OracleDbType.Varchar2, utulek.Telephone, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_EMAIL", OracleDbType.Varchar2, utulek.Email, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ID_ADRESA", OracleDbType.Decimal, utulek.AddressID, ParameterDirection.Input));
                    
                    cmd.CommandText = "INS_SET.IU_UTULEK";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.ID = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Shelters.CollectionChanged -= Utulek_CollectionChanged;
                LoadShelters(permissions);
                Shelters.CollectionChanged += Utulek_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Utulek_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Shelter dog in e.NewItems ?? new List<Storage>())
            {
                await SaveUtulek(dog);

            }

            foreach (Shelter dog in e.OldItems ?? new List<Storage>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.ID));
                        cmd.CommandText = "delete from utulek where id_utulek=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Shelters.CollectionChanged -= Utulek_CollectionChanged;
                        LoadDogs(permissions);
                        Shelters.CollectionChanged += Utulek_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    } 
}
