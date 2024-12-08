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
            Tables.Users d = new();
            DataContext = new AddUsersViewModel(d);
            ((AddUsersViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public UsersAdd(Tables.Users d)
        {
            InitializeComponent();
            DataContext = new AddUsersViewModel(d);
            ((AddUsersViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }

        private void nam_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
