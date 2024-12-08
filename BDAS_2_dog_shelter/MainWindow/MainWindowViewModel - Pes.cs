using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        public ObservableCollection<Dog> Dogs { get; set; } = new();


        private void LoadDogs(ulong permissions)
        {
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "select id_pes,jmeno,vek, barva_srsti,datum_prijeti,duvod_prijeti,stav_pes,utulek_id_utulek,karantena_id_karantena,majitel_id_majitel,id_otec,id_matka,dog_image,image_id from dog_image";
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
                        Dogs.Add
                            (
                            new(v.IsDBNull(0) ? null : v.GetInt32(0),
                            v.GetString(1),
                            v.GetInt32(2),
                            v.GetString(3),
                            v.GetDateTime(4),
                            v.GetString(5),
                            v.GetString(6),
                            v.IsDBNull(7) ? null : v.GetInt32(7),
                            v.IsDBNull(8) ? null : v.GetInt32(8),
                            v.IsDBNull(9) ? null : v.GetInt32(9),
                            v.IsDBNull(10) ? null : v.GetInt32(10),
                            v.IsDBNull(11) ? null : v.GetInt32(11),
                            decoder?.Frames[0],
                            v.IsDBNull(13) ? null : v.GetInt32(13))
                            );

                        if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.PES_UPDATE)) Dogs.Last().PropertyChanged += DogChanged;
                    }
                    List<Dog> DogForest = Dogs.Select<Dog,Dog> 
                        (a => { 
                            a.Matka = Dogs.Where(d => d.ID == a.MatkaId).FirstOrDefault(); 
                            a.Otec = Dogs.Where(d => d.ID == a.MatkaId).FirstOrDefault();
                            a.Utulek = Shelters.Where(d => d.ID == a.UtulekId).FirstOrDefault();
                            return a; 
                        }).ToList();
                    Dogs.CollectionChanged -= Dogs_CollectionChanged;
                    Dogs.Clear();
                    foreach (var item in DogForest)
                    {
                        Dogs.Add(item);
                    }
                    Dogs.CollectionChanged += Dogs_CollectionChanged;
                }
                catch (Exception ex)//something went wrong
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            }
        }

        private async void Dogs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            foreach (Dog dog in e.NewItems ?? new List<Dog>())
            {
                await SaveDog(dog,true);

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
                        Dogs.CollectionChanged -= Dogs_CollectionChanged;
                        Dogs.Clear();
                        LoadDogs(permissions);
                        Dogs.CollectionChanged += Dogs_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
            
        }

        private async Task SaveDog(Dog dog, bool saveImage)
        {
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            try
            {
                if(saveImage)
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Assign id to the department number 50 

                    cmd.Parameters.Add(dog.Obrazek_Id is null ? new("V_ID_IMAGE", OracleDbType.Decimal, DBNull.Value, ParameterDirection.InputOutput) : new("V_ID_IMAGE", OracleDbType.Decimal, dog.Obrazek_Id, System.Data.ParameterDirection.InputOutput));
                    // cmd.Parameters.Add(new("imgid", dog.Obrazek.));
                    PngBitmapEncoder pe = new PngBitmapEncoder();
                    pe.Frames.Add(BitmapFrame.Create(dog.Obrazek));
                    MemoryStream ms = new MemoryStream();
                    pe.Save(ms);
                    byte[] b = ms.ToArray();
                    cmd.Parameters.Add("V_IMAGE",OracleDbType.Blob,b, ParameterDirection.Input);
                    // cmd.Parameters.Add(new("path"), dog.Obrazek.);
                    cmd.Parameters.Add(dog.Obrazek is null ? new("V_FILENAME", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Input) : new("V_FILENAME", OracleDbType.Varchar2, Path.GetFileName(dog.FileName), ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_DOG_IMAGES";
                    int j = await cmd.ExecuteNonQueryAsync();

                    dog.Obrazek_Id = (int)((OracleDecimal)cmd.Parameters[0].Value).ToInt64();
                }
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(dog.ID is null ? new("V_ID_PES", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_PES", OracleDbType.Varchar2, dog.ID, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_JMENO",OracleDbType.Varchar2, dog.Name,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_VEK",OracleDbType.Decimal, dog.Age,ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_BARVA_SRSTI",OracleDbType.Varchar2, dog.BodyColor, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_DATUM_PRIJETI",OracleDbType.Date, dog.DatumPrijeti, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_DUVOD_PRIJETI", OracleDbType.Varchar2, dog.DuvodPrijeti, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_STAV_PES", OracleDbType.Varchar2, dog.StavPes, ParameterDirection.Input));
                    cmd.Parameters.Add(dog.UtulekId is null ? new("V_UTULEK_ID_UTULEK",OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input) : new("V_UTULEK_ID_UTULEK", OracleDbType.Varchar2, dog.UtulekId, ParameterDirection.Input));
                    cmd.Parameters.Add(dog.KarantenaId is null ? new("V_KARANTENA_ID_KARANTENA", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input) : new("V_KARANTENA_ID_KARANTENA", OracleDbType.Decimal, dog.KarantenaId, ParameterDirection.Input));
                    cmd.Parameters.Add(dog.MajtelId is null ? new("V_MAJITEL_ID_MAJITEL",OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input) : new("V_MAJITEL_ID_MAJITEL", OracleDbType.Decimal, dog.MajtelId, ParameterDirection.Input));
                    cmd.Parameters.Add(dog.Obrazek_Id is null ? new("V_IMAGE_ID", DBNull.Value) : new("V_IMAGE_ID", dog.Obrazek_Id));
                    cmd.Parameters.Add(dog.OtecId is null ? new("V_ID_OTEC", OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input) : new("V_ID_OTEC", OracleDbType.Decimal, dog.OtecId, ParameterDirection.Input));
                    cmd.Parameters.Add(dog.MatkaId is null ? new("V_ID_MATKA",OracleDbType.Decimal, DBNull.Value, ParameterDirection.Input) : new("V_ID_MATKA",OracleDbType.Decimal, dog.MatkaId, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_PES";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    dog.ID = Convert.ToInt32(cmd.Parameters[0].Value.ToString());
                    dog.Otec = Dogs.Where(g => g.ID == dog.OtecId).FirstOrDefault();
                    dog.Matka = Dogs.Where(g => g.ID == dog.MatkaId).FirstOrDefault();
                    dog.Utulek = Shelters.Where(g => g.ID == dog.UtulekId).FirstOrDefault(); //TODO: MAJTEL, KARANTENA
                }
            }
            catch (Exception ex)//something went wrong
            {
                MessageBox.Show("Data nejsou správná. Klikněte na Cancel & refresh pro obnovení");
                return;
            }
        }


private RelayCommand addCMD;
private RelayCommand<object> rmCMD;
private RelayCommand<object> trCMD;
private RelayCommand<object> edCMD;
public ICommand cmdAdd => addCMD ??= new RelayCommand(CommandAdd,() => (Permission.HasAnyOf(permissions,Permissions.ADMIN, Permissions.PES_INSERT)));
public ICommand cmdRm => rmCMD ??= new RelayCommand<object>(CommandRemove,(p)=>(p is not null && Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.PES_DELETE)));
public ICommand cmdEd => edCMD ??= new RelayCommand<object>(CommandEdit, (p) => (p is not null && Permission.HasAnyOf(permissions,Permissions.ADMIN, Permissions.PES_UPDATE)));
public ICommand cmdTree => trCMD ??= new RelayCommand<object>(CommandShowTree);

private void CommandShowTree(object? obj)
{
    if (obj is Dog selectedDog)
    {
        var dogTreeWindow = new DogTree.DogTree(selectedDog);
        dogTreeWindow.ShowDialog();
    }
}

private void CommandAdd()
{
    if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.PES_INSERT))
    {
        DogAdd da = new(new Dog(),Dogs.ToList(),Owners.ToList(),Karanteny.ToList());
        if (da.ShowDialog() == true)
        {
                    //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                    Dog d = ((AddDogViewModel)da.DataContext).Dog;

                    Dogs.Add(((AddDogViewModel)da.DataContext).Dog);
            if ((permissions & (ulong)Permissions.PES_UPDATE) != 0) Dogs.Last().PropertyChanged += DogChanged;
        }
    }
}

private void CommandEdit(Object o)
{
    if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PES_UPDATE))
    {
        DogAdd da = new(((IEnumerable)o).Cast<Dog>().First(),Dogs.ToList(),Owners.ToList(),Karanteny.ToList());
        if (da.ShowDialog() == true)
        {
                    Dog d = ((AddDogViewModel)da.DataContext).Dog;
                    d.Otec = Dogs.Where(g => g.ID == d.OtecId).FirstOrDefault();
                    d.Matka = Dogs.Where(g => g.ID == d.MatkaId).FirstOrDefault();
                    d.Utulek = Shelters.Where(g => g.ID == d.UtulekId).FirstOrDefault();

                }
    }
}

private async void DogChanged(object? sender, PropertyChangedEventArgs e)
{
    Dog? dog = sender as Dog;
    using (OracleCommand cmd = con.CreateCommand())
    {
        await SaveDog(dog,e.PropertyName is nameof(dog.Obrazek) or nameof(dog.Obrazek_Id));
    }
}

private void CommandRemove(object SelectedDogs)
{
    if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.PES_DELETE))
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
