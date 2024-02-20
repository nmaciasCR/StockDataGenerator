using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business.Interfaces
{
    public interface ICurrencies
    {
        List<Repositories.Model.Entities.Currencies> GetList();
        Task<Boolean> RefreshFromService();
        bool UpdateList(List<Repositories.Model.Entities.Currencies> currenciesToUpdate);
    }
}
