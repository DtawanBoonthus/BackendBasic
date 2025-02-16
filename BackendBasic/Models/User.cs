using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendBasic.Models;

[Table("user_account")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int ID { get; set; }

    [Column("user_name")]
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;

    [Column("password")]
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;
}