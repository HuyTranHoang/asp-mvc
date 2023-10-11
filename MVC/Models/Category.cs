using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class Category
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "Name can't be empty!")]
    public string Name { get; set; } = "";

    [DisplayName("Display order")]
    [Required(ErrorMessage = "Description can't be empty!")]
    [Range(1, int.MaxValue, ErrorMessage = "{0} need to be greater than {1}")]
    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}