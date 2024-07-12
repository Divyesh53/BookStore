using BookStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Helpers
{
    public class GetUserIdByName
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public GetUserIdByName(BookStoreDBContext dBContext)
        {
            this._dbContext = dBContext;
        }
        #endregion

        #region Public Mehods
        public async Task<int> GetUserId(string userName)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            return user.UserId;
        }
        #endregion

    }
}
