using System;
using System.IO;

namespace Computer.API
{
    public class Logger
    {
        public static Logger Instance { get; private set; }

        public Logger()
        {
            Logger.Instance = this;
        }
        public static void Log(string sText)
        {
            LogWrite("LOG", sText);
        }
        public static void LogErr(string sText)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            LogWrite("ERR", sText);
            Console.ResetColor();
        }
        public static void Log(Exception exc)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            LogWrite("EXC", exc.ToString());
            Console.ResetColor();
        }
        public static void Log(string sText, Exception exc)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            LogWrite("EXC", $"{sText}: {exc}");
            Console.ResetColor();
        }
        private static void LogWrite(string sTag, string sText)
        {
            Console.WriteLine($"[{sTag} {DateTime.Now:HH:mm:ss:fff}] {sText}");
        }
    }
}
