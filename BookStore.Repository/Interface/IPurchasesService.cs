using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Interface
{
    public interface IPurchasesService
    {
        Task<CommonAPIResponseModel> AddPurchase(PurchaseRequestDTO purchase , string userName);
        Task<CommonAPIResponseModel> UpdatePurchase(int purchaseId, PurchaseRequestDTO purchase);
        Task<CommonAPIResponseModel> DeletePurchase(int purchaseId);
        Task<CommonAPIResponseModel> GetPurchase(int purchaseId);
        Task<CommonAPIResponseModel> GetPurchasedBooks(string userName);

    }
}
