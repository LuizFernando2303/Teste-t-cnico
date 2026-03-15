public class LoginRequest
{
  public required string Email { get; set; }
  public required string Password { get; set; }

  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
  }
}