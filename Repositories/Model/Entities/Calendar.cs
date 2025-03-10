using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Calendar
    {
        public int ID { get; set; }
        public int calendarTypeId { get; set; }
        public string description { get; set; }
        public DateTime scheduleDate { get; set; }
        public DateTime entryDate { get; set; }
        public bool deleted { get; set; }
        public string referenceId { get; set; }

        public virtual CalendarTypes calendarType { get; set; }
    }
}
