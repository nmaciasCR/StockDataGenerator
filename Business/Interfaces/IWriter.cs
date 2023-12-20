using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Interfaces
{
    public interface IWriter
    {
        public Utils.Write write { get; set; }
        public Utils.WriteLine writeLine { get; set; }
    }
}
