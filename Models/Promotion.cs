using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionType { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal? DiscountPercent { get; set; }

    public decimal? DiscountAmount { get; set; }

    public int? GiftProductId { get; set; }

    public decimal? MinOrderValue { get; set; }

    public int? MinOrderQuantity { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual Product? GiftProduct { get; set; }
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}
