using BDAS_2_dog_shelter.Tables;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
namespace BDAS_2_dog_shelter.Add.Dog_Image
{
    internal class AddDogImageViewModel
    {
        private Tables.Dog_Images d;

        RelayCommand okCommand;
        private BitmapSource _obraze;
        private string name;

        public ICommand OkCommand => okCommand ??= new RelayCommand(Ok, () => name is not null and not "" && _obraze is not null /*&& psc is not null and not < 0*/ );

        public delegate void OkUtulekAddEditDone();
        public event OkUtulekAddEditDone? OkClickFinished;

        private void Ok()
        {
            d.Image = _obraze;
            d.FileName = name;
            OkClickFinished?.Invoke();
        }

        public string Filename { get => name; set { name = value; if (okCommand is not null) okCommand.NotifyCanExecuteChanged(); } }

        public Tables.Storage Sklad;
        public List<Tables.Storage> Sklady;


        public BitmapSource Obrazek { get => _obraze; set => _obraze = value; }
        public Tables.Dog_Images Food => d;

        public AddDogImageViewModel(Tables.Dog_Images d)
        {
            _obraze = d.Image;
            name = d.FileName;
        }
    }
}