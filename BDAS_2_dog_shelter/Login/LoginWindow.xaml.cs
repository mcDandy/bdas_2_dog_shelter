using System.Windows;
using static BDAS_2_dog_shelter.Secrets;
using BDAS_2_dog_shelter.Login;
using System.Windows.Controls;

namespace BDAS_2_dog_shelter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                if (ConnectionString == null) return;
            }
            catch (System.TypeInitializationException)
            {
                MessageBox.Show("Nenašel se soubor secrets.json s tajnými konstantami ve složce programu nebo formát souboru není JSON.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ((LoginWindowViewModel)DataContext).OnCloaseRequest += Colse;

        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Pwd = ((PasswordBox)sender).SecurePassword; }
        }
        private void Colse()
        {
            Close();
        }
    }
}