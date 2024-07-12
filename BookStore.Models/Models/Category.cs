using System;
using System.Collections.Generic;

namespace BookStore.Models.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public DateTime? InsertedDate { get; set; }

    public int? InsertedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
