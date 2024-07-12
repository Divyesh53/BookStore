using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Interface
{
    public interface IAuthService
    {
        Task<CommonAPIResponseModel> Login(UserLoginRequestModel model);
    }
}
