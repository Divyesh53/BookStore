using BookStore.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.RequestModels
{
    public class PurchaseResponseDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<PurchaseDetailsResponseDTO> PurchaseDetails { get; set; }
    }
    public class PurchaseDetailsResponseDTO
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal BookPurchasedPrice { get; set; }
    }
}
