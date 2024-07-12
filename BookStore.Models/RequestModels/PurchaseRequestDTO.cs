using BookStore.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.RequestModels
{
    public class PurchaseRequestDTO
    {
        /*public int UserId { get; set; }*/ //Not needed right now
        public List<PurchaseDetails> PurchaseDetails { get; set; }
    }
    public class PurchaseDetails
    {
        [Required(ErrorMessage = "Please select books!")]
        public int BookId { get; set; }
        [Required(ErrorMessage = "Please enter the quantity.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Please enter the price!")]
        public decimal BookPurchasedPrice { get; set; }
    }
}
