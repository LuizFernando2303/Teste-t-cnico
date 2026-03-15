using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
  private readonly AppDbContext _db;

  public UserController(AppDbContext db) => _db = db;

  [HttpGet("totals")]
  public async Task<IActionResult> GetUserTotals()
  {
    var users = await _db.Users
        .Select(u => new
        {
          u.Id,
          u.Name,
          u.Email,
          u.Age,

          incomes = _db.Transactions
                .Where(t => t.UserId == u.Id && t.Type == TransactionType.Income)
                .Sum(t => (decimal?)t.Value) ?? 0,

          expenses = _db.Transactions
                .Where(t => t.UserId == u.Id && t.Type == TransactionType.Expense)
                .Sum(t => (decimal?)t.Value) ?? 0,
        })
        .ToListAsync();

    var result = users.Select(u => new
    {
      u.Id,
      u.Name,
      u.Email,
      u.Age,
      incomes = u.incomes,
      expenses = u.expenses,
      balance = u.incomes - u.expenses
    }).ToList();

    var totalIncomes = result.Sum(u => u.incomes);
    var totalExpenses = result.Sum(u => u.expenses);
    var totalBalances = result.Sum(u => u.balance);

    return Ok(new
    {
      users = result,
      summary = new
      {
        incomes = totalIncomes,
        expenses = totalExpenses,
        balance = totalIncomes - totalExpenses
      }
    });
  }
}