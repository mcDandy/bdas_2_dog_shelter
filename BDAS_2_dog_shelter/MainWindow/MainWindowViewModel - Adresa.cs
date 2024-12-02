using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Adress;
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
            AdressAdd s = new AdressAdd((Adress)obj);
            s.ShowDialog();
        }

        private void CommandAdressRemove(object? SelectedShelters)
        {
            if ((Permission.HasAnyOf(permissions, Permissions.ADMIN,Permissions.ADRESA_DELETE)))
            {
                List<Adress> e = new List<Adress>();
                foreach (Adress d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Adress shelter in e)
                {
                    Adresses.Remove(shelter);
                }
            }
        }
        


        private void CommandAdressAdd()
        {
            AdressAdd s = new AdressAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Adresses.Add(((AddAdressViewModel)s.DataContext).Adresa);
                if ((Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_INSERT)))
                    Adresses.Last().PropertyChanged += AdressChanged;
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
                        cmd.CommandText = "select id_adresa,ulice,mesto,psc,cislopopisne from adresa";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Adresses.Add
                                (
                                new(
                                    v.IsDBNull(0) ? null : v.GetInt32(0),
                                    v.GetString(1),
                                    v.GetString(2),
                                    v.IsDBNull(3)?null:v.GetString(3),
                                    int.Parse(v.GetString(4))
                                ));

                            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_UPDATE))
                                Adresses.Last().PropertyChanged += AdressChanged;
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
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_ADRESA", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_ADRESA", OracleDbType.Varchar2, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_ULICE", OracleDbType.Varchar2, utulek.Street,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_MESTO", OracleDbType.Varchar2, utulek.City, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_PSC", OracleDbType.Varchar2, utulek.Psc, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_CISLOPOPISNE", OracleDbType.Decimal, utulek.Number, ParameterDirection.Input));
                    
                    cmd.CommandText = "INS_SET.IU_ADRESS";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Adresses.CollectionChanged -= Adress_CollectionChanged;
                LoadAdresses(permissions);
                Adresses.CollectionChanged += Adress_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Adress_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Adress dog in e.NewItems ?? new List<Adress>())
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
                        Adresses.CollectionChanged -= Adress_CollectionChanged;
                        LoadAdresses(permissions);
                        Adresses.CollectionChanged += Adress_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    } 
}
