using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace YeongHun.EraTrans.WPF
{
    public class ErbLine : INotifyPropertyChanged
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
            get => Info.Str;
            set => Info.Str = value;
        }

        public ErbLine(int lineNo, LineInfo info, LineSetting lineSetting)
        {
            LineNo = lineNo;
            Info = info;
            _lineSetting = lineSetting;
        }
    }
}
