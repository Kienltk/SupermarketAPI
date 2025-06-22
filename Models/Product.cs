using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string Slug { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal? UnitCost { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? BrandId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
