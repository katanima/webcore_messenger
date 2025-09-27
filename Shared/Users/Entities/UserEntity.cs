using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using webcore_backend.Features.Users.Dtos;

namespace webcore_backend.Features.Users.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Username { get; set; }
    
    //TODO: implement 
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; }
    
    [Required]
    [Column(TypeName = "text")]
    public string PasswordHash { get; set; }
    
    [Phone]
    [MaxLength(20)]
    [Column(TypeName = "varchar(20)")]
    public string? PhoneNumber { get; set; }
    
    //TODO: implement join date
    //TODO: implement guild list
}