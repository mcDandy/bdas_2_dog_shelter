using BDAS_2_dog_shelter.Add.Procedure;
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
        private RelayCommand ProcedureadhCMD;
        private RelayCommand<object> ProcedurermhCMD;
        private RelayCommand<object> ProcedureedhCMD;
        private int _selectedProcedureIndex = -1;
        public ICommand cmdProcedureAdd => ProcedureadhCMD ??= new RelayCommand(CommandProcedureAdd, () => (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_INSERT)));
        public ICommand cmdProcedureRm => ProcedurermhCMD ??= new RelayCommand<object>(CommandprocedureRemove, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_DELETE)) && ProcedureSI > -1);
        public ICommand cmdProcedureEd => ProcedureedhCMD ??= new RelayCommand<object>(CommandprocedureEdit, (p) => (p is not null && Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_UPDATE)) && ProcedureSI > -1);
        public ObservableCollection<Procedure> Procedures { get; set; } = new();

        public int ProcedureSI { get => _selectedProcedureIndex; set { if (_selectedProcedureIndex != value) { _selectedProcedureIndex = value; ProcedureedhCMD?.NotifyCanExecuteChanged(); ProcedurermhCMD?.NotifyCanExecuteChanged(); } } }

        private void CommandprocedureEdit(object? obj)
        {
            ProcedureAdd s = new(((IEnumerable)obj).Cast<Procedure>().First(),MedicalRec.ToList(),Dogs.ToList());
            s.ShowDialog();
        }

        private void CommandprocedureRemove(object? SelectedShelters)
        {
            if ((permissions & (long)Permissions.PROCEDURA_DELETE) > 0)
            {
                List<Procedure> e = new List<Procedure>();
                foreach (Procedure d in (IEnumerable)SelectedShelters) e.Add(d);
                foreach (Procedure shelter in e)
                {
                    Procedures.Remove(shelter);
                }
            }
        }

        private void CommandProcedureAdd()
        {
            ProcedureAdd s = new ProcedureAdd(MedicalRec.ToList(),Dogs.ToList());
            if (s.ShowDialog() == true)
            {
                //new("test", 10, "Cyan", DateTime.Now, ".", "Naživu");
                Procedures.Add(((AddProcedureViewModel)s.DataContext).procedure);
                if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_UPDATE)) Procedures.Last().PropertyChanged += ProcedureChanged;
            }
        }

        private void LoadProcedure(ulong permissions)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            if (Permission.HasAnyOf(permissions, Permissions.ADMIN, Permissions.PROCEDURA_SELECT))
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        cmd.CommandText = "select id_procedura,nazev_procedury,popis_procedury,zdr_zaznam_id_zaznam,id_pes from W_PROCEDURA";
                        OracleDataReader v = cmd.ExecuteReader();
                        if (v.HasRows)
                        {
                            while (v.Read())
                            {
                                Procedures.Add(new(v.GetInt32(0), v.GetString(1), v.GetString(2), v.GetInt32(3),v.GetInt32(4)));
                            }
                        }
                        List<Procedure> DogForest = Procedures.Select<Procedure, Procedure>
                               (a =>
                               {
                                   a.record = MedicalRec.Where(d => d.id == a.ZdrZaznamid).FirstOrDefault();

                                   return a;
                               }).ToList();

                        Procedures.Clear();
                        foreach (var item in DogForest)
                        {
                            Procedures.Add(item);
                        }
                    }
                    catch (Exception ex)//something went wrong
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private async void ProcedureChanged(object? sender, PropertyChangedEventArgs e)
        {
            Procedure? dog = sender as Procedure;
            using (OracleCommand cmd = con.CreateCommand())
            {

                await SaveProcedure(dog);
            }
        }

        private async Task SaveProcedure(Procedure utulek)
        {
            if (con.State == ConnectionState.Closed) con.Open();
            try
            {
                using (OracleCommand cmd = con.CreateCommand())
                {

                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(utulek.id is null ? new("V_ID_PROCEDURA", OracleDbType.Decimal, DBNull.Value, System.Data.ParameterDirection.InputOutput) : new("V_ID_PROCEDURA", OracleDbType.Decimal, utulek.id, System.Data.ParameterDirection.InputOutput));
                    cmd.Parameters.Add(new("V_NAZEV_PROCEDURY", OracleDbType.Varchar2, utulek.ProcName, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_POPIS_PROCEDURY", OracleDbType.Varchar2, utulek.DescrName, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ID_ZDR_ZAZNAM_ID_ZAZNAM", OracleDbType.Decimal, utulek.ZdrZaznamid, ParameterDirection.Input));
                    cmd.Parameters.Add(new("V_ID_PES", OracleDbType.Decimal, utulek.PesID, ParameterDirection.Input));

                    cmd.CommandText = "INS_SET.IU_PROCEDURA";

                    //Execute the command and use DataReader to display the data
                    int i = await cmd.ExecuteNonQueryAsync();
                    utulek.id = Convert.ToInt32(cmd.Parameters[0].Value.ToString());

                }
            }
            catch (Exception ex)//something went wrong
            {
                Procedures.CollectionChanged -= Procedure_CollectionChanged;
                Procedures.Clear();
                LoadProcedure(permissions);
                Procedures.CollectionChanged += Procedure_CollectionChanged;
                MessageBox.Show(ex.Message);
                return;
            }
        }
        private async void Procedure_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Procedure dog in e.NewItems ?? new List<Procedure>())
            {
                await SaveProcedure(dog);

            }

            foreach (Procedure dog in e.OldItems ?? new List<Procedure>())
            {
                using (OracleCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        if (con.State == System.Data.ConnectionState.Closed) con.Open();
                        cmd.BindByName = true;

                        // Assign id to the department number 50 
                        cmd.Parameters.Add(new("ID", dog.id));
                        cmd.CommandText = "delete from PROCEDURA where id_procedura=:ID";
                        //Execute the command and use DataReader to display the data
                        int i = await cmd.ExecuteNonQueryAsync();

                    }

                    catch (Exception ex)//something went wrong
                    {
                        Procedures.CollectionChanged -= Procedure_CollectionChanged;
                        Procedures.Clear();
                        LoadProcedure(permissions);
                        Procedures.CollectionChanged += Procedure_CollectionChanged;
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
        }
    }
}