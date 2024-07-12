using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Interface
{
    public interface IBooksService
    {
        Task<CommonAPIResponseModel> AddBook(BookRequestDTO book, string userName);
        Task<CommonAPIResponseModel> UpdateBook(int bookId, BookRequestDTO book);
        Task<CommonAPIResponseModel> DeleteBook(int bookId);
        Task<CommonAPIResponseModel> GetBook(int bookId);
        Task<CommonAPIResponseModel> GetSoldBooks(string userName);
    }
}
