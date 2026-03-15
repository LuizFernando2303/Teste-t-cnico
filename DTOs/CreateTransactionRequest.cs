public class CreateTransactionRequest
{
  public required string Description { get; set; }

  public decimal Value { get; set; }

  public TransactionType Type { get; set; }

  public int CategoryId { get; set; }

  public int UserId { get; set; }

  public bool IsValid()
  {
    return !string.IsNullOrWhiteSpace(Description) && Value > 0;
  }
}