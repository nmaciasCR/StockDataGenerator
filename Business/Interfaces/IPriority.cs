using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Interfaces
{
    public interface IPriority
    {
        int DefinePriority(decimal price, List<decimal> priceAlerts);
    }
}
