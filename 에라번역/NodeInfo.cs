using Fillter2.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 에라번역
{
    public class NodeInfo
    {

        public string ID => ErbName + "|" + LineNo;

        public int LineNo { get; }

        public string ErbName { get; }

        public string ErbFileName { get; }

        public LineInfo Info { get; }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public string GetString(LineSetting setting)
        {
            return setting.GetLine(LineNo + 1, Info.PrintStr);
        }

        public NodeInfo(int line, string erbName, LineInfo info)
        {
            LineNo = line;
            ErbName = erbName;
            Info = info;
            ErbFileName = System.IO.Path.GetFileName(erbName);
        }
    }
}
