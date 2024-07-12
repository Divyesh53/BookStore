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
using System.Runtime.ExceptionServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Service
{
    public class BooksService : IBooksService
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public BooksService(BookStoreDBContext bookStoreDBContext)
        {
            this._dbContext = bookStoreDBContext;
        }
        #endregion

        #region Public Methods
        public async Task<CommonAPIResponseModel> AddBook(BookRequestDTO BookRequestDTO, string userName)
        {
            //Return if any Id is not valid
            IsCategoryIdValid isCategoryIdValid = new IsCategoryIdValid(_dbContext);
            if (!(isCategoryIdValid.IsIDValid(BookRequestDTO.CategoryId)))
            {
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGCategory };
            }

            BookCodeGenerator bookCodeGenerator = new BookCodeGenerator(_dbContext);
            string BookCode = await bookCodeGenerator.GetBooksCode(BookRequestDTO.CategoryId);

            GetUserIdByName getUserIdByName = new GetUserIdByName(_dbContext);
            //Insert data into book table
            Book book = new Book()
            {
                BookTitle = BookRequestDTO.BookTitle,
                BookPrice = BookRequestDTO.BookPrice,
                InsertedDate = DateTime.UtcNow,
                InsertedBy = await getUserIdByName.GetUserId(userName),
                CategoryId = BookRequestDTO.CategoryId,
                BookCode = BookCode,
                IsDeleted = false,
            };
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Data = book.BookId, Message = ConstantValues.SuccessMSGAddBook };
        }

        public async Task<CommonAPIResponseModel> UpdateBook(int bookId, BookRequestDTO BookRequestDTO)
        {
            IsBookIdValid isBookIdValid = new IsBookIdValid(_dbContext);
            //check if it is valid bookId
            if (!isBookIdValid.IsIDValid(bookId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGBook };

            IsCategoryIdValid isCategoryIdValid = new IsCategoryIdValid(_dbContext);
            if (!(isCategoryIdValid.IsIDValid(BookRequestDTO.CategoryId)))
            {
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGCategory };
            }

            //Update data into book table
            Book book = await _dbContext.Books.Where(x => x.BookId == bookId).FirstOrDefaultAsync();

            book.BookTitle = BookRequestDTO.BookTitle;
            book.BookPrice = BookRequestDTO.BookPrice;
            book.UpdatedDate = DateTime.UtcNow;
            if (book.CategoryId != BookRequestDTO.CategoryId)
            {
                BookCodeGenerator bookCodeGenerator = new BookCodeGenerator(_dbContext);
                string BookCode = await bookCodeGenerator.GetBooksCode(BookRequestDTO.CategoryId);
                book.BookCode = BookCode;
            }

            await _dbContext.SaveChangesAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Data = book.BookId, Message = ConstantValues.SuccessMSGUpdatedBook };

        }

        public async Task<CommonAPIResponseModel> DeleteBook(int bookId)
        {
            IsBookIdValid isBookIdValid = new IsBookIdValid(_dbContext);
            //check if it is valid bookId
            if (!isBookIdValid.IsIDValid(bookId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGBook };

            //Delete data from book table
            Book book = await _dbContext.Books.Where(x => x.BookId == bookId).FirstOrDefaultAsync();
            book.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Message = ConstantValues.SuccessMSGDeleteBook };
        }

        public async Task<CommonAPIResponseModel> GetBook(int bookId)
        {
            IsBookIdValid isBookIdValid = new IsBookIdValid(_dbContext);
            //check if it is valid bookId
            if (!isBookIdValid.IsIDValid(bookId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGBook };

            Book book = await _dbContext.Books.Where(x => x.BookId == bookId).FirstOrDefaultAsync();

            BookResponseDTO bookResponseDTO = new BookResponseDTO()
            {
                BookTitle = book.BookTitle,
                BookCode = book.BookCode,
                BookPrice = book.BookPrice ?? 0,
                Category = (from books in _dbContext.Books
                            join category in _dbContext.Categories
                            on books.CategoryId equals category.CategoryId
                            where books.BookId == bookId
                            select category.CategoryName).FirstOrDefault() ?? ""
            };

            return new CommonAPIResponseModel() { StatusCode = 0, Data = bookResponseDTO };
        }

        public async Task<CommonAPIResponseModel> GetSoldBooks(string userName)
        {
            GetUserIdByName getUserIdByName = new GetUserIdByName(_dbContext);
            int userId = await getUserIdByName.GetUserId(userName);

            var soldBooksReport = await (from userTable in _dbContext.Users
                                         join booksTable in _dbContext.Books
                                         on userTable.UserId equals booksTable.InsertedBy
                                         join purchaseDetailTable in _dbContext.PurchaseDetails
                                         on booksTable.BookId equals purchaseDetailTable.BookId
                                         join purchaseTable in _dbContext.Purchases
                                         on purchaseDetailTable.PurchaseId equals purchaseTable.PurchaseId
                                         join customer in _dbContext.Users
                                         on purchaseTable.UserId equals customer.UserId
                                         where userTable.UserId == userId
                                         select new SoldBooksReponseModel
                                         {
                                             BookTitle = booksTable.BookTitle ?? "",
                                             Price = purchaseDetailTable.BookPurchasedPrice ?? 0,
                                             Quantity = purchaseDetailTable.Quantity ?? 0,
                                             UserName = customer.UserName ?? "",
                                         }).ToListAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Data = soldBooksReport };
        }
        #endregion
    }
}