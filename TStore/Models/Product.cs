using System.ComponentModel.DataAnnotations;

namespace TStore;

public class Product
{
    [Key]
    public string ProductId { get; set; }

    [Required]
    public string Image { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Brand { get; set; }

    public string? Description { get; set; }


    public string CategoryId {  get; set; } 
    public Category Category { get; set; } 
}