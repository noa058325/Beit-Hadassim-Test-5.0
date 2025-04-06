using System.ComponentModel.DataAnnotations.Schema;

namespace StoreBackend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Order
    {
        [Key]
        public long Id { get; set; }

        // ��� �� ��� ������
        public long GrocerId { get; set; }
        public Grocer Grocer { get; set; }

        // ��� �� ����
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        // ��� �� �����
        public long StockId { get; set; }
        public Stock Stock { get; set; }

        public int Quantity { get; set; }

        // ����� ������ (Enum)
        public OrderStatus Status { get; set; }
    }
}

public enum OrderStatus
{
    Pending,   // ������
    Approved,  // ������
    Completed  // ������
}

