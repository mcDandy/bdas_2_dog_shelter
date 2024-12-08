using System.Windows;

namespace BDAS_2_dog_shelter.Add.Procedure
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class ProcedureAdd : Window
    {
        public ProcedureAdd(List<Tables.Medical_Record>? zaznamy, List<Tables.Dog> dogs)
        {
            InitializeComponent();
            Tables.Procedure d = new();
            DataContext = new AddProcedureViewModel(d,zaznamy,dogs);
            ((AddProcedureViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public ProcedureAdd(Tables.Procedure d, List<Tables.Medical_Record>? zaznamy, List<Tables.Dog> dogs)
        {
            InitializeComponent();
            DataContext = new AddProcedureViewModel(d,zaznamy,dogs);
            ((AddProcedureViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
