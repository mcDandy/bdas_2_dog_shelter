using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BDAS_2_dog_shelter.DogTree
{
    public class DogTreeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Dog _mainDog;
        public Dog MainDog
        {
            get => _mainDog;
            set
            {
                if (_mainDog != value)
                {
                    _mainDog = value;
                    OnPropertyChanged(nameof(MainDog));
                }
            }
        }

        private OracleConnection _connection;

        public DogTreeViewModel(Dog rootDog, OracleConnection connection)
        {
            _mainDog = rootDog;
            _connection = connection;
            LoadDogTree(rootDog);
        }

        private void LoadDogTree(Dog rootDog)
        {
            // Dictionary pro vyhledávání psů podle ID
            var dogDictionary = new Dictionary<int?, Dog>();
            dogDictionary[rootDog.ID] = rootDog;

            string cmd = "SELECT id_otec, id_matka, id_pes,  LEVEL FROM pes START WITH ID_PES = :rootDogId CONNECT BY PRIOR ID_OTEC = ID_PES OR PRIOR ID_MATKA = ID_PES";

            using var command = new OracleCommand(cmd, _connection);
            command.Parameters.Add(new OracleParameter(":rootDogId", rootDog.ID));

            using var reader = command.ExecuteReader();
            List<Dog> d = new List<Dog>();
            while (reader.Read())
            {
                d.Add(new());
                d.Last().ID = reader.GetInt32(2);
                d.Last().MatkaId = reader.IsDBNull(1)?null:reader.GetInt32(1);
                d.Last().OtecId = reader.IsDBNull(0) ? null : reader.GetInt32(0);
            }
            List<Dog> DogForest = d.Select
    (a => {
        a.Matka = d.Where(d => d.ID == a.MatkaId).FirstOrDefault();
        a.Otec = d.Where(d => d.ID == a.OtecId).FirstOrDefault();
        return a;
    }).ToList();
            // Nastavení hierarchie kořenového psa
            MainDog = rootDog;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
