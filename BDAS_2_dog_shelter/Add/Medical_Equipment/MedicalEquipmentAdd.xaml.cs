using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Add.Medical_Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

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
            this.DataContext = new MedicalEquipmentViewModelAdd(d);
            ((MedicalEquipmentViewModelAdd)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public MedicalEquipmentAdd(Tables.Medical_Equipment d)
        {
            InitializeComponent();
            this.DataContext = new MedicalEquipmentViewModelAdd(d);
            ((MedicalEquipmentViewModelAdd)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
