using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  [Required]
  [StringLength(200, MinimumLength = 2)]
  public string Name { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }

  [Range(0, 120)]
  public int Age { get; set; }

  [Required]
  public string PasswordHash { get; set; }
}