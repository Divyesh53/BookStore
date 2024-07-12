using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.RequestModels
{
    public class BookRequestDTO
    {
        [Required(ErrorMessage = "Book Title is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Book Title should have 3 to 100 characters.")]
        public string BookTitle { get; set; }
        [Required(ErrorMessage = "Book price is required!")]
        [Range(1, 1000000, ErrorMessage = "Price should be betweeb 1 to 1000000.")]
        public decimal BookPrice { get; set; } = 0;
        [Required]
        public int CategoryId { get; set; }
    }
}