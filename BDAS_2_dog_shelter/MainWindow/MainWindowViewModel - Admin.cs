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
        KeyValueUS SelectedType;

        private RelayCommand tadCMD;
        private RelayCommand<object> trmCMD;
        private RelayCommand<object> tedCMD;
        public ICommand cmdTAdd => tadCMD ??= new RelayCommand        (CommandTypesAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_INSERT)));
        public ICommand cmdTRm => trmCMD ??= new RelayCommand<object> (CommandTypesRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_DELETE)));
        public ICommand cmdTEd => tedCMD ??= new RelayCommand<object> (CommandTypesOK, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ADRESA_UPDATE)));

        private void CommandTypesRemove(object? obj)
        {
            throw new NotImplementedException();
        }


        private void CommandTypesOK(object? obj)
        {
            throw new NotImplementedException();
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
                        cmd.CommandText = "select id_typu,nazev from typ_udalosti";
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
                                Typy.Last().PropertyChanged += AdressChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
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
                        LoadFood(permissions);
                        Typy.CollectionChanged += Typy_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }

        }

        private async Task SaveTypy(KeyValueUS dog)
        {
            throw new NotImplementedException();
        }
    }
    } 

