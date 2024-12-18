using System.Windows;

namespace BDAS_2_dog_shelter.Add.Hracka
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class HrackaAdd : Window
    {
        public HrackaAdd(List<Tables.Storage> storages)
        {
            InitializeComponent();
            Tables.Hracka d = new();
            DataContext = new AddHrackaViewModel(d,storages.Where(x=>x.Type=="h").ToList());
            ((AddHrackaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }

        public HrackaAdd(Tables.Hracka d, List<Tables.Storage> storages) 
        {
            InitializeComponent();
            DataContext = new AddHrackaViewModel(d, storages);
            ((AddHrackaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
