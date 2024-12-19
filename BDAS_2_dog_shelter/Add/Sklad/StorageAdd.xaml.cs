using System.ComponentModel;
using System.Windows;

namespace BDAS_2_dog_shelter.Add.Storage
{
    /// <summary>
    /// Interakční logika pro AdressAdd.xaml
    /// </summary>
    public enum SkladTypy
     {

        [Description("Hračky")]
        Hracky,
        [Description("Zdravotnický materiál")]
        Zdr_Material,
        [Description("Krmivo")]
        Krmivo
        }
    public partial class StorageAdd : Window
    {
      
        public StorageAdd()
        {
            InitializeComponent();
            Tables.Storage d = new();
            DataContext = new AddStorageViewModel(d);
            ((AddStorageViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public StorageAdd(Tables.Storage d)
        {
            InitializeComponent();
            DataContext = new AddStorageViewModel(d);
            ((AddStorageViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
