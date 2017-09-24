using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace 로그분석
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
                return;
            var formatter = new BinaryFormatter();
            string logPath = "";
            logPath = args[0];
            using(var fs=new FileStream(args[0], FileMode.Open))
            {
                var e = formatter.Deserialize(fs) as Exception;
                Console.WriteLine("\tSource\n");
                Console.WriteLine(e.Source);
                Console.WriteLine("\n\tMessage\n");
                Console.WriteLine(e.Message);
                Console.WriteLine("\n\tStackTrace\n");
                Console.WriteLine(e.StackTrace);
            }
            Console.WriteLine("\n\n\n\nPress Any Key to continue...");
            Console.ReadKey(true);
        }
    }
}
