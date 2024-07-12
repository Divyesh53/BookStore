using BookStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Validators
{
    public abstract class BaseCheckIdAvailability
    {
        #region Private Variable
        BookStoreDBContext _context;
        #endregion

        #region Public properties
        public BookStoreDBContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }
        #endregion

        #region Constructor
        public BaseCheckIdAvailability(BookStoreDBContext context)
        {
            this.Context = context;
        }
        #endregion

        #region Public Methods
        public abstract bool IsIDValid(int id);
        #endregion
    }
}