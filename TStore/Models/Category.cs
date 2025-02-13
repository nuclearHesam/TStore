﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TStore;
public class Category
{
    [Key]
    public string CategoryId { get; set; }  = default!;

    [Required]
    public string Name { get; set; } = default!;

    [NotMapped]
    public IFormFile FormImage { get; set; }

    public byte[]? Image { get; set; } = default!;

    public string? Brands { get; set; } =default!; // split by ','

    public bool ShowinSlider { get; set; } = default!;
}