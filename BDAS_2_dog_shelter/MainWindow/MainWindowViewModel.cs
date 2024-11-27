using BDAS_2_dog_shelter;
using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    cmd.CommandText = "select id_pes,jmeno,vek, barva_srsti,datum_prijeti,duvod_prijeti,stav_pes,utulek_id_utulek,karantena_id_karantena,majitel_id_majitel,id_otec,id_matka,imAage,image_id from dog_image";
                    OracleDataReader v = cmd.ExecuteReader();

                    while (v.Read())
                    {
                        BitmapDecoder decoder = null;
                        if (!v.IsDBNull(12))
                        {
                            byte[] data = v.GetOracleBlob(12)?.Value;
                            MemoryStream ms = new MemoryStream(data);
                            decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        }
                        Dogs.Add(new(v.IsDBNull(0) ? null : v.GetInt32(0), v.GetString(1), v.GetInt32(2), v.GetString(3), v.GetDateTime(4), v.GetString(5), v.GetString(6), v.IsDBNull(7) ? null : v.GetInt32(7), v.IsDBNull(8) ? null : v.GetInt32(8), v.IsDBNull(9) ? null : v.GetInt32(9), v.IsDBNull(10) ? null : v.GetInt32(10), v.IsDBNull(11) ? null : v.GetInt32(11), decoder?.Frames[0], v.IsDBNull(13) ? null : v.GetInt32(13)));

                        if ((permissions & (ulong)Permissions.DOGS_UPDATE) != 0) Dogs.Last().PropertyChanged += DogChanged;
                    }

                }
                catch (Exception ex)//something went wrong
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                if ((permissions & (long)Permissions.DOGS_INSERT) != 0) //TODO: nějaká lepší prevence úpravy
                    Dogs.CollectionChanged += Dogs_CollectionChanged;

            }
        }
        private async void Dogs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            foreach (Dog dog in e.NewItems ?? new List<Dog>())
            {
                await SaveDog(dog);


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
                        cmd.Parameters.Add(new ("ID", dog.ID));
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

        private async Task SaveDog(Dog dog)
        {
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Assign id to the department number 50 

                    cmd.Parameters.Add(dog.Obrazek_Id is null ? new("V_ID_IMAGE", OracleDbType.Int32, DBNull.Value, ParameterDirection.InputOutput) : new("V_ID_IMAGE", OracleDbType.Int32, dog.Obrazek_Id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(dog.Obrazek is null ? new("V_FILENAME",OracleDbType.Varchar2, "DBNull.Value",ParameterDirection.Input) : new("V_FILENAME", OracleDbType.Varchar2, "fill", ParameterDirection.Input));
                    // cmd.Parameters.Add(new("imgid", dog.Obrazek.));
                    PngBitmapEncoder pe = new PngBitmapEncoder();
                    pe.Frames.Add(BitmapFrame.Create(dog.Obrazek));
                    MemoryStream ms = new MemoryStream();
                    pe.Save(ms);
                    byte[] b = ms.ToArray();
                    cmd.Parameters.Add("V_IMAAGE",OracleDbType.Blob,b, ParameterDirection.Input);
                    // cmd.Parameters.Add(new("path"), dog.Obrazek.);
                    cmd.CommandText = "INS_SET.IU_DOG_IMAGE (:V_ID_IMAGE,:V_IMAAGE,:V_FILENAME)";
                    int j = await cmd.ExecuteNonQueryAsync();

                    dog.Obrazek_Id = (int)cmd.Parameters[0].Value;
                }
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(dog.ID is null ? new("did", OracleDbType.Int32, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("did", OracleDbType.Varchar2, dog.ID, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("age", dog.Age));
                    cmd.Parameters.Add(new("color", dog.BodyColor));
                    cmd.Parameters.Add(new("jmeno", dog.Name));
                    cmd.Parameters.Add(new("prijeti", dog.DatumPrijeti));
                    cmd.Parameters.Add(new("duvod", dog.DuvodPrijeti));
                    cmd.Parameters.Add(new("stav", dog.StavPes));
                    cmd.Parameters.Add(dog.UtulekId is null ? new("utulek", DBNull.Value) : new("utulek", dog.UtulekId));
                    cmd.Parameters.Add(dog.KarantenaId is null ? new("karantena", DBNull.Value) : new("karantena", dog.KarantenaId));
                    cmd.Parameters.Add(dog.MajtelId is null ? new("majtel", DBNull.Value) : new("majtel", dog.MajtelId));
                    cmd.Parameters.Add(dog.OtecId is null ? new("otec", DBNull.Value) : new("otec", dog.OtecId));
                    cmd.Parameters.Add(dog.MatkaId is null ? new("matka", DBNull.Value) : new("matka", dog.MatkaId));
                    cmd.Parameters.Add(dog.Obrazek_Id is null ? new("V_IMAGE_ID", DBNull.Value) : new("V_IMAGE_ID", dog.Obrazek_Id));

                    cmd.CommandText = "INS_SET.IU_PES (:did,:jmeno,:age,:color,:prijeti,:duvod,:stav,:utulek,:karantena,:majtel,:otec,:matka,:imgid)";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    dog.ID = (int)cmd.Parameters[0].Value;

                }
            }
            catch (Exception ex)//something went wrong
            {
                con.Rollback(); MessageBox.Show(ex.Message);
                return;
            }
        }


private RelayCommand addCMD;
public ICommand cmdAdd => addCMD ??= new RelayCommand(buttdonAdd_Click);
private RelayCommand<object> rmCMD;
private RelayCommand<object> trCMD;
private RelayCommand<object> edCMD;
public ICommand cmdRm => rmCMD ??= new RelayCommand<object>(buttonRemove_Click);
public ICommand cmdEd => edCMD ??= new RelayCommand<object>(buttonEdit_Click);
public ICommand cmdTree => trCMD ??= new RelayCommand<object>(MenuCommandDog);
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
            if ((permissions & (ulong)Permissions.DOGS_UPDATE) != 0) Dogs.Last().PropertyChanged += DogChanged;
        }
    }
}

private void buttonEdit_Click(Object o)
{
    if ((permissions & (long)Permissions.DOGS_UPDATE) > 0)
    {
        DogAdd da = new(((IEnumerable)o).Cast<Dog>().First());
        if (da.ShowDialog() == true)
        {
            //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");

        }
    }
}

private async void DogChanged(object? sender, PropertyChangedEventArgs e)
{
    Dog? dog = sender as Dog;
    using (OracleCommand cmd = con.CreateCommand())
    {
        try
        {
            await SaveDog(dog);

        }

        catch (Exception ex)//something went wrong
        {
            con.Rollback(); MessageBox.Show(ex.Message);

            return;
        }
    }
}

private void buttonRemove_Click(object SelectedDogs)
{
    if ((permissions & (long)Permissions.DOGS_DELETE) > 0)
    {
        List<Dog> e = new List<Dog>();
        foreach (Dog d in (IEnumerable)SelectedDogs) e.Add(d);
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
