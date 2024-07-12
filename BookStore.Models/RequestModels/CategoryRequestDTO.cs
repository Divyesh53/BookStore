using BookStore.Models.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.RequestModels
{
    public class CategoryRequestDTO
    {
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Category name should have 3 to 50 characters.")]
        public string CategoryName { get; set; }
    }
}
