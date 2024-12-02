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
        private RelayCommand aadCMD;
        private RelayCommand<object> armCMD;
        private RelayCommand<object> aedCMD;
        public ICommand cmdAAdd => aadCMD ??= new RelayCommand(CommandAdressAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_INSERT)));
        public ICommand cmdA0Rm => armCMD ??= new RelayCommand<object>(CommandAdressRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_DELETE)));
        public ICommand cmdAEd => aedCMD ??= new RelayCommand<object>(CommandAdressEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_UPDATE)));
        public ObservableCollection<Adress> Adresses { get; set; } = new();

        private void CommandAdressEdit(object? obj)
        {
            ShelterAdd s = new ShelterAdd((Shelter)obj);
            s.ShowDialog();
        }

        private void CommandAdressRemove(object? SelectedShelters)
        {
            if ((Permission.HasAnyOf(permissions, Permissions.ADMIN,Permissions.ADRESA_DELETE)))
            {
                List<Shelter> e = new List<Shelter>();
                foreach (Shelter d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Shelter shelter in e)
                {
                    Shelters.Remove(shelter);
                }
            }
        }
        


        private void CommandAdressAdd()
        {
            ShelterAdd s = new ShelterAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Adresses.Add(((AddAdressViewModel)s.DataContext).Adresa);
                if ((Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_INSERT)))
                    Shelters.Last().PropertyChanged += DogChanged;
            }
        }

        private void LoadAdresses(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_adresa,psc,telefon,number,id_adresa from adresa";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Adresses.Add
                                (
                                new(
                                    v.GetInt32(0),
                                    v.GetString(1),
                                    v.GetString(2),
                                    v.IsDBNull(3) ? null : v.GetString(3),
                                    v.GetInt32(4)
                                ));

                            if ((Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_UPDATE)))
                                Shelters.Last().PropertyChanged += AdressChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void AdressChanged(object? sender, PropertyChangedEventArgs e)
        {
            Adress? dog = sender as Adress;
            using (OracleCommand cmd = con.CreateCommand())
            {

                    await SaveAdress(dog);
            }
        }

        private async Task SaveAdress(Adress utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_ADRESA", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_UTULEK", OracleDbType.Varchar2, utulek.ID, System.Data.ParameterDirection.InputOutput));
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
                Dogs.CollectionChanged -= Dogs_CollectionChanged;
                LoadDogs(permissions);
                Dogs.CollectionChanged += Dogs_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Adress_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Adress dog in e.NewItems ?? new List<Shelter>())
            {
                await SaveAdress(dog);

            }

            foreach (Adress dog in e.OldItems ?? new List<Adress>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from adresa where id_adresa=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Dogs.CollectionChanged -= Dogs_CollectionChanged;
                        LoadAdresses(permissions);
                        Dogs.CollectionChanged += Dogs_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    } 
}
