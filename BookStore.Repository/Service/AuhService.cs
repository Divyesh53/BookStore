using BookStore.Models.Helpers;
using BookStore.Models.Models;
using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Service
{
    public class AuhService : IAuthService
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public AuhService(BookStoreDBContext bookStoreDBContext, IConfiguration configuration)
        {
            this._dbContext = bookStoreDBContext;
            this._configuration = configuration;

        }

        public async Task<CommonAPIResponseModel> Login(UserLoginRequestModel model)
        {
            var user = await _dbContext.Users.Where(x => x.UserName == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
            if (user == null)
            {
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.CheckCredentialsMSGUser };
            }
            Role userRole = _dbContext.Roles.Where(x => x.RoleId == user.RoleId).FirstOrDefault();
            string token = await GenerateTokenForUser(model, user.UserId, userRole.RoleName);
            UserLoginResponseModel UserLoginResponseModel = new UserLoginResponseModel()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                token = token,
            };
            return new CommonAPIResponseModel() { StatusCode = 0, Data = UserLoginResponseModel, Message = ConstantValues.SuccessMSGLoginUser };
        }
        #endregion

        #region Public Methods


        #endregion

        private async Task<string> GenerateTokenForUser(UserLoginRequestModel userDet, int userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);
            var sessionTimeOut = _configuration.GetSection("SessionTimeOut");
            double sessionTimeOutMinutes = Convert.ToDouble(string.IsNullOrEmpty(sessionTimeOut.Value) ? 60 : sessionTimeOut.Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name , userDet.Username),
                    new Claim(ClaimTypes.UserData, Convert.ToString(userId)),
                    new Claim(ClaimTypes.Role , role)
                }),
                IssuedAt = DateTime.UtcNow,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(sessionTimeOutMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            try
            {
                var tokenDesc = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(tokenDesc);

                return token;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
