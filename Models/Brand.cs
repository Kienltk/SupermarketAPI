using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } 

    public string Slug { get; set; } 

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
