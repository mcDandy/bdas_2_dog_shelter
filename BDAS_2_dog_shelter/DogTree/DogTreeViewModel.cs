using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.ComponentModel;

public class DogTreeViewModel : INotifyPropertyChanged
{
    private Dog _dog;
    private OracleConnection _connection;

    public DogTreeViewModel(Dog dog, OracleConnection connection)
    {
        Dog = dog;
        _connection = connection;
        LoadTreeLevels();
    }

    public Dog Dog
    {
        get => _dog;
        set
        {
            _dog = value;
            OnPropertyChanged(nameof(Dog));
            OnPropertyChanged(nameof(DogName));
        }
    }

    // Jméno psa
    public string DogName => Dog.Name;

    // Rodiče
    public DogTreeViewModel? Father { get; private set; }
    public DogTreeViewModel? Mother { get; private set; }

    // Prarodiče (2. generace)
    public DogTreeViewModel? FatherFather => Father?.Father;
    public DogTreeViewModel? FatherMother => Father?.Mother;
    public DogTreeViewModel? MotherFather => Mother?.Father;
    public DogTreeViewModel? MotherMother => Mother?.Mother;

    // Prarodiče (3. generace)
    public DogTreeViewModel? FatherFatherFather => FatherFather?.Father;
    public DogTreeViewModel? FatherFatherMother => FatherFather?.Mother;
    public DogTreeViewModel? FatherMotherFather => FatherMother?.Father;
    public DogTreeViewModel? FatherMotherMother => FatherMother?.Mother;
    public DogTreeViewModel? MotherFatherFather => MotherFather?.Father;
    public DogTreeViewModel? MotherFatherMother => MotherFather?.Mother;
    public DogTreeViewModel? MotherMotherFather => MotherMother?.Father;
    public DogTreeViewModel? MotherMotherMother => MotherMother?.Mother;

    private void LoadTreeLevels()
    {
        // Načtení rodičů
        List<Dog> parents = LoadDogs((int)Dog.ID);

        if (parents.Count > 0)
        {
            Father = new DogTreeViewModel(parents[0], _connection);
            Mother = new DogTreeViewModel(parents[1], _connection);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public List<Dog> LoadDogs(int dogId)
    {
        List<Dog> dogs = new List<Dog>();

        using (var cmd = _connection.CreateCommand())
        {
            cmd.CommandText = @"
                SELECT id_otec, id_matka, id_pes 
                FROM pes 
                START WITH id_pes = :dogId 
                CONNECT BY PRIOR id_pes = id_otec OR PRIOR id_pes = id_matka";

            cmd.Parameters.Add(new OracleParameter("dogId", dogId));

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idPes = reader.GetInt32(reader.GetOrdinal("id_pes"));
                    int? idOtec = reader.IsDBNull(reader.GetOrdinal("id_otec")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("id_otec"));
                    int? idMatka = reader.IsDBNull(reader.GetOrdinal("id_matka")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("id_matka"));

                    Dog dog = new Dog
                    {
                        ID = idPes,
                        Otec = idOtec != null ? new Dog { ID = idOtec.Value } : null,
                        Matka = idMatka != null ? new Dog { ID = idMatka.Value } : null
                    };

                    dogs.Add(dog);
                }
            }
        }

        return dogs;
    }
}