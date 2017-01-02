using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EraTrans
{
    public class LineInfo:INotifyPropertyChanged
    {
        private bool korean;
        private bool japanese;
        private string str;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string Str
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
                GetLang.Get(value, out korean, out japanese);
                OnPropertyChanged();
                OnPropertyChanged("Korean");
                OnPropertyChanged("Japanese");
            }
        }

        public bool IsForm { get; }
        public bool IsFormS { get; }
        public bool IsData { get; }
        public bool IsList { get; }
        public int PrintDataLine { get; }
        public int ListLine { get; }
        public string OriginalString { get; }
        public bool Korean => korean;
        public bool Japanese => japanese;

        public LineInfo(string str, bool isForm, bool isFormS, string originalString)
        {
            Str = str;
            IsList = false;
            IsForm = !isFormS && isForm;
            IsFormS = isFormS;
            ListLine = PrintDataLine = -1;
            OriginalString = originalString ?? str;
        }
        public LineInfo(string str, bool isForm, string originalString, int printDataLine, int listLine = -1)
        {
            //parent_line 부모 DATALIST의 줄수이며 이것으로 같은 FORM인지 구분
            Str = str;
            PrintDataLine = printDataLine;
            ListLine = listLine;
            IsData = true;
            IsList = listLine != -1;
            IsForm = isForm;
            IsFormS = false;
            OriginalString = originalString ?? str;
        }

        public override string ToString()
        {
            return $"{str}|IsForm = {IsForm}|IsFormS = {IsFormS}|IsList = {IsList}|PrintDataLine = {PrintDataLine}";
        }
    }
}
