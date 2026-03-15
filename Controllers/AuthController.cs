using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
  private readonly AppDbContext _db;

  public AuthController(AppDbContext db)
  {
    _db = db;
  }

  [HttpGet("list")]
  public async Task<ActionResult> List()
  {
    var users = await _db.Users
        .Select(u => new { u.Id, u.Name, u.Age, u.Email })
        .ToListAsync();

    return Ok(new { message = "Usuários listados com sucesso", users });
  }

  [HttpPost("delete")]
  public async Task<ActionResult> Delete([FromBody] DeleteRequest request)
  {
    if (!ModelState.IsValid || !request.IsValid())
      return BadRequest(new { message = "Dados inválidos" });

    var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null)
      return NotFound(new { message = "Usuário não encontrado" });

    if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
      return Unauthorized(new { message = "Senha incorreta" });

    var transactions = _db.Transactions.Where(t => t.UserId == user.Id);
    _db.Transactions.RemoveRange(transactions);

    var categories = _db.Categories.Where(c => c.UserId == user.Id);
    _db.Categories.RemoveRange(categories);

    _db.Users.Remove(user);

    await _db.SaveChangesAsync();

    return Ok(new { message = "Usuário deletado com sucesso" });
  }

  [HttpPost("edit")]
  public async Task<ActionResult> Edit([FromBody] EditRequest request)
  {
    if (!ModelState.IsValid || !request.IsValid() || !request.HasValidEditingData())
      return BadRequest(new { message = "Dados inválidos" });

    var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
    if (user == null)
      return NotFound(new { message = "Usuário não existe" });

    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
      return Unauthorized(new { message = "Senha incorreta" });

    if (!string.IsNullOrWhiteSpace(request.EditingData.Name))
      user.Name = request.EditingData.Name;

    if (!string.IsNullOrWhiteSpace(request.EditingData.Password))
      user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.EditingData.Password);

    if (request.EditingData.Age > 0)
      user.Age = request.EditingData.Age;

    await _db.SaveChangesAsync();

    return Ok(new
    {
      message = "Usuário modificado com sucesso",
      user = new { user.Id, user.Name, user.Age, user.Email }
    });
  }

  [HttpPost("register")]
  public async Task<ActionResult> Register([FromBody] RegisterRequest request)
  {
    if (!ModelState.IsValid || !request.IsValid())
      return BadRequest(new { message = "Dados inválidos" });

    var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
    if (exists)
      return Conflict(new { message = "Email já cadastrado" });

    var user = new User
    {
      Email = request.Email,
      Name = request.Name,
      Age = request.Age,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
    };

    _db.Users.Add(user);
    await _db.SaveChangesAsync();

    return Ok(new
    {
      message = "Usuário registrado com sucesso",
      user = new { user.Id, user.Name, user.Age, user.Email }
    });
  }

  [HttpPost("login")]
  public async Task<ActionResult> Login([FromBody] LoginRequest request)
  {
    if (!ModelState.IsValid || !request.IsValid())
      return BadRequest(new { message = "Email e senha são obrigatórios" });

    var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
      return Unauthorized(new { message = "Email ou senha inválidos" });

    return Ok(new
    {
      message = "Login realizado com sucesso",
      user = new { user.Id, user.Name, user.Age, user.Email }
    });
  }
}