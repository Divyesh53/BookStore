using BookStore.Models.Models;
using BookStore.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Service
{
    public class RolesService : IRolesService
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public RolesService(BookStoreDBContext bookStoreDBContext)
        {
            this._dbContext = bookStoreDBContext;
        }
        #endregion

        #region Public Methods

        public async Task<List<Role>> GetRoles()
        {
            var roles = new List<Role>();
            roles = await _dbContext.Roles.ToListAsync();
            return roles;
        }
        #endregion
    }
}
