using System.Globalization;
using System.Windows.Data;

namespace BDAS_2_dog_shelter.Conversion
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int i = Int32.Parse(value.ToString()??"0");
            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
    public class StringToUlongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ulong i = ulong.Parse(value.ToString()??"0");
            return i;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
    public class SecondItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((List<Tuple<int?,string>>)value).Select(a => a.Item2).ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StringToTime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeOnly)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeOnly.Parse(value as string);
        }
    }
    
    public class DateTimeToDate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateOnly)value).ToDateTime(TimeOnly.MinValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateOnly.FromDateTime((DateTime)value);
        }
    }

}
