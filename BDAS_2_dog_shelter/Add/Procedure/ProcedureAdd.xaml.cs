using System.Windows;

namespace BDAS_2_dog_shelter.Add.Procedure
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class ProcedureAdd : Window
    {
        public ProcedureAdd()
        {
            InitializeComponent();
            Tables.Procedure d = new();
            DataContext = new AddProcedureViewModel(d);
            ((AddProcedureViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public ProcedureAdd(Tables.Procedure d)
        {
            InitializeComponent();
            DataContext = new AddProcedureViewModel(d);
            ((AddProcedureViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
