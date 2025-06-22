using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class BillDetail
{
    public int BillDetailId { get; set; }

    public int BillId { get; set; }

    public string ItemType { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public int? PromotionId { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Promotion? Promotion { get; set; }
}
