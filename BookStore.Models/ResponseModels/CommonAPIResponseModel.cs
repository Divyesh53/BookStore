using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ResponseModels
{
    public class CommonAPIResponseModel
    {
        public int StatusCode { get; set; } // 0= Success, 1= Error, 2 = ExistRecord
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
