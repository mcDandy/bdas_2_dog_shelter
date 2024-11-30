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

        public ObservableCollection<Dog> Shelters { get; set; } = new();
        private void LoadShelters(ulong permissions)
        {
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    cmd.CommandText = "select id_utulek,jmeno,vek, barva_srsti,datum_prijeti,duvod_prijeti,stav_pes,utulek_id_utulek,karantena_id_karantena,majitel_id_majitel,id_otec,id_matka,imAage,image_id from dog_image";
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

                        if ((permissions & (ulong)Permissions.PES_UPDATE) != 0) Dogs.Last().PropertyChanged += DogChanged;
                    }

                }
    }
}
