using BDAS_2_dog_shelter.Add.Logy;
using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;


namespace BDAS_2_dog_shelter.MainWindow
{
    internal partial class MainWindowViewModel
    {
        private RelayCommand uadlCMD;
        private RelayCommand<object> urmlCMD;

        public ICommand cmdLAdd => uadlCMD ??= new RelayCommand(CommandLogAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN)));
        public ICommand cmdLRm => urmlCMD ??= new RelayCommand<object>(CommandLogRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN)));
        public ICommand cmdLEd => uedfCMD ??= new RelayCommand<object>(CommandLogEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN)));
        public ObservableCollection<Logs> Logs { get; set; } = new();

        private void CommandLogEdit(object? obj)
        {
            LogyAdd s = new(((IEnumerable)obj).Cast<Logs>().First());
            s.ShowDialog();
        }

        private void CommandLogRemove(object? SelectedShelters)
        {
            if (Permission.HasAnyOf(permissions,Permissions.ADMIN))
            {
                List<Logs> e = new List<Logs>();
                foreach (Logs d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Logs shelter in e)
                {
                    Logs.Remove(shelter);
                }
            }
        }

        private void CommandLogAdd()
        {
            LogyAdd s = new LogyAdd();
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Logs.Add(((AddLogsViewModel)s.DataContext).Log);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN)) Logs.Last().PropertyChanged += LogsChanged;
            }
        }

        private void LoadLogs(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select ID,C_USER,EVENT_TIME,TABLE_NAME,OPERATION,OLD_VALUE,NEW_VALUE from LOGS";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Logs.Add(new(v.GetInt32(0), v.GetString(1), v.GetDateTime(2), v.GetString(3), v.GetString(4), v.IsDBNull(5)? null: v.GetString(5), v.IsDBNull(6) ? null : v.GetString(6)));
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

        private async void LogsChanged(object? sender, PropertyChangedEventArgs e)
        {
            Tables.Logs? dog = sender as Tables.Logs;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveLogs(dog);
            }
        }

        private async Task SaveLogs(Logs utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.Id is null ? new("V_ID", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID", OracleDbType.Decimal, utulek.Id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_C_USER", OracleDbType.Varchar2, utulek.CUser, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_EVENT_TIME", OracleDbType.TimeStamp, utulek.EventTime, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_TABLE_NAME", OracleDbType.Varchar2, utulek.TableName, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_OPERATION", OracleDbType.Varchar2, utulek.Operation, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_OLD_VALUE", OracleDbType.Varchar2, utulek.OldValue, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_NEW_VALUE", OracleDbType.Varchar2, utulek.NewValue, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_LOGS";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.Id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Logs.CollectionChanged -= Logs_CollectionChanged;
                Logs.Clear();
                LoadLogs(permissions);
                Logs.CollectionChanged += Logs_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Logs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Logs dog in e.NewItems ?? new List<Logs>())
            {
                await SaveLogs(dog);

            }

            foreach (Logs dog in e.OldItems ?? new List<Logs>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.Id));
                        cmd.CommandText = "delete from logs where id=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();
                    }

                    catch (Exception ex)//something went wrong
                    {
                        Logs.CollectionChanged -= Logs_CollectionChanged;
                        Logs.Clear();
                        LoadLogs(permissions);
                        Logs.CollectionChanged += Logs_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}
