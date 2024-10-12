using System.Windows;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;

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
                MessageBox.Show("Nenašel se soubor secrets.json s tajnými konstantami ve složce programu nebo formát souboru není JSON.","Chyba",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        con.Open();
                        cmd.BindByName = true;

                        //Use the command to display employee names from 
                        // the EMPLOYEES table
                        cmd.CommandText = "select jmeno from psi where vek > :id";

                        // Assign id to the department number 50 
                        OracleParameter id = new OracleParameter("id", 10_000_000+"; select 10 from dual as fail");
                        cmd.Parameters.Add(id);

                        //Execute the command and use DataReader to display the data
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())//for every row
                        {
                            /*await*/ MessageBox.Show(reader.GetString(0));
                        }

                        reader.Dispose();
                    }
                    catch (Exception ex)//something went wrong
                    {

                    }
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            mw.Show();
        }
    }
}