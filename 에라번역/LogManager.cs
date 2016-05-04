using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace 에라번역
{
    [Serializable]
    public class LogManager
    {
        public Stack<ChangeLog> logs;
        public Stack<ChangeLog> back_logs;
        public LogManager()
        {
        }
        public LogManager(bool Init)
        {
            if (Init)
            {
                logs = new Stack<ChangeLog>();
                back_logs = new Stack<ChangeLog>();
            }
        }
    }
}
