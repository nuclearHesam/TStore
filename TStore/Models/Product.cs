using System.ComponentModel.DataAnnotations;

namespace TStore;

public class Product
{
    [Key]

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }



    public string CategoryId {  get; set; }
    public Category Category { get; set; }
}