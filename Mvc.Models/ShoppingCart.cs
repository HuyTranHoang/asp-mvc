using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Mvc.Models;

public class ShoppingCart
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }

    [DisplayName("Quantity: ")]
    [Range(1, 999, ErrorMessage = "Please enter a value beetween 1 and 999")]
    public int Quantity { get; set; }

    public string IdentityUserId { get; set; }
    [ForeignKey("IdentityUserId")]
    [ValidateNever]
    public IdentityUser IdentityUser { get; set; }

    // [NotMapped]
    // public double Price { get; set; }
}