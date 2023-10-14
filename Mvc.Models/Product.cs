using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mvc.Models;

public class Product
{
    [Key] public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public double Price { get; set; }

    public string Description { get; set; } = "";

    [DisplayName("Image")]
    public string ImageUrl { get; set; } = "default.jpg";

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Select a category")]
    [DisplayName("Category")]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [ValidateNever]
    public Category Category { get; set; }
}