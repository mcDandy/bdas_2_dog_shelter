using BDAS_2_dog_shelter.Add.Dog;
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

namespace BDAS_2_dog_shelter.Add.Shelter
{
    /// <summary>
    /// Interakční logika pro ShelterAdd.xaml
    /// </summary>
    public partial class ShelterAdd : Window
    {
        public ShelterAdd()
        {
            InitializeComponent();
            Tables.Shelter d = new();
            this.DataContext = new AddShelterViewModel(d);
            ((AddShelterViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public ShelterAdd(Tables.Shelter d)
        {
            InitializeComponent();
            this.DataContext = new AddShelterViewModel(d);
            ((AddShelterViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
