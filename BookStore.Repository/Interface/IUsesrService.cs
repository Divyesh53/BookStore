using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Interface
{
    public interface IUsesrService
    {
        Task<CommonAPIResponseModel> AddUser(UserRequestDTO user);
        Task<CommonAPIResponseModel> UpdateUser(int userId, UserRequestDTO user);
        Task<CommonAPIResponseModel> DeleteUser(int userId);
        Task<CommonAPIResponseModel> GetUser(int userId);
    }
}
