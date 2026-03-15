
public class RegisterRequest
{
  public required string Email { get; set; }
  public required string Name { get; set; }
  public int Age { get; set; } = 0;
  public required string Password { get; set; }

  public bool IsValid()
  {
    if (string.IsNullOrWhiteSpace(Email))
      return false;
    if (string.IsNullOrWhiteSpace(Name))
      return false;
    if (string.IsNullOrWhiteSpace(Password))
      return false;

    if (Age <= 0)
      return false;

    return true;
  }
}