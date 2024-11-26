using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static BDAS_2_dog_shelter.Secrets;

namespace BDAS_2_dog_shelter.MainWindow
{
    internal class MainWindowViewModel
    {
        private ulong permissions;
        private OracleConnection con;
        public ObservableCollection<Dog> Dogs { get; set; } = new();
        public ObservableCollection<Dog> Shelters { get; set; } = new();

        public MainWindowViewModel(ulong permissions)
        {
            this.permissions = permissions;
            con = new OracleConnection(ConnectionString);
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "select id_pes,jmeno,vek, barva_srsti,datum_prijeti,duvod_prijeti,stav_pes from pes";
                    OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {
                            Dogs.Add(new(v.GetInt32(0), v.GetString(1), v.GetInt32(2), v.GetString(3), v.GetDateTime(4), v.GetString(5), v.GetString(6)));
                        }
                    
                }
                catch (Exception ex)//something went wrong
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                if ((permissions & (long)Permissions.DOGS_UPDATE) != 0) //TODO: nějaká lepší prevence úpravy
                    Dogs.CollectionChanged += Dogs_CollectionChanged;

            }
        }
        private async void Dogs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {


            foreach (Dog dog in e.NewItems ?? new List<Dog>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("did",OracleDbType.Varchar2,dog.ID,System.Data.ParameterDirection.InputOutput));
                        cmd.Parameters.Add(new("age", dog.Age));
                        cmd.Parameters.Add(new("color", dog.BodyColor));
                        cmd.Parameters.Add(new("jmeno", dog.Name));
                        cmd.Parameters.Add(new("prijeti", dog.DatumPrijeti));
                        cmd.Parameters.Add(new("duvod", dog.DuvodPrijeti));
                        cmd.Parameters.Add(new("stav", dog.StavPes));
                        cmd.Parameters.Add(new("utulek", dog.UtulekId??-1));
                        cmd.Parameters.Add(new("karantena", dog.KarantenaId??-1));
                        cmd.Parameters.Add(new("majtel", dog.MajtelId??-1));
                        cmd.Parameters.Add(new("otec", dog.OtecId??-1));
                        cmd.Parameters.Add(new("matka", dog.MatkaId??-1));
                        cmd.CommandText = "INS_SET.IU_PES (:did,:jmeno,:age,:color,:prijeti,:duvod,:stav,:utulek,:karantena,:majtel,:otec,:matka)";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();
                        dog.ID =  (int)cmd.Parameters[0].Value;
                    }

                    catch (Exception ex)//something went wrong
                    {
                        con.Rollback(); MessageBox.Show(ex.Message);

                        return;
                    }
                }
            }
            foreach (Dog dog in e.OldItems ?? new List<Dog>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.ID));
                        cmd.CommandText = "delete from pes where id_pes=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        con.Rollback(); MessageBox.Show(ex.Message);

                        return;
                    }
                }
            }
            con.Commit();
        }
        private RelayCommand addCMD;
        public ICommand cmdAdd => addCMD ??= new RelayCommand(buttdonAdd_Click);
        private RelayCommand<object> rmCMD;
        private RelayCommand<object> tr;
        public ICommand cmdRm => rmCMD ??= new RelayCommand<object>(MenuCommandDog);
        public ICommand cmdTree => trCMD ??= new RelayCommand<object>(buttonRemove_Click);
        private void MenuCommandDog(object? obj)
        {
            throw new NotImplementedException();
        }

        private void buttdonAdd_Click()
        {
            if ((permissions & (long)Permissions.DOGS_INSERT) > 0)
            {
                DogAdd da = new(new Dog());
                if (da.ShowDialog() == true)
                {
                    //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                    Dogs.Add(((AddDogViewModel)da.DataContext).Dog);
                    Dogs.Last().PropertyChanged += DogChanged;
                }
            }
        }

        private void DogChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog? dog = sender as Dog;
            MessageBox.Show(e.PropertyName);
        }

        private void buttonRemove_Click(object SelectedDogs)
        {
            if ((permissions & (long)Permissions.DOGS_DELETE) > 0)
            {
                List<Dog> e = new List<Dog>();
                foreach(Dog d in (IEnumerable)SelectedDogs) e.Add(d);
                foreach (Dog dog in e)
                {
                    Dogs.Remove(dog);
                }
            }
        }

        private void gridOnChangeDogUtulek(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ((Dog)e.AddedItems[0]).UtulekId = ((ComboBox)sender).SelectedIndex;//TODO: e.addedItems je typu který se tam přidával
        }

    }
}
