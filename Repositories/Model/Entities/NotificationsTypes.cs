using System;
using System.Collections.Generic;

#nullable disable

namespace StockDataGenerator.Repositories.Model.Entities
{
    public partial class NotificationsTypes
    {
        public NotificationsTypes()
        {
            Notifications = new HashSet<Notifications>();
        }

        public int ID { get; set; }
        public string name { get; set; }

        public virtual ICollection<Notifications> Notifications { get; set; }
    }
}
