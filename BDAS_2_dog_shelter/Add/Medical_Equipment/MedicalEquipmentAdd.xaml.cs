using System.Windows;

namespace BDAS_2_dog_shelter.Add.Medical_Equipment
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class MedicalEquipmentAdd : Window
    {
        public MedicalEquipmentAdd()
        {
            InitializeComponent();
            Tables.Medical_Equipment d = new();
            DataContext = new MedicalEquipmentViewModelAdd(d);
            ((MedicalEquipmentViewModelAdd)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public MedicalEquipmentAdd(Tables.Medical_Equipment d)
        {
            InitializeComponent();
            DataContext = new MedicalEquipmentViewModelAdd(d);
            ((MedicalEquipmentViewModelAdd)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
