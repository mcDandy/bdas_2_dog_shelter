using System.Windows;

namespace BDAS_2_dog_shelter.Add.Medical_Record
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class MedicalRecAdd : Window
    {
        public MedicalRecAdd()
        {
            InitializeComponent();
            Tables.Medical_Record d = new();
            DataContext = new AddMedicalRecViewModel(d);
            ((AddMedicalRecViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public MedicalRecAdd(Tables.Medical_Record d)
        {
            InitializeComponent();
            DataContext = new AddMedicalRecViewModel(d);
            ((AddMedicalRecViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
