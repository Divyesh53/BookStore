using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Validators
{
    public class IsPurchaseIdValid : BaseCheckIdAvailability
    {
        public IsPurchaseIdValid(BookStoreDBContext context) : base(context)
        {
        }

        public override bool IsIDValid(int Id)
        {
            var user = Context.Purchases.Where(x => x.PurchaseId == Id).FirstOrDefault();
            if (user == null)
                return false;
            else
                return true;
        }
    }
}