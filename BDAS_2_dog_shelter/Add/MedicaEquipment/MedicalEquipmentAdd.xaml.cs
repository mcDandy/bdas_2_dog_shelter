using BDAS_2_dog_shelter.Add.feed;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.MedicaEquipment
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class MedicalEquipmentAdd : Window
    {
        public MedicalEquipmentAdd(Tables.Medical_Equipment d, List<Tables.Storage> storages)
        {
            InitializeComponent();
            DataContext = new MedicalEquipmentViewModelAdd(d, storages);
            ((MedicalEquipmentViewModelAdd)DataContext).OkClickFinished += () => DialogResult = true;
        }
      
    }
}
