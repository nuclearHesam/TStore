using System.ComponentModel.DataAnnotations;

namespace TStore.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string Description { get; set; }


    public string CategoryId {  get; set; }
    public Category Category { get; set; }
}