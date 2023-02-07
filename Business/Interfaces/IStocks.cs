using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Interfaces
{
    public interface IStocks
    {

        List<Repositories.Model.Entities.Quotes> GetList();
        Boolean RefreshFromService();
    }
}
