using Fillter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 에라번역
{
    public class NodeInfo
    {
        private int line;
        private string erbName;
        private string erbFileName;
        private LineInfo info;

        public string ID
        {
            get
            {
                return ErbName + "|" + Line;
            }
        }

        public int Line
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }

        public string ErbName
        {
            get
            {
                return erbName;
            }

            set
            {
                erbName = value;
            }
        }

        public string ErbFileName
        {
            get
            {
                return erbFileName;
            }

            set
            {
                erbFileName = value;
            }
        }

        public LineInfo Info
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
            return setting.GetLine(Line + 1, Info.Str);
        }

        public NodeInfo(int line, string erb_name, LineInfo info)
        {
            Line = line;
            ErbName = erb_name;
            Info = info;
            ErbFileName = erb_name.Split('\\').Last();
        }
    }
}
