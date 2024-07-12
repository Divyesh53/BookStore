using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ResponseModels
{
    public class UserLoginResponseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string token { get; set; }
    }
}
