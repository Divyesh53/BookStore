using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Validators
{
    public class IsCategoryIdValid : BaseCheckIdAvailability
    {
        public IsCategoryIdValid(BookStoreDBContext context) : base(context)
        {
        }

        public override bool IsIDValid(int Id)
        {
            var user =  Context.Categories.Where(x => x.CategoryId== Id && x.IsDeleted == false).FirstOrDefault();
            if (user == null)
                return false;
            else
                return true;
        }
    }
}
