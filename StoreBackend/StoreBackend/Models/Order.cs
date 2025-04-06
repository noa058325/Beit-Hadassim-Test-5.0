using System.ComponentModel.DataAnnotations.Schema;

namespace StoreBackend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Order
    {
        [Key]
        public long Id { get; set; }

        // קשר עם בעל המכולת
        public long GrocerId { get; set; }
        public Grocer Grocer { get; set; }

        // קשר עם הספק
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        // קשר עם סחורה
        public long StockId { get; set; }
        public Stock Stock { get; set; }

        public int Quantity { get; set; }

        // סטטוס ההזמנה (Enum)
        public OrderStatus Status { get; set; }
    }
}

public enum OrderStatus
{
    Pending,   // בהמתנה
    Approved,  // בתהליך
    Completed  // הושלמה
}

