using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDataGenerator.Business
{
    public class Priority : Interfaces.IPriority
    {


        public Priority()
        {

        }


        /// <summary>
        /// Define la prioridad de la accion evaluando las aletas
        /// </summary>
        public int DefinePriority(decimal price, List<decimal> priceAlerts)
        {
            int priorityId;
            //Lista con la diferencia entre el valor de la accion y el de la alerta
            List<decimal> listAbs = priceAlerts.Select(p => Math.Abs(price - p)).ToList();

            
            if (listAbs.Any(d => (int)((((price + d) / price) - 1) * 100) < 5)) {
                //ALTA PRIORIDAD (0 A 4%)
                priorityId = 1;
            } else if (listAbs.Any(d => (int)((((price + d) / price) - 1) * 100) < 10))
            {
                //MEDIA PRIORIDAD (5 A 10%)
                priorityId = 2;
            } else
            {
                //BAJA PRIORIDAD (>10%)
                priorityId = 3;
            }

            return priorityId;
        }



    }
}
