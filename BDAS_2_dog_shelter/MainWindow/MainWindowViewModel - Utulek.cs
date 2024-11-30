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
    internal partial class MainWindowViewModel
    {
        private RelayCommand uadCMD;
        private RelayCommand<object> urmCMD;
        private RelayCommand<object> uedCMD;
        public ICommand cmdUAdd => uadCMD ??= new RelayCommand(CommandUtulekAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_INSERT)));
        public ICommand cmdURm => urmCMD ??= new RelayCommand<object>(CommandUtulekRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_DELETE)));
        public ICommand cmdUEd => uedCMD ??= new RelayCommand<object>(CommandUtulekEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.UTULEK_UPDATE)));

        private void CommandUtulekEdit(object? obj)
        {
            throw new NotImplementedException();
        }

        private void CommandUtulekRemove(object? obj)
        {
            throw new NotImplementedException();
        }


        private void CommandUtulekAdd()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Shelter> Shelters { get; set; } = new();
        private void LoadShelters(ulong permissions)
        {
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.SKLAD_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_utulek,nazev,telefon,email,id_adresa from utulek";
                        OracleDataReader v = cmd.ExecuteReader();

                        while (v.Read())
                        {

                            Shelters.Add
                                (
                                new(
                                    v.GetInt32(0),
                                    v.GetString(1),
                                    v.GetString(2),
                                    v.IsDBNull(3) ? null : v.GetString(3),
                                    v.GetInt32(4)
                                ));

                            if ((permissions & (ulong)Permissions.PES_UPDATE) != 0) Shelters.Last().PropertyChanged += ShelterChanged;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void ShelterChanged(object? sender, PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    } 
}
