﻿using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? PromotionId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Promotion Promotion { get; set; }
}
