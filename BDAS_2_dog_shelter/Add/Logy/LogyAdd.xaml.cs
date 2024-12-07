using BDAS_2_dog_shelter.Add.Dog;
using BDAS_2_dog_shelter.Tables;
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

namespace BDAS_2_dog_shelter.Add.Logy
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class LogyAdd : Window
    {
        public LogyAdd()
        {
            InitializeComponent();
            Tables.Logs d = new();
            this.DataContext = new AddLogsViewModel(d);
            ((AddLogsViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public LogyAdd(Tables.Logs d)
        {
            InitializeComponent();
            this.DataContext = new AddLogsViewModel(d);
            ((AddLogsViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
