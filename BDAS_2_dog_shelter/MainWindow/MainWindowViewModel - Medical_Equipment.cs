using BDAS_2_dog_shelter.Add.Medical_Equipment;
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
        private RelayCommand MedicaladhCMD;
        private RelayCommand<object> MedicalrmhCMD;
        private RelayCommand<object> MedicaledhCMD;
        private int _medicalSelectedIndex;

        public ICommand cmdMedicalAdd => MedicaladhCMD ??= new RelayCommand(CommandMedicalAdd, () => Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_INSERT));
        public ICommand MedicalHRm => MedicalrmhCMD ??= new RelayCommand<object>(CommandMedicalRemove, (p) => p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_DELETE) && MeidcalSI > -1);
        public ICommand cmdMedicalEd => MedicaledhCMD ??= new RelayCommand<object>(CommandMedicalEdit, (p) => p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_UPDATE) && MeidcalSI > -1);
        public ObservableCollection<Medical_Equipment> Medical_Equipment { get; set; } = new();

        public int MeidcalSI { get => _medicalSelectedIndex; set { if (_medicalSelectedIndex != value) { _medicalSelectedIndex = value; MedicalrmhCMD?.NotifyCanExecuteChanged(); MedicaledhCMD?.NotifyCanExecuteChanged(); } } }

        private void CommandMedicalEdit(object? obj)
        {
            MedicalEquipmentAdd s = new(((IEnumerable)obj).Cast<Medical_Equipment>().First(),Storages.ToList());
            s.ShowDialog();
        }

        private void CommandMedicalRemove(object? Selectedmedical)
        {
            if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.ZDRAVOTNICKY_MATERIAL_DELETE))
            {
                List<Medical_Equipment> e = new List<Medical_Equipment>();
                foreach (Medical_Equipment d in (IEnumerable)Selectedmedical) e.Add(d);
                foreach (Medical_Equipment medi in e)
                {
                    Medical_Equipment.Remove(medi);
                }
            }
        }

        private void CommandMedicalAdd()
        {
            MedicalEquipmentAdd s = new MedicalEquipmentAdd(Storages.ToList());
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Medical_Equipment.Add(((MedicalEquipmentViewModelAdd)s.DataContext).Medical_equipment);
                if (Permission.HasAnyOf(permissions,Permissions.ADMIN,Permissions.ZDRAVOTNICKY_MATERIAL_UPDATE)) Medical_Equipment.Last().PropertyChanged += MedicalChanged;
            }
        }

        private void LoadMedical(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.ZDRAVOTNICKY_MATERIAL_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_material,nazev,user,id_sklad from W_ZDRAVOTNICKY_MATERIAL";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Medical_Equipment.Add(new(v.GetInt32(0), v.GetString(1), v.GetInt32(2), v.GetInt32(3)));
                            }
                        }
                        List<Medical_Equipment> DogForest = Medical_Equipment.Select<Medical_Equipment, Medical_Equipment>
                               (a => {
                                   a.Sklad = Storages.Where(d => d.id == a.SkladID).FirstOrDefault();

                                   return a;
                               }).ToList();

                        Medical_Equipment.Clear();
                        foreach (var item in DogForest)
                        {
                            Medical_Equipment.Add(item);
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }  
        }

        private async void MedicalChanged(object? sender, PropertyChangedEventArgs e)
        {
            
            Medical_Equipment? medical = sender as Medical_Equipment;
            using (OracleCommand cmd = con.CreateCommand())
            {

                    await SaveMedical(medical);
            }
        }

        private async void Medical_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Medical_Equipment medical in e.NewItems ?? new List<Medical_Equipment>())
            {
                await SaveMedical(medical);

            }

            foreach (Medical_Equipment dog in e.OldItems ?? new List<Medical_Equipment>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from Zdravotnicky_material where id_material=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Medical_Equipment.CollectionChanged -= Medical_CollectionChanged;
                        Medical_Equipment.Clear();
                        LoadMedical(permissions);
                        Medical_Equipment.CollectionChanged += Medical_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
        private async Task SaveMedical(Medical_Equipment utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_MATERIAL", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_MATERIAL", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_NAZEV", OracleDbType.Varchar2, utulek.MedicalName, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_POCET", OracleDbType.Decimal, utulek.CountMedical, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ID_SKLAD", OracleDbType.Decimal, utulek.SkladID, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_ZDRAVOTNICKY_MATERIAL";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Medical_Equipment.CollectionChanged -= Medical_CollectionChanged;
                LoadMedical(permissions);
                Medical_Equipment.CollectionChanged += Medical_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
    } 
}
