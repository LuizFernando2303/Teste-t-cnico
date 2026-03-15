using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Transaction
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [MaxLength(400)]
  [Required]
  public string Description { get; set; }

  [Range(0.01, double.MaxValue)]
  [Column(TypeName = "decimal(10,2)")]
  public decimal Value { get; set; }

  public TransactionType Type { get; set; }

  public int CategoryId { get; set; }

  public int UserId { get; set; }
}