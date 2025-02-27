using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TStore;

public class Product
{
    [Key]
    public string ProductId { get; set; }

    [Required]
    public string Image { get; set; }

    public string? Images { get; set; } // split ','

    [NotMapped]
    public List<string> ImageList
    {
        get => string.IsNullOrEmpty(Images) ? new List<string>() : new List<string>(Images.Split(','));
        set => Images = string.Join(",", value);
    }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Brand { get; set; }

    public string? Description { get; set; }


    public string CategoryId { get; set; }
    public Category Category { get; set; }
}