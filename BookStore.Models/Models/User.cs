using System;
using System.Collections.Generic;

namespace BookStore.Models.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Book> BookInsertedByNavigations { get; set; } = new List<Book>();

    public virtual ICollection<Book> BookUpdatedByNavigations { get; set; } = new List<Book>();

    public virtual ICollection<PurchaseDetail> PurchaseDetailInsertedByNavigations { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<PurchaseDetail> PurchaseDetailUpdatedByNavigations { get; set; } = new List<PurchaseDetail>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual Role? Role { get; set; }
}
