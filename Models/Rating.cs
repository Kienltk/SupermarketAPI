using System;
using System.Collections.Generic;

namespace SupermarketAPI.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int Rating1 { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
