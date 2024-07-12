using System;
using System.Collections.Generic;

namespace BookStore.Models.Models;

public partial class Purchase
{
    public int PurchaseId { get; set; }

    public int? UserId { get; set; }

    public int? TotalQuantity { get; set; }

    public decimal? NetAmount { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual User? User { get; set; }
}
