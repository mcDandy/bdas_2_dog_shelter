using BDAS_2_dog_shelter.Add.Hracka;
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

namespace BDAS_2_dog_shelter.Add.feed
{
    /// <summary>
    /// Interakční logika pro Feed_Add.xaml
    /// </summary>
    public partial class FeedAdd : Window
    {
        public FeedAdd()
        {
            InitializeComponent();
            Tables.Feed d = new();
            this.DataContext = new AddFeedViewModel(d);
            ((AddFeedViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
        public FeedAdd(Tables.Feed d, List<Tables.Storage> storages)
        {
            InitializeComponent();
            this.DataContext = new AddFeedViewModel(d);
            ((AddFeedViewModel)this.DataContext).OkClickFinished += () => this.DialogResult = true;
        }
      
    }
}
