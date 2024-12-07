using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Owner;
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
        private RelayCommand OwneradhCMD;
        private RelayCommand<object> OwnerrmhCMD;
        private RelayCommand<object> OwneredhCMD;
        public ICommand cmdOwnerAdd => OwneradhCMD ??= new RelayCommand(CommandOwnerAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.MAJITEL_INSERT)));
        public ICommand cmdOwnerRm => OwnerrmhCMD ??= new RelayCommand<object>(CommandOwnerRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.MAJITEL_DELETE)));
        public ICommand cmdOwnerEd => OwneredhCMD ??= new RelayCommand<object>(CommandOwnerEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.MAJITEL_UPDATE)));
        public ObservableCollection<Owner> Owners { get; set; } = new();

        private void CommandOwnerEdit(object? obj)
        {
            OwnerAdd s = new(((IEnumerable)obj).Cast<Owner>().First());
            s.ShowDialog();
        }

        private void CommandOwnerRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.MAJITEL_DELETE) > 0)
            {
                List<Owner> e = new List<Owner>();
                foreach (Owner d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Owner shelter in e)
                {
                    Owners.Remove(shelter);
                }
            }
        }

        private void CommandOwnerAdd()
        {
            OwnerAdd s = new OwnerAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Owners.Add(((AddOwnerViewModel)s.DataContext).Owner);
                if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.MAJITEL_UPDATE)) Owners.Last().PropertyChanged += OwnerChanged;
            }
        }

        private void LoadOwner(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.MAJITEL_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_majitel,jmeno,prijmeni,adresa,telefon,email from MAJITELE";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Owners.Add(new(v.GetInt32(0), v.GetString(1), v.GetString(2), v.GetInt32(3), v.GetString(4), v.GetString(5)));
                            }
                        }
                        List<Owner> DogForest = Owners.Select<Owner, Owner>
                               (a => {
                                   a.Adresa = Adresses.Where(d => d.id == a.AdresaId).FirstOrDefault();

                                   return a;
                               }).ToList();

                        Owners.Clear();
                        foreach (var item in DogForest)
                        {
                            Owners.Add(item);
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }  
        }

        private async void OwnerChanged(object? sender, PropertyChangedEventArgs e)
        {
            Owner? dog = sender as Owner;
            using (OracleCommand cmd = con.CreateCommand())
            {

                    await SaveOwner(dog);
            }
        }

        private async Task SaveOwner(Owner utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_MAJITEL", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_MAJITEL", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_JMENO", OracleDbType.Varchar2, utulek.Name,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_PRIJMENI", OracleDbType.Varchar2, utulek.Surname, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ADRESA", OracleDbType.Decimal, utulek.AdresaId, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_EMAIL", OracleDbType.Varchar2, utulek.Email, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.Phone, ParameterDirection.Input));
                    
                    
                    cmd.CommandText = "INS_SET.IU_MAJITEL";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Owners.CollectionChanged -= Owner_CollectionChanged;
                LoadOwner(permissions);
                Owners.CollectionChanged += Owner_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Owner_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Owner dog in e.NewItems ?? new List<Owner>())
            {
                await SaveOwner(dog);

            }

            foreach (Owner dog in e.OldItems ?? new List<Owner>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from Owner where id_Owner=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Owners.CollectionChanged -= Owner_CollectionChanged;
                        LoadOwner(permissions);
                        Owners.CollectionChanged += Owner_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
        private async Task SaveFood(Owner utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_MAJITEL", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_MAJITEL", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_JMENO", OracleDbType.Varchar2, utulek.Name,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_PRIJMENI", OracleDbType.Varchar2, utulek.Surname, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ADRESA", OracleDbType.Decimal, utulek.AdresaId, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_EMAIL", OracleDbType.Varchar2, utulek.Email, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.Phone, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_MAJITEL";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Owners.CollectionChanged -= Owner_CollectionChanged;
                LoadOwner(permissions);
                Owners.CollectionChanged += Owner_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
    } 
}
