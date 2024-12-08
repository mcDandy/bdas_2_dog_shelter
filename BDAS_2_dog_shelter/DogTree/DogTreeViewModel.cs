using BDAS_2_dog_shelter.Tables;
using System.ComponentModel;

public class DogTreeViewModel : INotifyPropertyChanged
{
    private Dog _dog;

    public DogTreeViewModel(Dog dog)
    {
        Dog = dog;
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
        // Rekurzivní načtení stromu pro rodiče
        Father = Dog.Otec != null ? new DogTreeViewModel(Dog.Otec) : null;
        Mother = Dog.Matka != null ? new DogTreeViewModel(Dog.Matka) : null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
