using System.Windows;

namespace BDAS_2_dog_shelter.Add.Shelter
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class ShelterAdd : Window
    {
        public ShelterAdd()
        {
            InitializeComponent();
            Tables.Shelter d = new();
            DataContext = new AddShelterViewModel(d);
            ((AddShelterViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public ShelterAdd(Tables.Shelter d)
        {
            InitializeComponent();
            DataContext = new AddShelterViewModel(d);
            ((AddShelterViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
