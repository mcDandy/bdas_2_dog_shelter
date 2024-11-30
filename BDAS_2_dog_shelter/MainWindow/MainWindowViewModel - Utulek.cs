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

        public ObservableCollection<Shelter> Shelters { get; set; } = new();
        private void LoadShelters(ulong permissions)
        {
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
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
                            new(v.GetInt32(0),
                            v.GetString(1),
                            v.GetString(2),
                            v.IsDBNull(3) ? null : v.GetString(3),
                            v.GetInt32(4)
                            )
                            );

                        if ((permissions & (ulong)Permissions.PES_UPDATE) != 0) Dogs.Last().PropertyChanged += DogChanged;
                    }

                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        } 
    } 
}
