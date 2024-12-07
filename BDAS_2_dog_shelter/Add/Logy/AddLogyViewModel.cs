using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Logy
{
    internal class AddLogsViewModel
    {
        private Tables.Logs d;
        private DateOnly eventdate;
        private TimeOnly eventtime;
        private string user;
        private int? iD;
        private int? sklad;
        Tables.Storage storage;
        RelayCommand okCommand;
        private string oldvalue;
        private string newvalue;

        public ICommand OkHCommand => okCommand ??= new RelayCommand(Ok, () => /*Validation.TimeRule.RegexClock().IsMatch(EventTime) &&*/ true);

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.EventTime = EventDateTime;
            d.CUser = user;
            d.OldValue = oldvalue;
            d.NewValue = newvalue;
            OkClickFinished?.Invoke();
        }

        public TimeOnly EventTime { get => eventtime;  set { eventtime = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public DateOnly EventDate { get => eventdate;  set { eventdate = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }
        public string User { get => user;  set => user = value; }
        public int? ID { get => iD;  set => iD = value; }

        public Tables.Logs Hracka => d;

        public string OldValue { get => oldvalue; set => oldvalue = value; }
        public string NewValue { get => newvalue; set => newvalue = value; }
        public DateTime EventDateTime { get => eventdate.ToDateTime(eventtime); set { eventtime = TimeOnly.FromDateTime(value); eventdate = DateOnly.FromDateTime(value); } }

        public AddLogsViewModel(Tables.Logs logs)
        {
            this.d = logs;
            EventDateTime = logs.EventTime;
            this.User = d.CUser;
            this.newvalue = d.NewValue;
            this.oldvalue = d.OldValue;
        }
    }
}