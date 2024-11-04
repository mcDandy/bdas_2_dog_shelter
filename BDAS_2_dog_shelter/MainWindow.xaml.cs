using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Dog> Dogs { get; set; } = new();
        private long permissions = 0;
        OracleConnection con;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            con = new OracleConnection(ConnectionString);
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "select id_pes,jmeno,vek, barva_srsti,datum_prijeti,duvod_prijeti,stav_pes from psi";
                    OracleDataReader v = cmd.ExecuteReader();
                    if (v.HasRows)
                    {
                        for (int i = 0; i < cmd.ImplicitRefCursors.Length; i++)
                        {
                            Dogs.Add(new(v.GetInt32(0), v.GetString(1), v.GetInt32(2), v.GetString(3), v.GetDateTime(4), v.GetString(5), v.GetString(6)));
                        }
                    }
                }
                catch (Exception ex)//something went wrong
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                if ((permissions & (long)Permissions.DOGS_UPDATE) == 0) //TODO: nějaká lepší prevence úpravy
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
                        if(con.State==System.Data.ConnectionState.Closed)con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("age", dog.Age));
                        cmd.Parameters.Add(new("color", dog.BodyColor));
                        cmd.Parameters.Add(new("jmeno", dog.Name));
                        cmd.Parameters.Add(new("prijeti", dog.DatumPrijeti));
                        cmd.Parameters.Add(new("duvod", dog.DatumPrijeti));
                        cmd.Parameters.Add(new("duvod", dog.StavPes));
                        cmd.Parameters.Add(new("stav", dog.StavPes));
                        cmd.CommandText = "INSERT INTO psi (jmeno,vek,barva_srsti,datum_prijeti,duvod_prijeti,stav_pes,utulek_id_utulek,zdravotni_zaznam_id_zaznam,karantena_id_karantena,historie_psa_id_historie,majitel_id_majitel) values (:jmeno,:age,:color,:prijeti,:duvod,:stav,0,0,0,0,0)";
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




        public MainWindow(long permissions) : this() {
            this.permissions = permissions;
        }
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if ((permissions & (long)Permissions.DOGS_INSERT) > 0) {
                DogAdd da = new();
                if (da.ShowDialog()==true
                    && da.Dog.Name is not null 
                    && da.Dog.BodyColor is not null 
                    && da.Dog.DuvodPrijeti is not null 
                    && da.Dog.StavPes is not null
                    && da.Dog.Obrazek is not null)
                {
                    //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                    Dogs.Add(da.Dog);
                    da.Dog.PropertyChanged += DogChanged;
                }
                }
        }

        private void DogChanged(object? sender, PropertyChangedEventArgs e)
        {
            Dog? dog = sender as Dog;
            MessageBox.Show(e.PropertyName);
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if ((permissions & (long)Permissions.DOGS_DELETE) > 0)
            {
                List<Dog> selectedDogs = new List<Dog>();

                foreach (Dog dog in dogDataGrid.SelectedItems)
                {
                    selectedDogs.Add(dog);
                }

                foreach (Dog dog in selectedDogs)
                {
                    Dogs.Remove(dog);
                } 
            }
        }

        private void gridOnChangeDogUtulek(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ((Dog)sender).UtulekId = 0; e.AddedItems[0].ToString();//TODO: e.addedItems je typu který se tam přidával
        }
    }
}
