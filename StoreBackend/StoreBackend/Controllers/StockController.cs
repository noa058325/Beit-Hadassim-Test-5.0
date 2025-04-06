using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreBackend.Data;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly StoreContext _context;

    public StockController(StoreContext context)
    {
        _context = context;
    }

    // שליפת סחורות לפי ספק
    [HttpGet("supplier/{supplierId}")]
    public async Task<IActionResult> GetStockBySupplier(long supplierId)
    {
        var stocks = await _context.Stocks
            .Where(s => s.SupplierId == supplierId)
            .ToListAsync();

        if (stocks == null || !stocks.Any())
        {
            return NotFound("לא נמצאו סחורות עבור ספק זה.");
        }

        return Ok(stocks);
    }

    // שליפת סחורה לפי מזהה
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById(long id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock);
    }
}
