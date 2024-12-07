using System.Windows;

namespace BDAS_2_dog_shelter.Add.Karantena
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class KarantenaAdd : Window
    {
        public KarantenaAdd()
        {
            InitializeComponent();
            Tables.Quarantine d = new();
            DataContext = new AddKarantenaViewModel(d);
            ((AddKarantenaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public KarantenaAdd(Tables.Quarantine d)
        {
            InitializeComponent();
            DataContext = new AddKarantenaViewModel(d);
            ((AddKarantenaViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
