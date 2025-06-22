using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int? DiscountId { get; set; }

    public decimal Amount { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Discount? Discount { get; set; }

    public virtual Product Product { get; set; } = null!;
}
