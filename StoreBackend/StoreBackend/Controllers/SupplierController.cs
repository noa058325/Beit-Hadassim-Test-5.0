using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Data;
using StoreBackend.Models;
using StoreBackend.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly StoreContext _context;

        public SupplierController(StoreContext context)
        {
            _context = context;
        }

        // רישום ספק חדש עם סחורות
        [HttpPost("register")]
        public async Task<ActionResult<Supplier>> RegisterSupplier([FromBody] Supplier supplier)
        {
            // הוספת הספק למסד הנתונים
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            // הוספת הסחורות שהספק סיפק (אם יש)
            if (supplier.Stocks != null && supplier.Stocks.Any())
            {
                foreach (var stock in supplier.Stocks)
                {
                    stock.SupplierId = supplier.Id; // קישור הסחורה לספק
                    _context.Stocks.Add(stock); // הוספת הסחורה למסד הנתונים
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }

        // כניסת ספק
        [HttpPost("login")]
        public async Task<ActionResult<Supplier>> LoginSupplier([FromBody] SupplierRequest supplierRequest)
        {
            var supplier = await _context.Suppliers
                                         .FirstOrDefaultAsync(s => s.Name == supplierRequest.Name && s.Password == supplierRequest.Password);

            if (supplier == null)
            {
                return Unauthorized("שם משתמש או סיסמה שגויים");
            }

            return Ok(supplier);
        }

        // צפייה בהזמנות של הספק
        [HttpGet("orders/{supplierId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersForSupplier(long supplierId)
        {
            var orders = await _context.Orders
                                       .Where(o => o.SupplierId == supplierId)
                                       .Include(o => o.Stock)
                                       .ToListAsync();

            return Ok(orders);
        }

        // אישור הזמנה על ידי הספק - סטטוס "בתהליך"
        [HttpPut("approve/{orderId}")]
        public async Task<ActionResult<Order>> ApproveOrder(long orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return NotFound("ההזמנה לא נמצאה");
            }

            order.Status = OrderStatus.Approved; // שינוי לסטטוס "בתהליך"
            await _context.SaveChangesAsync();

            return Ok(order);
        }

  

        // קבלת ספק לפי מזהה
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(long id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound("הספק לא נמצא");
            }

            return Ok(supplier);
        }
    }
}
