public class EditRequest
{
  public required string Email { get; set; }
  public required string Password { get; set; }
  public UserData EditingData { get; set; } = new();

  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(Email) &&
           !string.IsNullOrWhiteSpace(Password);
  }

  public bool HasValidEditingData()
  {
    return EditingData != null && EditingData.IsValid();
  }
}

public class UserData
{
  public string? Name { get; set; }
  public string? Password { get; set; }
  public int Age { get; set; }

  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(Name) ||
           !string.IsNullOrWhiteSpace(Password) ||
           Age > 0;
  }
}