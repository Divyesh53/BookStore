using BookStore.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Models.Helpers
{
    public class BookCodeGenerator
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public BookCodeGenerator(BookStoreDBContext dBContext)
        {
            this._dbContext = dBContext;
        }
        #endregion

        #region Public Mehods
        public async Task<string> GetBooksCode(int categoryId)
        {
            string bookCode = string.Empty;

            var category = await _dbContext.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
            string firstThreeChars = category.CategoryName.Substring(0, 3).ToUpper();
            int totalCount = 0;

            var numberOfBooksWithThisCategory = await _dbContext.Books.Where(x => x.CategoryId == categoryId).ToListAsync();
            if (!(numberOfBooksWithThisCategory == null) || !(numberOfBooksWithThisCategory.Count == 0))
                totalCount = numberOfBooksWithThisCategory.Count() + 1;
            else
                totalCount = 1;

            bookCode = $"{firstThreeChars}{totalCount}";
            return bookCode;
        }
        #endregion

    }
}
