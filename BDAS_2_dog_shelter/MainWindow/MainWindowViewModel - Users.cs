using BDAS_2_dog_shelter.Add.Hracka;
using BDAS_2_dog_shelter.Add.Users;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Windows;
using System.Windows.Input;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private RelayCommand uaduCMD;
        private RelayCommand<object> urmuCMD;
        private RelayCommand<object> ueduCMD;
        private RelayCommand<object> uediCMD;
        public ICommand cmdUSAdd => uaduCMD ??= new RelayCommand(CommandUserAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_INSERT)));
        public ICommand cmdUSRm => urmuCMD ??= new RelayCommand<object>(CommandUserRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_DELETE)));
        public ICommand cmdUSEd => ueduCMD ??= new RelayCommand<object>(CommandUserEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_UPDATE)));
        public ICommand CommandImpersonate => ueduCMD ??= new RelayCommand<object>(CommandImpersonateF, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.HRACKA_UPDATE)));
        public ObservableCollection<Users> Users { get; set; } = new();

        private void CommandUserEdit(object? obj)
        {
            UsersAdd s = new(((IEnumerable)obj).Cast<Users>().First());
            s.ShowDialog();
        }

        private void CommandUserRemove(object? SelectedShelters)
        {
            if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.HRACKA_DELETE))
            {
                List<Users> e = new List<Users>();
                foreach (Users d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Users shelter in e)
                {
                    Users.Remove(shelter);
                }
            }
        }

        private void CommandUserAdd()
        {
            UsersAdd s = new UsersAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Users.Add(((AddUsersViewModel)s.DataContext).Uzivatel);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) Users.Last().PropertyChanged += UsersChanged;
            }
        }

        private void LoadUsers(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id,uname,passwd,perms from w_users";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Users.Add(new(v.GetInt32(0),v.GetString(1), v.GetString(2), UInt64.Parse(((OracleDecimal)v.GetOracleValue(3)).ToString())));
                            }
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void UsersChanged(object? sender, PropertyChangedEventArgs e)
        {
            Users? dog = sender as Users;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveUsers(dog);
            }
        }

        private async Task SaveUsers(Users utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.ID is null ? new("V_USER_ID", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_USER_ID", OracleDbType.Decimal, utulek.ID, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_UNAME", OracleDbType.Decimal, utulek.Uname, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_PASSWD", OracleDbType.Decimal, utulek.Password, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_PERMS", OracleDbType.Decimal, utulek.Perms, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_USERS";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.ID = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Users.CollectionChanged -= Users_CollectionChanged;
                Users.Clear();
                LoadUsers(permissions);
                Users.CollectionChanged += Users_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Users_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Users dog in e.NewItems ?? new List<Users>())
            {
                await SaveUsers(dog);

            }

            foreach (Users dog in e.OldItems ?? new List<Users>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.ID));
                        cmd.CommandText = "delete from users where user_id=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Users.CollectionChanged -= Users_CollectionChanged;
                        Users.Clear();
                        LoadUsers(permissions);
                        Users.CollectionChanged += Users_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
                void CommandImpersonateF(object o){ MainWindow mw = new MainWindow(((Tables.Users)o).Perms); }
            }
        }
    }
}