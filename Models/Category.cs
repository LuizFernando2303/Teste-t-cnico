using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Category
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  public int UserId { get; set; }
  public User User { get; set; }

  [Required]
  [StringLength(200)]
  public string Description { get; set; }

  public CategoryType Purpose { get; set; }
}