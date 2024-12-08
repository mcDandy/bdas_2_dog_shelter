using System.Windows;

namespace BDAS_2_dog_shelter.Add.Users
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class UsersAdd : Window
    {
        public UsersAdd()
        {
            InitializeComponent();
            Tables.Shelter d = new();
            DataContext = new AddShelterViewModel(d);
            ((AddShelterViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public UsersAdd(Tables.Shelter d)
        {
            InitializeComponent();
            DataContext = new AddShelterViewModel(d);
            ((AddShelterViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
