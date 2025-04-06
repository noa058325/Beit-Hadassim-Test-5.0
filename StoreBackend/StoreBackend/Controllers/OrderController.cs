using Microsoft.AspNetCore.Mvc;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly StoreContext _context;

    public OrderController(StoreContext context)
    {
        _context = context;
    }

    // הזמנת סחורה מספק
    [HttpPost("order")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (order == null)
        {
            return BadRequest("ההזמנה לא תקינה.");
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    // שליפת הזמנה לפי מזהה
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(long id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    // שליפת כל ההזמנות
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _context.Orders.ToListAsync();
        return Ok(orders);
    }



}

