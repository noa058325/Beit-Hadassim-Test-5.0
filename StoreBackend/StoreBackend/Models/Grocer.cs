namespace StoreBackend.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Grocer
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        // קשר עם ההזמנות
        [JsonIgnore]
        public List<Order> Orders { get; set; }
    }

}
