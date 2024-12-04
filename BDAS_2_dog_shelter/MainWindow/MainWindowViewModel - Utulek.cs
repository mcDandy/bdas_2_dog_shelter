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
        private RelayCommand uadCMD;
        private RelayCommand<object> urmCMD;
        private RelayCommand<object> uedCMD;
        public ICommand cmdUAdd => uadCMD ??= new RelayCommand(CommandUtulekAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_INSERT)));
        public ICommand cmdURm => urmCMD ??= new RelayCommand<object>(CommandUtulekRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_DELETE)));
        public ICommand cmdUEd => uedCMD ??= new RelayCommand<object>(CommandUtulekEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_UPDATE)));
        public ObservableCollection<Shelter> Shelters { get; set; } = new();

        private void CommandUtulekEdit(object? obj)
        {
            ShelterAdd s = new(((IEnumerable)obj).Cast<Shelter>().First());
            s.ShowDialog();
        }

        private void CommandUtulekRemove(object? SelectedShelters)
        {
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_DELETE))
            {
                List<Shelter> e = new List<Shelter>();
                foreach (Shelter d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Shelter shelter in e)
                {
                    Shelters.Remove(shelter);
                }
            }
        }
   


        private void CommandUtulekAdd()
        {
            ShelterAdd s = new ShelterAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Shelters.Add(((AddShelterViewModel)s.DataContext).Utulek);
                if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.SKLAD_UPDATE)) Shelters.Last().PropertyChanged += ShelterChanged;
            }
        }

        private void LoadShelters(ulong permissions)
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
            foreach (Shelter dog in e.NewItems ?? new List<Shelter>())
            {
                await SaveUtulek(dog);

            }

            foreach (Shelter dog in e.OldItems ?? new List<Shelter>())
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
                        LoadShelters(permissions);
                        Shelters.CollectionChanged += Utulek_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    } 
}
