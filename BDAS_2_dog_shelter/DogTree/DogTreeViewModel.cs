using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace BDAS_2_dog_shelter.DogTree
{
    internal class DogTreeViewModel
    {
        private Dog dog;
        public DogTreeViewModel(Dog d) { dog = d; }
        private void LoadDogTree()
        {
        //    if (con.State == ConnectionState.Closed) con.Open();

        //    try
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //        SELECT id_pes, id_otec, id_matka 
        //        FROM pes 
        //        CONNECT BY PRIOR id_pes = id_otec OR id_pes = id_matka 
        //        START WITH id_pes = :DogId";
        //            cmd.Parameters.Add(new OracleParameter("DogId", dogId));

        //            using (OracleDataReader reader = cmd.ExecuteReader())
        //            {
        //                Dictionary<int, Dog> dogDictionary = new();

        //                while (reader.Read())
        //                {
        //                    int id = reader.GetInt32(0);
        //                    int? fatherId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
        //                    int? motherId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);

        //                    if (!dogDictionary.ContainsKey(id))
        //                    {
        //                        dogDictionary[id] = new Dog { Id = id };
        //                    }

        //                    if (fatherId.HasValue && !dogDictionary.ContainsKey(fatherId.Value))
        //                    {
        //                        dogDictionary[fatherId.Value] = new Dog { Id = fatherId.Value };
        //                    }

        //                    if (motherId.HasValue && !dogDictionary.ContainsKey(motherId.Value))
        //                    {
        //                        dogDictionary[motherId.Value] = new Dog { Id = motherId.Value };
        //                    }

        //                    dogDictionary[id].Father = fatherId.HasValue ? dogDictionary[fatherId.Value] : null;
        //                    dogDictionary[id].Mother = motherId.HasValue ? dogDictionary[motherId.Value] : null;
        //                }

        //                if (dogDictionary.ContainsKey(dogId))
        //                {
        //                    MainDog = dogDictionary[dogId];
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Chyba při načítání rodokmenu: {ex.Message}");
        //    }
        //
        }

    }
}
