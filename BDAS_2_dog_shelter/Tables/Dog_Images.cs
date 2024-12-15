using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BDAS_2_dog_shelter.Tables
{
    public class Dog_Images : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public int? id;
        private BitmapSource _obrazek;
        public BitmapSource Image
        {
            get => _obrazek;
            set
            {
                if (_obrazek != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Image)));
                    _obrazek = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
                }
            }
        }
        private string file_name;
        public string FileName
        {
            get => file_name;
            set
            {
                if (file_name != value)
                {
                    PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(FileName)));
                    file_name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileName)));
                }
            }
        }
        public Dog_Images() { byte[] b = new byte[256 * 256 * 4]; new Random().NextBytes(b); _obrazek = BitmapSource.Create(256, 256, 96, 96, PixelFormats.Bgra32, null, b, 256 * 4); ; file_name = ""; }

        public Dog_Images(int? ID, BitmapSource bitmapFrame, string FILENAME)
        {
            id = ID;
            Image = bitmapFrame;
            FileName = FILENAME;
        }

        public event PropertyChangingEventHandler? PropertyChanging;
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
