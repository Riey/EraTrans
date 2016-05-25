using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace 인코딩_일괄변환
{
    class Program
    {
        static void Main(string[] args)
        {
            int original_code, target_code;
            if (args.Length == 0)
            {
                return;
            }
            Console.WriteLine("원본 인코딩을 입력하세요");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out original_code))
                    break;
            }
            Console.WriteLine("변환할 인코딩을 입력하세요");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out target_code))
                    break;
            }
            Encoding original = Encoding.GetEncoding(original_code);
            Encoding target = Encoding.GetEncoding(target_code);
            bool exit = false;
            while (!exit)
            {
                Console.Write("{0}--->{1}\n이렇게 변환하시겠습니까?(y/n)\n",original.EncodingName,target.EncodingName);
                char input = Console.ReadKey(true).KeyChar;
                switch (input)
                {
                    case ('y'):
                        {
                            exit = true;
                            break;
                        }
                    case ('n'):
                        {
                            return;
                        }
                    default:
                        {
                            Console.Clear();
                            continue;
                        }
                }
            }
            var files = GetFiles(new DirectoryInfo(args[0]));
            var result=Parallel.ForEach(files, file =>
            {
                try
                {
                    string text = "";
                    using (FileStream fs = file.Open(FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader reader = new StreamReader(fs, original))
                        {
                            text = reader.ReadToEnd();
                        }
                    }
                    file.Delete();
                    using (FileStream fs = file.Create())
                    {
                        using(StreamWriter writer=new StreamWriter(fs, target))
                        {
                            writer.Write(text);
                            writer.Flush();
                        }
                    }
                    Console.WriteLine(file.Name + " 완료");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });
            Console.WriteLine("변환작업이 완료되었습니다.");
            Console.Read();
        }
        static FileInfo[] GetFiles(DirectoryInfo info)
        {
            List<FileInfo> files = new List<FileInfo>(info.GetFiles("*.*").Where(f=>f.Extension.ToUpper()==".ERB"|| f.Extension.ToUpper() ==".CSV"|| f.Extension.ToUpper() =="*.ERH"));
            DirectoryInfo[] dirs = info.GetDirectories();
            if (dirs.Length > 0)
            {
                foreach(var dir in dirs)
                {
                    files.AddRange(GetFiles(dir));
                }
            }
            return files.ToArray();
        }
    }
}
