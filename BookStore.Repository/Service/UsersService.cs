using BookStore.Models.Helpers;
using BookStore.Models.Models;
using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using BookStore.Repository.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Repository.Service
{
    public class UsersService : IUsesrService
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public UsersService(BookStoreDBContext bookStoreDBContext)
        {
            this._dbContext = bookStoreDBContext;
        }
        #endregion

        #region Public Methods

        public async Task<CommonAPIResponseModel> AddUser(UserRequestDTO user)
        {
            var userData = _dbContext.Users.Where(x => x.UserName == user.UserName).FirstOrDefault();
            if (userData != null)
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.ErrorMSGAddUser };

            User user1 = new User()
            {
                UserName = user.UserName,
                Password = user.Password,
                RoleId = user.RoleId,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false,
            };
            await _dbContext.Users.AddAsync(user1);
            await _dbContext.SaveChangesAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Data = user1.UserId, Message = ConstantValues.SuccessMSGAddUser };
        }

        public async Task<CommonAPIResponseModel> DeleteUser(int userId)
        {
            // Validate Id
            IsUserIdValid isUserIdValid = new IsUserIdValid(_dbContext);
            if (!isUserIdValid.IsIDValid(userId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGUser };

            var user = _dbContext.Users.Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefault();
            user.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Message = ConstantValues.SuccessMSGDeleteUser };
        }

        public async Task<CommonAPIResponseModel> GetUser(int userId)
        {
            // Validate Id
            IsUserIdValid isUserIdValid = new IsUserIdValid(_dbContext);
            if (!isUserIdValid.IsIDValid(userId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGUser };

            var userObject = await (from user in _dbContext.Users
                                    join role in _dbContext.Roles
                                    on user.RoleId equals role.RoleId
                                    where user.UserId == userId
                                    select new UserResponseDTO
                                    {
                                        CreatedDate = user.CreatedDate ?? DateTime.Now,
                                        UserName = user.UserName,
                                        RoleName = role.RoleName
                                    }).FirstOrDefaultAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Data = userObject };
        }

        public async Task<CommonAPIResponseModel> UpdateUser(int userId, UserRequestDTO user)
        {
            // Validate Id
            IsUserIdValid isUserIdValid = new IsUserIdValid(_dbContext);
            if (!isUserIdValid.IsIDValid(userId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGUser };

            var user1 = await _dbContext.Users.Where(x => x.UserId == userId && x.IsDeleted == false).FirstOrDefaultAsync();
            user1.UserName = user.UserName;
            user1.Password = user.Password;
            user1.RoleId = user.RoleId;

            await _dbContext.Users.AddAsync(user1);
            await _dbContext.SaveChangesAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Data = user1.UserId, Message = ConstantValues.SuccessMSGUpdateUser };
        }
        #endregion
    }
}
