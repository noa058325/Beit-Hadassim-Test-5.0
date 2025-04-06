namespace StoreBackend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Stock
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int MinimumPurchase { get; set; }

        // ��� �� ����
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        // ��� �� ������
        [JsonIgnore]
        public List<Order> Orders { get; set; }
    }
}