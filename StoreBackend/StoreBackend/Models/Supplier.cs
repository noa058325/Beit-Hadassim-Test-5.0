using StoreBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Supplier
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string CompanyName { get; set; }
    public string NumberPhone { get; set; }

    // קשר עם המלאי
    [JsonIgnore]
    public List<Stock> Stocks { get; set; } = new List<Stock>(); // לוודא שהרשימה מאותחלת

    //// קשר עם ההזמנות
    //[JsonIgnore]
    //public List<Order> Orders { get; set; } = new List<Order>(); // לוודא שהרשימה מאותחלת
}