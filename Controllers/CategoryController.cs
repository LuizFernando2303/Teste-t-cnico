using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
  private readonly AppDbContext _db;

  public CategoryController(AppDbContext db) => _db = db;

  [HttpGet("all")]
  public async Task<IActionResult> GetAllCategories()
  {
    var categories = await _db.Categories
        .Select(c => new { c.Id, c.UserId, c.Description, c.Purpose })
        .ToListAsync();

    return Ok(new { message = "Categorias listadas com sucesso", categories });
  }

  [HttpGet("totals/{userId}")]
  public async Task<IActionResult> GetCategoryTotals(int userId)
  {
    var categories = await _db.Categories
        .Where(c => c.UserId == userId)
        .Select(c => new
        {
          c.Id,
          c.Description,

          incomes = _db.Transactions
                .Where(t => t.CategoryId == c.Id && t.Type == TransactionType.Income)
                .Sum(t => (decimal?)t.Value) ?? 0,

          expenses = _db.Transactions
                .Where(t => t.CategoryId == c.Id && t.Type == TransactionType.Expense)
                .Sum(t => (decimal?)t.Value) ?? 0
        })
        .ToListAsync();

    var result = categories.Select(c => new
    {
      c.Id,
      c.Description,
      incomes = c.incomes,
      expenses = c.expenses,
      balance = c.incomes - c.expenses
    }).ToList();

    var totalIncomes = result.Sum(c => c.incomes);
    var totalExpenses = result.Sum(c => c.expenses);

    return Ok(new
    {
      categories = result,
      summary = new
      {
        incomes = totalIncomes,
        expenses = totalExpenses,
        balance = totalIncomes - totalExpenses
      }
    });
  }

  [HttpGet("user/{userId}")]
  public async Task<IActionResult> GetUserCategories(int userId)
  {
    var categories = await _db.Categories
        .Where(c => c.UserId == userId)
        .Select(c => new { c.Id, c.UserId, c.Description, c.Purpose })
        .ToListAsync();

    return Ok(new { message = "Categorias do usuário listadas com sucesso", categories });
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
  {
    var user = await _db.Users.FindAsync(request.UserId);
    if (user == null)
      return NotFound(new { message = "Usuário não existe" });

    var exists = await _db.Categories
        .AnyAsync(c => c.UserId == request.UserId &&
                       c.Description == request.Description &&
                       c.Purpose == (CategoryType)request.Purpose);
    if (exists)
      return Conflict(new { message = "Uma categoria igual já existe" });

    var category = new Category
    {
      UserId = request.UserId,
      Description = request.Description,
      Purpose = (CategoryType)request.Purpose
    };

    await _db.Categories.AddAsync(category);
    await _db.SaveChangesAsync();

    return Ok(new
    {
      message = "Categoria criada com sucesso",
      category = new { category.Id, category.UserId, category.Description, category.Purpose }
    });
  }
}