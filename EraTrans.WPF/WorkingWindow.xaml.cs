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
        private Config _config;
        private WorkingWindowViewModel _viewModel;
        private Dictionary<string, ErbParser> _parsers;
        private bool _changed = false;

        public WorkingWindow(Dictionary<string, ErbParser> parsers, Config config)
        {
            _viewModel = new WorkingWindowViewModel(config);
            _parsers = parsers;
            _config = config;
            
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

        private void AutoTranslateButtonPressed(object sender, RoutedEventArgs e)
        {
            foreach(var selectedLine in _selectedLines)
            {
                var result = AutoTransFillter.TranslateWithFillter(selectedLine.Info);
                if (result != null)
                    selectedLine.Info.Str = result;
            }
        }

        private void BatchTranslateButtonPressed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("미구현입니다");
        }

        private void SaveButtonPressed(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void ExitButtonPressed(object sender, RoutedEventArgs e)
        {
            _changed = false;
            Close();
        }

        private void Save()
        {
            foreach(var parser in _parsers)
            {
                parser.Value.Save(_config.OutputType);
            }
            _changed = false;
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                switch(e.Key)
                {
                    case Key.S:
                        Save();
                        break;
                }
            }
        }

        private void WorkingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _changed = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_changed)
            {
                switch (MessageBox.Show("저장되지 않았습니다 저장 하시겠습니까?", "", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Yes:
                        Save();
                        return;
                    case MessageBoxResult.No:
                        return;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
        }

        private List<ErbLine> _selectedLines = new List<ErbLine>();
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (ErbLine addedLine in e.AddedItems)
                _selectedLines.Add(addedLine);
            foreach (ErbLine removedLine in e.RemovedItems)
                _selectedLines.Remove(removedLine);
        }
    }

    public class ListViewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return Math.Max(0, d - 200);
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
                return Math.Max(0, d - 100);
            return 500;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class WorkingWindowViewModel : INotifyPropertyChanged
    {
        private Config _config;


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

        public bool SaveOriginalString
        {
            get => _config.SaveOriginalString;
            set => _config.SaveOriginalString = value;
        }

        public WorkingWindowViewModel(Config config)
        {
            ParentLines = new ObservableCollection<ErbParentLine>();
            ParentLines.CollectionChanged += (s, e) => 
            {
                OnPropertyChanged("ParentLines");
            };

            _config = config;
        }
    }
}
