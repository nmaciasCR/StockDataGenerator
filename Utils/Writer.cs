using StockDataGenerator.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Utils
{
    public class Writer : Business.Interfaces.IWriter
    {
        public Write write { get; set; }
        public WriteLine writeLine { get; set; }



        public Writer()
        {
            write = new Write();
            writeLine = new WriteLine();
        }

    }
}
