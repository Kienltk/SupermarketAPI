using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int OrderId { get; set; }

    public decimal BillAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual Order Order { get; set; } = null!;
}
