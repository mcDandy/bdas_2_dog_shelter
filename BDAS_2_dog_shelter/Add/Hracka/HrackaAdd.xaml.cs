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

namespace BDAS_2_dog_shelter.Add.Hracka
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class HrackaAdd : Window
    {
        public HrackaAdd()
        {
            InitializeComponent();
            Tables.Hracka d = new();
            this.DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public HrackaAdd(Tables.Hracka d)
        {
            InitializeComponent();
            this.DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }

        public HrackaAdd(List<Tables.Storage> storages)
        {
            InitializeComponent();
            Tables.Hracka d = new();
            this.DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }

        public HrackaAdd(Tables.Hracka d, List<Tables.Storage> storages) : this(d)
        {
            InitializeComponent();
            this.DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
    }
}
