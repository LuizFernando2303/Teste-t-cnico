using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
  private readonly AppDbContext _db;

  public TransactionController(AppDbContext db)
  {
    _db = db;
  }

  [HttpPost("create")]
  public async Task<IActionResult> Create([FromBody] CreateTransactionRequest request)
  {
    var user = await _db.Users.FindAsync(request.UserId);
    if (user == null)
      return NotFound("Usuário não encontrado");

    var category = await _db.Categories.FindAsync(request.CategoryId);
    if (category == null)
      return NotFound("Categoria não encontrada");

    bool IsCategoryCompatible(TransactionType txType, CategoryType categoryPurpose) =>
        categoryPurpose == CategoryType.Both || (TransactionType)categoryPurpose == txType;

    if (!IsCategoryCompatible(request.Type, category.Purpose))
      return BadRequest(new
      {
        message = "A finalidade da categoria não corresponde ao tipo da transação",
        categoryPurpose = category.Purpose.ToString(),
        transactionType = request.Type.ToString()
      });

    if (user.Age < 18 && request.Type == TransactionType.Income)
      return BadRequest("Menores de idade só podem registrar despesas");

    var transaction = new Transaction
    {
      Description = request.Description,
      Value = request.Value,
      Type = request.Type,
      CategoryId = request.CategoryId,
      UserId = request.UserId
    };

    await _db.Transactions.AddAsync(transaction);
    await _db.SaveChangesAsync();

    return Ok(new
    {
      message = "Transação criada com sucesso",
      transaction
    });
  }

  [HttpGet("category/{categoryId}")]
  public async Task<IActionResult> GetByCategory(int categoryId)
  {
    var transactions = await _db.Transactions
        .Where(t => t.CategoryId == categoryId)
        .ToListAsync();

    return Ok(transactions);
  }

  [HttpGet("list")]
  public async Task<IActionResult> List()
  {
    var transactions = await _db.Transactions.ToListAsync();
    return Ok(transactions);
  }
}