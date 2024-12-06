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

namespace BDAS_2_dog_shelter.Add.Pavilon
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class PavilonAdd : Window
    {
        public PavilonAdd()
        {
            InitializeComponent();
            Tables.Pavilion d = new();
            this.DataContext = new AddPavilonViewModel(d);
            ((AddPavilonViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public PavilonAdd(Tables.Pavilion d)
        {
            InitializeComponent();
            this.DataContext = new AddPavilonViewModel(d);
            ((AddPavilonViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
