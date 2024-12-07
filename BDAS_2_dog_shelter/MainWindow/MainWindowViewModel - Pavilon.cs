using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Pavilon;
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
        private RelayCommand PavilonadhCMD;
        private RelayCommand<object> PavilonrmhCMD;
        private RelayCommand<object> PavilonedhCMD;
        public ICommand cmdPavilonAdd => PavilonadhCMD ??= new RelayCommand(CommandPavilonAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_INSERT)));
        public ICommand cmdPavilonRm => PavilonrmhCMD ??= new RelayCommand<object>(CommandPavilonRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_DELETE)));
        public ICommand cmdPavilonEd => PavilonedhCMD ??= new RelayCommand<object>(CommandPavilonEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_UPDATE)));
        public ObservableCollection<Pavilion> Pavilony { get; set; } = new();

        private void CommandPavilonEdit(object? obj)
        {
            PavilonAdd s = new(((IEnumerable)obj).Cast<Pavilion>().First());
            s.ShowDialog();
        }

        private void CommandPavilonRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.PAVILON_DELETE) > 0)
            {
                List<Pavilion> e = new List<Pavilion>();
                foreach (Pavilion d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Pavilion shelter in e)
                {
                    Pavilony.Remove(shelter);
                }
            }
        }

        private void CommandPavilonAdd()
        {
            PavilonAdd s = new PavilonAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Pavilony.Add(((AddPavilonViewModel)s.DataContext).pavilon);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_UPDATE)) Pavilony.Last().PropertyChanged += PavilonChanged;
            }
        }

        private void LoadPavilon(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PAVILON_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_Pavilon,nazev,kapacita from PAVILON";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Pavilony.Add(new(v.GetInt32(0), v.GetString(1), v.GetInt32(2)));
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

        private async void PavilonChanged(object? sender, PropertyChangedEventArgs e)
        {
            Pavilion? dog = sender as Pavilion;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SavePavilon(dog);
            }
        }

        private async Task SavePavilon(Pavilion utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_PAVILON", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_PAVILON", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.PavName, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_KAPACITA", OracleDbType.Decimal, utulek.CapacityPav, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_PAVILON";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Pavilony.CollectionChanged -= Pavilon_CollectionChanged;
                Pavilony.Clear();
                LoadPavilon(permissions);
                Pavilony.CollectionChanged += Pavilon_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Pavilon_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Pavilion dog in e.NewItems ?? new List<Pavilion>())
            {
                await SavePavilon(dog);

            }

            foreach (Pavilion dog in e.OldItems ?? new List<Pavilion>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from Pavilon where id_Pavilon=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Pavilony.CollectionChanged -= Pavilon_CollectionChanged;
                        Pavilony.Clear();
                        LoadPavilon(permissions);
                        Pavilony.CollectionChanged += Pavilon_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}