using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Utils
{
    public class WriteLine : Business.Interfaces.IWrite
    {
        public void Error(string text)
        {
            ConsoleColor colorDefault = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = colorDefault;
        }

        public void Info(string text)
        {
            Console.WriteLine(text);
        }

        public void Success(string text)
        {
            ConsoleColor colorDefault = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = colorDefault;
        }

        public void Warning(string text)
        {
            ConsoleColor colorDefault = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = colorDefault;
        }
    }
}
