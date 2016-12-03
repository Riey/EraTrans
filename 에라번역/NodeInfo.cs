
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EraTrans
{
    public class NodeInfo
    {
        private int lineNo;
        private string erbPath;
        private string erbFileName;
        private LineInfo info;

        public string ID
        {
            get
            {
                return ErbPath + "|" + LineNo;
            }
        }
        public int LineNo
        {
            get
            {
                return lineNo;
            }
        }
        public string ErbPath
        {
            get
            {
                return erbPath;
            }
        }
        public string ErbFileName
        {
            get
            {
                return erbFileName;
            }
        }
        public LineInfo LineInfo
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public string GetString(LineSetting setting)
        {
            return setting.GetLine(LineNo + 1, LineInfo.Str);
        }

        public NodeInfo(int lineNo, string erbPath, LineInfo info)
        {
            this.lineNo = lineNo;
            this.erbPath = erbPath;
            LineInfo = info;
            erbFileName = Path.GetFileName(erbPath);
        }
    }
}
