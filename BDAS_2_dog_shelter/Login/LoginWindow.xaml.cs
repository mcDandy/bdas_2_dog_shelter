using System.Windows;
using Oracle.ManagedDataAccess.Client;
using static BDAS_2_dog_shelter.Secrets;
using System.Security.Cryptography;
using System.Text;


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

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
                        cmd.CommandText = "select PERMS from USERS where UNAME = :id and PASSWD = :pw";
                        SHA256 sha256 = SHA256.Create();

                        // Assign id to the department number 50 
                        OracleParameter id = new OracleParameter("id", inp_username.Text);
                        OracleParameter id1 = new OracleParameter("pw", sha256.ComputeHash(Encoding.UTF8.GetBytes(inp_password.Text)));
                        cmd.Parameters.Add(id);
                        cmd.Parameters.Add(id1);

                        //Execute the command and use DataReader to display the data
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())//for every row
                        {
                            /*await*/ 
                            MainWindow mw = new(reader.GetInt64(0));
                            this.Close();
                            mw.Show();
                        }
                        if (!reader.HasRows) MessageBox.Show("Nesprávné uživatelské jméno nebo heslo."); 

                        reader.Dispose();
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
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
                        cmd.CommandText = "insert into USERS (UNAME,PASSWD) VALUES (:id, :pw)";
                        SHA256 sha256 = SHA256.Create();

                        // Assign id to the department number 50 
                        OracleParameter id = new OracleParameter("id", inp_username.Text);
                        OracleParameter id1 = new OracleParameter("pw", sha256.ComputeHash(Encoding.UTF8.GetBytes(inp_password.Text)));
                        cmd.Parameters.Add(id);
                        cmd.Parameters.Add(id1);

                        //Execute the command and use DataReader to display the data
                         cmd.ExecuteNonQuery();
                        con.Commit();
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show("Uživatel existuje.");
                    }
                }
            }
        }
    }
}