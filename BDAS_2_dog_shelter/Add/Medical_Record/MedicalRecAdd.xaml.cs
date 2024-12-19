using System.Windows;

namespace BDAS_2_dog_shelter.Add.Medical_Record
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class MedicalRecAdd : Window
    {
        public MedicalRecAdd(List<Tables.KeyValueUS> typy)
        {
            InitializeComponent();
            Tables.Medical_Record d = new();
            DataContext = new AddMedicalRecViewModel(d,typy);
            ((AddMedicalRecViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public MedicalRecAdd(Tables.Medical_Record d, List<Tables.KeyValueUS> typy)
        {
            InitializeComponent();
            DataContext = new AddMedicalRecViewModel(d,typy);
            ((AddMedicalRecViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
