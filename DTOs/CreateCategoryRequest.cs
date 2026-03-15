public class CreateCategoryRequest
{
  public int UserId { get; set; }

  public required string Description { get; set; }

  public TransactionType Purpose { get; set; }

  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(Description) &&
           UserId > 0;
  }
}