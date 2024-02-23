using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace StockDataGenerator.Utils
{
    public class DateConverter : Business.Interfaces.IDateConverter
    {


        public DateConverter()
        {

        }

        /// <summary>
        /// Convierte un timestamp en una fecha, teniendo en cuenta el huso horario
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="timeZone">Ej: "America/New_York"</param>
        public DateTime? TimestampToDatetime(long? timestamp, string timeZone)
        {
            try
            {
                if (timestamp != null)
                {
                    // Convertir el timestamp a un objeto DateTime
                    DateTime dateTimeUtc = DateTimeOffset.FromUnixTimeSeconds((long)timestamp).UtcDateTime;
                    // Convertir la timezone "America/New_York" a una TimeZoneInfo válida en .NET
                    TimeZoneInfo desiredTimeZone = TZConvert.GetTimeZoneInfo(timeZone);
                    // Convertir el DateTime a la zona horaria deseada
                    DateTime desiredDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, desiredTimeZone);

                    return desiredDateTime;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }


    }
}
