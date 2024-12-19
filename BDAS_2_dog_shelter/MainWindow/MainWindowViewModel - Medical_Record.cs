using BDAS_2_dog_shelter.Add.Medical_Record;
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
        private RelayCommand MedRecordadhCMD;
        private RelayCommand<object> MedRecordrmhCMD;
        private RelayCommand<object> MedRecordedhCMD;
        private int _mrcSelectedIndex = -1;

        public ICommand cmdMedRecordAdd => MedRecordadhCMD ??= new RelayCommand(CommandMedicalRecAdd, () => Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_INSERT));
        public ICommand cmdMedRecordRm => MedRecordrmhCMD ??= new RelayCommand<object>(CommandMedicalRecRemove, (p) => p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_DELETE)&& MedicalRSI > -1);
        public ICommand cmdMedRecordEd => MedRecordedhCMD ??= new RelayCommand<object>(CommandMedicalRecEdit, (p) => p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_UPDATE) && MedicalRSI > -1);
        public ObservableCollection<Medical_Record> MedicalRec { get; set; } = new();

        public int MedicalRSI { get => _mrcSelectedIndex; set { if (_mrcSelectedIndex != value) { _mrcSelectedIndex = value; MedRecordedhCMD?.NotifyCanExecuteChanged(); MedRecordrmhCMD?.NotifyCanExecuteChanged(); } } }

        private void CommandMedicalRecEdit(object? obj)
        {
            MedicalRecAdd s = new(((IEnumerable)obj).Cast<Medical_Record>().First(),Typy.ToList());
            s.ShowDialog();
        }

        private void CommandMedicalRecRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.ZDR_ZAZNAM_DELETE) > 0)
            {
                List<Medical_Record> e = new List<Medical_Record>();
                foreach (Medical_Record d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Medical_Record shelter in e)
                {
                    MedicalRec.Remove(shelter);
                }
            }
        }

        private void CommandMedicalRecAdd()
        {
            MedicalRecAdd s = new MedicalRecAdd(Typy.ToList());
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                MedicalRec.Add(((AddMedicalRecViewModel)s.DataContext).MedicalRec);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_UPDATE)) MedicalRec.Last().PropertyChanged += MedicalRecChanged;
            }
        }

        private void LoadMedicalRec(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();

            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_SELECT))
            {
                using (var cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_zaznam, datum_zaz, typ_procedury from W_ZDR_ZAZNAM";
                        using (var v = cmd.ExecuteReader())
                        {
                            if (v.HasRows)
                            {
                             while (v.Read())
                                {
                                    MedicalRec.Add(new(v.GetInt32(0), v.GetDateTime(1), v.GetInt32(2)));
                                    MedicalRec.Last().Type = Typy.Where(a => a.Id == MedicalRec.Last().TypeProcId).FirstOrDefault();
                                    if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDR_ZAZNAM_UPDATE)) MedicalRec.Last().PropertyChanged += MedicalRecChanged;
                                }
                            }
                        }

                        // Processing relations, assuming TypeProcId relates to another record (this logic depends on your schema)
                        foreach (var record in MedicalRec)
                        {
                            record.medRecord = MedicalRec.FirstOrDefault(d => d.id == record.TypeProcId);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading medical records: {ex.Message}");
                    }
                }
            }
        }

        private async void MedicalRecChanged(object? sender, PropertyChangedEventArgs e)
        {
            Medical_Record? dog = sender as Medical_Record;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveMedicalRec(dog);
            }
        }

        private async Task SaveMedicalRec(Medical_Record utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_ZAZNAM", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_ZAZNAM", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_DATUM_ZAZ", OracleDbType.Date, utulek.DateRec, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_TYP_PROCEDURY", OracleDbType.Decimal, utulek.TypeProcId, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_ZDR_ZAZNAM";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                MedicalRec.CollectionChanged -= MedicalRec_CollectionChanged;
                MedicalRec.Clear();
                LoadMedicalRec(permissions);
                MedicalRec.CollectionChanged += MedicalRec_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void MedicalRec_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Medical_Record dog in e.NewItems ?? new List<Medical_Record>())
            {
                await SaveMedicalRec(dog);

            }

            foreach (Medical_Record dog in e.OldItems ?? new List<Medical_Record>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from ZDR_ZAZNAM where id_zaznam=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        MedicalRec.CollectionChanged -= MedicalRec_CollectionChanged;
                        MedicalRec.Clear();
                        LoadMedicalRec(permissions);
                        MedicalRec.CollectionChanged += MedicalRec_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}