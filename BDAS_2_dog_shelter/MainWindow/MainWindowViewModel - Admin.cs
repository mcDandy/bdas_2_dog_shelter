using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Input;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        public KeyValueUS SelectedType { get; set; }

        private RelayCommand tadCMD;
        private RelayCommand<object> trmCMD;
        private RelayCommand<object> tedCMD;
        private RelayCommand scc;
        public ICommand cmdTAdd => tadCMD ??= new RelayCommand        (CommandTypesAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_INSERT)));
        public ICommand cmdTRm => trmCMD ??= new RelayCommand<object> (CommandTypesRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_DELETE)));
        public ICommand cmdTEd => tedCMD ??= new RelayCommand<object> (CommandTypesOK, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_UPDATE)));
        public ICommand sc => scc ??= new RelayCommand (ShowSystem);

        private void ShowSystem()
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "select OBJECT_NAME,OBJECT_TYPE from user_objects";
                    OracleDataReader v = cmd.ExecuteReader();
                    if (v.HasRows)
                    {
                        StringBuilder builder = new StringBuilder();
                        while (v.Read())
                        {
                            builder.AppendLine($"{v.GetString(0)} {v.GetString(1)}");
                        }
                        MessageBox.Show( builder.ToString() );
                    }
                    List<Hracka> DogForest = Hracky.Select<Hracka, Hracka>
                           (a =>
                           {
                               a.Sklad = Storages.Where(d => d.id == a.SkladID).FirstOrDefault();

                               return a;
                           }).ToList();

                    Hracky.Clear();
                    foreach (var item in DogForest)
                    {
                        Hracky.Add(item);
                    }
                }
                catch (Exception ex)//something went wrong
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CommandTypesRemove(object? obj)
        {
            
            {
                List<KeyValueUS> e = new List<KeyValueUS>();
                foreach (KeyValueUS d in (IEnumerable<KeyValueUS>)obj) e.Add(d);
                foreach (KeyValueUS history in e)
                {
                    Typy.Remove(history);
                }
            }
        }


        private void CommandTypesOK(object? obj)
        {
            //throw new NotImplementedException();
        }

        private void CommandTypesAdd()
        {
            Typy.Add(new(null, "<prázdné>"));
        }


        public ObservableCollection<KeyValueUS> Typy { get; set; } = [];
        private void LoadTypes(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_typu,nazev from w_typ_udalosti";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Typy.Add
                                (
                                new(
                                    v.IsDBNull(0) ? null : v.GetInt32(0),
                                    v.GetString(1)
                                ));

                            if (Permission.HasAnyOf(permissions, Permissions.ADMIN))
                                Typy.Last().PropertyChanged += TypyChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void TypyChanged(object? sender, PropertyChangedEventArgs e)
        {
            KeyValueUS? history = sender as KeyValueUS;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveTypy(history);
            }
        }

        private async void Typy_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (KeyValueUS dog in e.NewItems ?? new List<KeyValueUS>())
            {
                await SaveTypy(dog);

            }

            foreach (KeyValueUS dog in e.OldItems ?? new List<KeyValueUS>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.Id));
                        cmd.CommandText = "delete from typ_udalosti where id_typu=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();
                    }

                    catch (Exception ex)//something went wrong
                    {
                        Typy.CollectionChanged -= Typy_CollectionChanged;
                        Typy.Clear();
                        LoadTypes(permissions);
                        Typy.CollectionChanged += Typy_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }

        }

        private async Task SaveTypy(KeyValueUS dog)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "INS_SET.IU_TYP_UDALOSTI"; // Replace this with your actual stored procedure
                    cmd.Parameters.Add("V_ID_TYPU", OracleDbType.Decimal, dog.Id ?? (object)DBNull.Value, ParameterDirection.InputOutput);
                    cmd.Parameters.Add("V_ID_NAZEV", OracleDbType.Varchar2, dog.Nazev ?? (object)DBNull.Value, ParameterDirection.Input);

                    await cmd.ExecuteNonQueryAsync();
                    dog.Id = Convert.ToInt32(cmd.Parameters["V_ID_TYPU"].Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Typy.CollectionChanged -= Typy_CollectionChanged;
                Typy.Clear();
                LoadTypes(permissions);
                Typy.CollectionChanged += Typy_CollectionChanged;

            }
        }
    }
    } 

