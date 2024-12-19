using BDAS_2_dog_shelter.Tables;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

public class DogTreeViewModel : INotifyPropertyChanged
{
    private Dog _dog;
    private OracleConnection _connection;

    public DogTreeViewModel(Dog dog, OracleConnection connection)
    {
        Dog = dog ?? throw new ArgumentNullException(nameof(dog)); // Ensure dog is not null
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        LoadTreeLevelsAsync().ConfigureAwait(false);
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

    // Dog name
    public string DogName => Dog?.Name ?? "Unknown Dog"; // Safe access to Name

    // Parents
    public DogTreeViewModel? Father { get; private set; }
    public DogTreeViewModel? Mother { get; private set; }

    private async Task LoadTreeLevelsAsync()
    {
        try
        {
            List<Dog> parents = await LoadDogsAsync((int)Dog.ID);
            if (parents.Count > 0)
            {
                Father = new DogTreeViewModel(parents[0], _connection);
                if (parents.Count > 1)
                {
                    Mother = new DogTreeViewModel(parents[1], _connection);
                }
            }
            OnPropertyChanged(nameof(Father));
            OnPropertyChanged(nameof(Mother));
            OnPropertyChanged(nameof(Ancestors));
        }
        catch (Exception ex)
        {
            // Implement proper error logging here
            Console.WriteLine($"Error loading tree levels: {ex.Message}");
        }
    }

    public List<DogTreeViewModel> Ancestors
    {
        get
        {
            List<DogTreeViewModel> ancestors = new List<DogTreeViewModel>();
            ancestors.AddRange(GetAncestors(1)); // Parents
            ancestors.AddRange(GetAncestors(2)); // Grandparents
            return ancestors;
        }
    }

    public List<DogTreeViewModel> GetAncestors(int levels)
    {
        var ancestors = new List<DogTreeViewModel>();
        if (levels > 0)
        {
            if (Father != null)
            {
                ancestors.Add(Father);
                ancestors.AddRange(Father.GetAncestors(levels - 1));
            }
            if (Mother != null)
            {
                ancestors.Add(Mother);
                ancestors.AddRange(Mother.GetAncestors(levels - 1));
            }
        }
        return ancestors;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task<List<Dog>> LoadDogsAsync(int dogId)
    {
        var dogs = new List<Dog>();

        using (var cmd = _connection.CreateCommand())
        {
            cmd.CommandText = @"
                SELECT id_pes, id_matka, id_otec
                FROM pes
                START WITH id_pes = :dogId 
                CONNECT BY PRIOR id_pes = id_otec OR PRIOR id_pes = id_matka";

            cmd.Parameters.Add(new OracleParameter("dogId", dogId));

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
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

    public static async Task<DogTreeViewModel> CreateAsync(Dog dog, OracleConnection connection)
    {
        var viewModel = new DogTreeViewModel(dog, connection);
        await viewModel.LoadTreeLevelsAsync();
        return viewModel;
    }
}