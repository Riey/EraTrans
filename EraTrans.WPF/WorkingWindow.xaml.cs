using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YeongHun.EraTrans.WPF
{
    /// <summary>
    /// WorkingWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkingWindow : Window
    {
        private WorkingWindowViewModel _viewModel;

        public WorkingWindow(Dictionary<string, ErbParser> parsers, Config config)
        {
            _viewModel = new WorkingWindowViewModel();
            
            foreach(var parser in parsers)
            {
                _viewModel.ParentLines.Add(new ErbParentLine(parser.Key, parser.Value.StringDictionary.Select(l => new ErbLine(l.Key, l.Value, config.LineSetting)))); 
            }

            InitializeComponent();

            DataContext = _viewModel;
        }

        private void ErbTextBlockLeftClicked(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var parentLine = ((TextBlock)sender).DataContext as ErbParentLine;
                parentLine.IsExtended = !parentLine.IsExtended;
            }
        }

        private void WorkingWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _viewModel.Width = e.NewSize.Width;
            _viewModel.Height = e.NewSize.Height;
        }

        private void SaveButtonPressed(object sender, RoutedEventArgs e)
        {

        }

        private void Save()
        {

        }

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }

    public class ListViewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return d - 150;
            return 800;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ListViewHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return d - 100;
            return 500;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class WorkingWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ErbParentLine> ParentLines { get; set; }

        private double _width, _height;
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        public WorkingWindowViewModel()
        {
            ParentLines = new ObservableCollection<ErbParentLine>();
            ParentLines.CollectionChanged += (s, e) => 
            {
                OnPropertyChanged("ParentLines");
            };
        }
    }
}
