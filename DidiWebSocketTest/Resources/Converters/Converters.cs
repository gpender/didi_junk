using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AC30Scope.Resources
{
    public class BooleanInverterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool en = bool.Parse(value.ToString().ToLowerInvariant());
                return !en;
            }
            catch
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            try
            {
                bool en = bool.Parse(value.ToString().ToLowerInvariant());
                return !en;

            }
            catch
            {
                return false;
            }
        }
    }

    public class ChannelCountToColumnCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int channelsPerColumn = 4;
                try
                {
                    channelsPerColumn = int.Parse(parameter.ToString());
                }
                catch { }
                ICollection col = value as ICollection;
                uint tmp = (uint)((col.Count + channelsPerColumn - 1) / channelsPerColumn);
                return tmp;
            }
            catch
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>   
    /// A type converter for visibility and boolean values.   
    /// </summary>   
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Color c = (Color)value;
                return new SolidColorBrush(c);
            }
            catch
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BorderThicknessToStrokeThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Thickness t = (Thickness)value;
                return t.Top;
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class VisToBool : IValueConverter
    {

        private bool _inverted = false;
        private bool _not = false;

        public bool Inverted
        {
            get { return _inverted; }
            set { _inverted = value; }
        }

        public bool Not
        {
            get { return _not; }
            set { _not = value; }
        }

        private object VisibilityToBool(object value)
        {
            if (!(value is Visibility))
                return DependencyProperty.UnsetValue;
            return (((Visibility)value) == Visibility.Visible) ^ Not;
        }

        private object BoolToVisibility(object value)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;

            return ((bool)value ^ Not) ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverted ? BoolToVisibility(value)
                : VisibilityToBool(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Inverted ? VisibilityToBool(value)
                : BoolToVisibility(value);
        }
    }

    public class VisToNull : IValueConverter
    {
        private bool _not = false;

        public bool Not
        {
            get { return _not; }
            set { _not = value; }
        }

        private object NullToVisibility(object value)
        {
            return ((value == null) ^ Not) ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return NullToVisibility(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BytesPerStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string size = "0 KB/s";

            if (value != null)
            {

                double byteCount = 0;

                byteCount = System.Convert.ToDouble(value);

                if (byteCount >= 1073741824)
                    size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB/s";
                else if (byteCount >= 1048576)
                    size = String.Format("{0:##.##}", byteCount / 1048576) + " MB/s";
                else if (byteCount >= 1024)
                    size = String.Format("{0:##.##}", byteCount / 1024) + " KB/s";
                else if (byteCount > 0 && byteCount < 1024)
                    size = "1 KB/s";    //Bytes are unimportant ;)            
            }

            return size;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BytesStringToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string size = "0 KB";

            if (value != null && value.ToString() != "<DIR>")
            {

                double byteCount = 0;

                byteCount = System.Convert.ToDouble(value);

                if (byteCount >= 1073741824)
                    size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB";
                else if (byteCount >= 1048576)
                    size = String.Format("{0:##.##}", byteCount / 1048576) + " MB";
                else if (byteCount >= 1024)
                    size = String.Format("{0:##.##}", byteCount / 1024) + " KB";
                else if (byteCount > 0 && byteCount < 1024)
                    size = "1 KB";    //Bytes are unimportant ;)            
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
