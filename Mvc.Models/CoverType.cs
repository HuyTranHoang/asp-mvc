using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mvc.Models;

public class CoverType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [DisplayName("Cover type")]
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}