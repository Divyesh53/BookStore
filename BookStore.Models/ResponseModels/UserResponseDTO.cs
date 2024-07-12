using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.ResponseModels
{
    public class UserResponseDTO
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
