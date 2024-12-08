using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using static BDAS_2_dog_shelter.Secrets;// do later https://github.com/upce-fei-course-bcsh2-2425-classroom/exercise-08-mcDandy



namespace BDAS_2_dog_shelter.Login
{
    internal class LoginWindowViewModel
    {
        private string _username;
        public string Uname { get => _username; set { if (value != _username) { _username = value; register.NotifyCanExecuteChanged();  login.NotifyCanExecuteChanged(); } } }
        public string Pwd { get => _password; set { if (value != _password) { _password = value;  register.NotifyCanExecuteChanged();  login.NotifyCanExecuteChanged();} } }
        public List<Object> selectedDogs { get; set; }

       
        public delegate void CloaseRequest();
        public event CloaseRequest OnCloaseRequest;
        private RelayCommand register;
        public ICommand Register => register ??= new RelayCommand(PerformRegister);

        private void PerformRegister()
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

                        // Assign id to the department number 50 
                        OracleParameter id = new OracleParameter("id", Uname);
                        OracleParameter id1 = new OracleParameter("pw", SHA256.HashData(Encoding.UTF8.GetBytes(Pwd)));
                        cmd.Parameters.Add(id);
                        cmd.Parameters.Add(id1);

                        //Execute the command and use DataReader to display the data
                        cmd.ExecuteNonQuery();
                        con.Commit();
                        MessageBox.Show("Registrace úspěšná.");

                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show("Uživatel existuje.");
                    }
                }
            }
        }

        private RelayCommand login;
        private RelayCommand nologin;
        private string _password;

        public ICommand Login => login ??= new RelayCommand(PerformLogin,CanLogin);
        public ICommand NoLogin => nologin ??= new RelayCommand(WhoCaresAboutLoggingIn);

        private void WhoCaresAboutLoggingIn()
        {
            MainWindow.MainWindow mw = new(0);
            OnCloaseRequest?.Invoke();
            mw.Show();
        }

        private void PerformLogin()
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

                        // Assign id to the department number 50 
                        OracleParameter id = new OracleParameter("id", Uname);
                        OracleParameter id1 = new OracleParameter("pw", SHA256.HashData(Encoding.UTF8.GetBytes(Pwd)));
                        cmd.Parameters.Add(id);
                        cmd.Parameters.Add(id1);

                        //Execute the command and use DataReader to display the data
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())//for every row, only one row ever
                        {
                            /*await*/
                            
                            MainWindow.MainWindow mw = new(UInt64.Parse(((OracleDecimal)reader.GetOracleValue(0)).ToString()));
                            OnCloaseRequest?.Invoke();
                            mw.Show();

                        }
                        if (!reader.HasRows) MessageBox.Show("Nesprávné uživatelské jméno nebo heslo.");
                        reader.Dispose();

                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }

        }
        private bool CanLogin() 
        {
            if (Uname is null or "" || Pwd is null or "") return false;
            
            return true;
        }
    }
}
