using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace YeongHun.EraTrans.WPF
{
    public class ExtenedToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool extended) return extended ? Visibility.Visible : Visibility.Collapsed;

            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ErbParentLine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static readonly ObservableCollection<ErbLine> blankCollection = new ObservableCollection<ErbLine>();
        public ObservableCollection<ErbLine> Lines { get; set; }

        public string ErbName { get; }

        private bool _isExtended;
        public bool IsExtended
        {
            get => _isExtended;
            set
            {
                _isExtended = value;
                OnPropertyChanged();
            }
        }

        public ErbParentLine(string erbPath, IEnumerable<ErbLine> lines)
        {
            ErbName = Path.GetFileName(erbPath);
            Lines = new ObservableCollection<ErbLine>(lines);
            Lines.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged("Lines");
            };
        }
    }
}
