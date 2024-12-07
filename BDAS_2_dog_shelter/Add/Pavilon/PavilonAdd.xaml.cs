using System.Windows;

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
            DataContext = new AddPavilonViewModel(d);
            ((AddPavilonViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public PavilonAdd(Tables.Pavilion d)
        {
            InitializeComponent();
            DataContext = new AddPavilonViewModel(d);
            ((AddPavilonViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
