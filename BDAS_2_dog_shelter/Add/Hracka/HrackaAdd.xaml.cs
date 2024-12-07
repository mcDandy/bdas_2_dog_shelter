using System.Windows;

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
            DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public HrackaAdd(Tables.Hracka d)
        {
            InitializeComponent();
            DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }

        public HrackaAdd(List<Tables.Storage> storages)
        {
            InitializeComponent();
            Tables.Hracka d = new();
            DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }

        public HrackaAdd(Tables.Hracka d, List<Tables.Storage> storages) : this(d)
        {
            InitializeComponent();
            DataContext = new AddHrackaViewModel(d);
            ((AddHrackaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
