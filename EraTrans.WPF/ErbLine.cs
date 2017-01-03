using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace YeongHun.EraTrans.WPF
{
    public class ErbLine : DependencyObject, INotifyPropertyChanged
    {
        public int LineNo { get; }
        public LineInfo Info { get; }

        private LineSetting _lineSetting;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string HeaderString => _lineSetting.GetHeaderString(LineNo);
        
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            { SetValue(TextProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ErbLine));

        public ErbLine(int lineNo, LineInfo info, LineSetting lineSetting)
        {
            LineNo = lineNo;
            Info = info;
            _lineSetting = lineSetting;

            var textBinding = new Binding()
            {
                Source = info,
                Path = new PropertyPath("Str"),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };

            BindingOperations.SetBinding(this, TextProperty, textBinding);
        }
    }
}
