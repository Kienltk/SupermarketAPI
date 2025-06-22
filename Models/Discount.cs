using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public int ProductId { get; set; }

    public int PromotionId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; } = null!;
}
