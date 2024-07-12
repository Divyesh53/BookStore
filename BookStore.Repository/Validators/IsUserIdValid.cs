using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Validators
{
    public class IsUserIdValid : BaseCheckIdAvailability
    {
        public IsUserIdValid(BookStoreDBContext context) : base(context)
        {
        }

        public override bool IsIDValid(int Id)
        {
            var user =  Context.Users.Where(x => x.UserId == Id && x.IsDeleted == false).FirstOrDefault();
            if (user == null)
                return false;
            else
                return true;
        }
    }
}
