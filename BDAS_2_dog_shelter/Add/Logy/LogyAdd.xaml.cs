using System.Windows;

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
            DataContext = new AddLogsViewModel(d);
            ((AddLogsViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
        public LogyAdd(Tables.Logs d)
        {
            InitializeComponent();
            DataContext = new AddLogsViewModel(d);
            ((AddLogsViewModel)DataContext).OkClickFinished += () => DialogResult = true;
        }
    }
}
