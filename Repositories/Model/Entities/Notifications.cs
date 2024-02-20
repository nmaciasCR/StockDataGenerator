using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class Notifications
    {
        public int ID { get; set; }
        public int notificationTypeId { get; set; }
        public DateTime entryDate { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string referenceId { get; set; }
        public bool active { get; set; }
        public bool deleted { get; set; }

        public virtual NotificationsTypes notificationType { get; set; }
    }
}
