using System;
using System.Collections.Generic;

namespace BookStore.Models.Models;

public partial class PurchaseDetail
{
    public int PurchaseDetailId { get; set; }

    public int? PurchaseId { get; set; }

    public int? Quantity { get; set; }

    public int? BookId { get; set; }

    public decimal? BookPurchasedPrice { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? InsertedDate { get; set; }

    public int? InsertedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? InsertedByNavigation { get; set; }

    public virtual Purchase? Purchase { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
