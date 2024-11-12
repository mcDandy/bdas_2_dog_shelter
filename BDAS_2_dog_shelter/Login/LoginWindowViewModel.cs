using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
        private ulong perms = 0;
        public LoginWindowViewModel()
        {

        }
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
                        SHA256 sha256 = SHA256.Create();

                        // Assign id to the department number 50 
                        OracleParameter id = new OracleParameter("id", Uname);
                        OracleParameter id1 = new OracleParameter("pw", sha256.ComputeHash(Encoding.UTF8.GetBytes(Pwd)));
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
        private string _password;

        public ICommand Login => login ??= new RelayCommand(PerformLogin,CanLogin);

        private void PerformLogin()
        {
            MainWindow.MainWindow mw = new(perms);
            OnCloaseRequest?.Invoke();
            mw.Show();
        }
        private bool CanLogin() 
        {
            if (Uname is null || Pwd is null) return false;
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
                        OracleParameter id = new OracleParameter("id", Uname);
                        OracleParameter id1 = new OracleParameter("pw", sha256.ComputeHash(Encoding.UTF8.GetBytes(Pwd)));
                        cmd.Parameters.Add(id);
                        cmd.Parameters.Add(id1);

                        //Execute the command and use DataReader to display the data
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())//for every row
                        {
                            /*await*/

                            return true;
                        }
                        if (!reader.HasRows) MessageBox.Show("Nesprávné uživatelské jméno nebo heslo.");
                        reader.Dispose();

                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
