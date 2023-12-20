using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Interfaces
{
    public interface IWrite
    {
        void Info(string text);
        void Success(string text);
        void Warning(string text);
        void Error(string text);
    }
}
