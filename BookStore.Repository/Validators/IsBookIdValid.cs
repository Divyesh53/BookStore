using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Validators
{
    public class IsBookIdValid : BaseCheckIdAvailability
    {
        public IsBookIdValid(BookStoreDBContext context) : base(context)
        {
        }

        public override bool IsIDValid(int Id)
        {
            var user =  Context.Books.Where(x => x.BookId == Id && x.IsDeleted == false).FirstOrDefault();
            if (user == null)
                return false;
            else
                return true;
        }
    }
}
