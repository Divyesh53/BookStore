using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.RequestModels
{
    public class BookResponseDTO
    {
        public string BookTitle { get; set; }
        public decimal BookPrice { get; set; }
        public string BookCode { get; set; }
        public string Category { get; set; }
    }
}
