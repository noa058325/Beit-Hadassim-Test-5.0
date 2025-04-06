using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Data;
using StoreBackend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrocerController : ControllerBase
    {
        private readonly StoreContext _context;

        public GrocerController(StoreContext context)
        {
            _context = context;
        }

        // כניסת בעל המכולת למערכת (בדיקה לפי שם וסיסמא)
        [HttpPost("login")]
        public async Task<ActionResult<Grocer>> Login([FromBody] Grocer grocer)
        {
            var existingGrocer = await _context.Grocers
                .FirstOrDefaultAsync(g => g.Name == grocer.Name && g.Password == grocer.Password);

            if (existingGrocer == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(existingGrocer);
        }

        // הזמנת סחורה מספק
        [HttpPost("order/{supplierId}")]
        public async Task<ActionResult<Order>> OrderStock(long supplierId, [FromBody] Order order)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId);
            if (supplier == null)
            {
                return NotFound("Supplier not found");
            }

            order.SupplierId = supplierId;
            order.Status = OrderStatus.Pending; // הזמנה בהמתנה
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(OrderStock), new { id = order.Id }, order);
        }

        // אישור הזמנה על ידי בעל המכולת - שינוי לסטטוס "הושלמה"
        [HttpPut("confirmOrder/{orderId}")]
        public async Task<ActionResult> ConfirmOrder(long orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            order.Status = OrderStatus.Completed; // עדכון הסטטוס
            await _context.SaveChangesAsync();

            return Ok(order); // החזרת ההזמנה עם הסטטוס המעודכן
        }
    }
}
