using System;
using System.Collections.Generic;

namespace BookStore.Models.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleDescription { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
