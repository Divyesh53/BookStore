using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ResponseModels
{
    public class SoldBooksReponseModel
    {
        public string UserName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string BookTitle { get; set; }
    }
}
