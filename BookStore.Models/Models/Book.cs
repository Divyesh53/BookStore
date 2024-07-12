using System;
using System.Collections.Generic;

namespace BookStore.Models.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string? BookTitle { get; set; }

    public decimal? BookPrice { get; set; }

    public int? CategoryId { get; set; }

    public string? BookCode { get; set; }

    public DateTime? InsertedDate { get; set; }

    public int? InsertedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? InsertedByNavigation { get; set; }

    public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();

    public virtual User? UpdatedByNavigation { get; set; }
}
