using System.Windows;

namespace BDAS_2_dog_shelter.Add.Owner
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public partial class OwnerAdd : Window
    {
        public OwnerAdd(List<Tables.Adress>? adresses)
        {
            InitializeComponent();
            Tables.Owner d = new();
            DataContext = new AddOwnerViewModel(d,adresses);
            ((AddOwnerViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public OwnerAdd(Tables.Owner d, List<Tables.Adress>? adresses)
        {
            InitializeComponent();
            DataContext = new AddOwnerViewModel(d,adresses);
            ((AddOwnerViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
