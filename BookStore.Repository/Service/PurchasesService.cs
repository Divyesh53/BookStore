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
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Service
{
    public class PurchasesService : IPurchasesService
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public PurchasesService(BookStoreDBContext bookStoreDBContext)
        {
            this._dbContext = bookStoreDBContext;
        }
        #endregion

        #region Public Methods
        public async Task<CommonAPIResponseModel> AddPurchase(PurchaseRequestDTO PurchaseRequestDTO, string userName)
        {
            IsBookIdValid isBookIdValid = new IsBookIdValid(_dbContext);
            //check if all book ids are valid
            foreach (var item in PurchaseRequestDTO.PurchaseDetails)
            {
                if (!isBookIdValid.IsIDValid(item.BookId))
                {
                    return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGCategory };
                }
            }
            GetUserIdByName getUserIdByName = new GetUserIdByName(_dbContext);


            //Insert data into Purchase table table
            Purchase purchase = new Purchase()
            {
                UserId = await getUserIdByName.GetUserId(userName),
                TotalQuantity = PurchaseRequestDTO.PurchaseDetails.Sum(x => x.Quantity),
                NetAmount = PurchaseRequestDTO.PurchaseDetails.Sum(x => (x.BookPurchasedPrice * x.Quantity)),
                PurchaseDate = DateTime.UtcNow,
            };
            await _dbContext.Purchases.AddAsync(purchase);
            await _dbContext.SaveChangesAsync();


            //Insert data in PurchaseDetails table
            List<PurchaseDetail> purchaseDetails = new List<PurchaseDetail>();

            PurchaseRequestDTO.PurchaseDetails.ForEach(eachPurchase =>
            {
                PurchaseDetail purchaseDetail = new PurchaseDetail()
                {
                    PurchaseId = purchase.PurchaseId,
                    Quantity = eachPurchase.Quantity,
                    BookId = eachPurchase.BookId,
                    BookPurchasedPrice = eachPurchase.BookPurchasedPrice,
                    TotalAmount = eachPurchase.Quantity * eachPurchase.BookPurchasedPrice,
                    InsertedDate = DateTime.UtcNow,
                };
                purchaseDetails.Add(purchaseDetail);
            });
            await _dbContext.AddRangeAsync(purchaseDetails);
            await _dbContext.SaveChangesAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Data = purchase.PurchaseId, Message = ConstantValues.SuccessMSGAddPurchase };

        }

        public async Task<CommonAPIResponseModel> UpdatePurchase(int purchaseId, PurchaseRequestDTO PurchaseRequestDTO)
        {
            IsPurchaseIdValid isPurchaseIdValid = new IsPurchaseIdValid(_dbContext);

            //check if it is valid pruchaseId
            if (!isPurchaseIdValid.IsIDValid(purchaseId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGPurchase };


            //verify that all purchased books ids are valid
            bool isAllBookIdsValid = true;
            IsBookIdValid isBookIdValid = new IsBookIdValid(_dbContext);
            PurchaseRequestDTO.PurchaseDetails.ForEach(eachPurchase =>
            {
                if (!isBookIdValid.IsIDValid(eachPurchase.BookId))
                    isAllBookIdsValid = false;
            });
            if (!isAllBookIdsValid)
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGBook };


            //Insert data in purchase table
            Purchase purchaseObject = new Purchase()
            {
                TotalQuantity = PurchaseRequestDTO.PurchaseDetails.Sum(x => x.Quantity),
                NetAmount = PurchaseRequestDTO.PurchaseDetails.Sum(x => (x.BookPurchasedPrice * x.Quantity)),
            };
            await _dbContext.SaveChangesAsync(); //Save changes 


            //Remove purchaseList which are not inserted by user while update
            var purchaseList = _dbContext.PurchaseDetails.Where(x => x.PurchaseId == purchaseId).ToList();
            var booksRemovedByUser = purchaseList.Where(oldList => PurchaseRequestDTO.PurchaseDetails.All(newList => newList.BookId != oldList.BookId)).ToList();
            _dbContext.PurchaseDetails.RemoveRange(booksRemovedByUser);
            await _dbContext.SaveChangesAsync();


            //Insert data in PurchaseDetail table
            List<PurchaseDetail> PurchaseDetail = new List<PurchaseDetail>();
            PurchaseRequestDTO.PurchaseDetails.ForEach(async purchase =>
            {
                var existingPurchase = await _dbContext.PurchaseDetails.Where(x => x.PurchaseId == purchaseId && x.BookId != purchase.BookId).FirstOrDefaultAsync();

                if (existingPurchase != null) //If book already purchased
                {
                    existingPurchase.BookPurchasedPrice = purchase.BookPurchasedPrice;
                    existingPurchase.Quantity = purchase.Quantity;
                    existingPurchase.TotalAmount = (purchase.Quantity * purchase.BookPurchasedPrice);
                    existingPurchase.UpdatedOn = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                }
                else //To insert new books
                {
                    PurchaseDetail purchaseDetail = new PurchaseDetail()
                    {
                        PurchaseId = purchaseObject.PurchaseId,
                        Quantity = purchase.Quantity,
                        BookId = purchase.BookId,
                        BookPurchasedPrice = purchase.BookPurchasedPrice,
                        TotalAmount = purchase.Quantity * purchase.BookPurchasedPrice,
                        InsertedDate = DateTime.UtcNow,
                    };
                    PurchaseDetail.Add(purchaseDetail);
                }
            });

            await _dbContext.AddRangeAsync(PurchaseDetail);
            await _dbContext.SaveChangesAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Message = ConstantValues.SuccessMSGUpdatedPurchase };
        }

        public async Task<CommonAPIResponseModel> DeletePurchase(int purchaseId)
        {
            IsPurchaseIdValid isPurchaseIdValid = new IsPurchaseIdValid(_dbContext);

            //check if it is valid pruchaseId
            if (!isPurchaseIdValid.IsIDValid(purchaseId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGPurchase };

            var purchase = _dbContext.Purchases.Where(x => x.PurchaseId.Equals(purchaseId)).FirstOrDefault();
            var purchaseDetails = _dbContext.PurchaseDetails.Where(x => x.PurchaseId == purchaseId).ToList();
            _dbContext.PurchaseDetails.RemoveRange(purchaseDetails);
            _dbContext.Purchases.Remove(purchase);
            await _dbContext.SaveChangesAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Message = ConstantValues.SuccessMSGDeletePurchase };
        }

        public async Task<CommonAPIResponseModel> GetPurchase(int purchaseId)
        {
            IsPurchaseIdValid isPurchaseIdValid = new IsPurchaseIdValid(_dbContext);

            //check if it is valid pruchaseId
            if (!isPurchaseIdValid.IsIDValid(purchaseId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGPurchase };


            var purchase = _dbContext.Purchases.Where(x => x.PurchaseId.Equals(purchaseId)).FirstOrDefault();

            PurchaseResponseDTO purchaseResponseDTO = new PurchaseResponseDTO()
            {
                UserId = purchase.UserId ?? 0,
                UserName = (from purchases in _dbContext.Purchases
                            join user in _dbContext.Users
                            on purchases.UserId equals user.UserId
                            where purchases.PurchaseId == purchase.PurchaseId
                            select user.UserName).First(),
            };

            purchaseResponseDTO.PurchaseDetails = await (from purchases in _dbContext.PurchaseDetails
                                                         join book in _dbContext.Books
                                                         on purchases.BookId equals book.BookId
                                                         where purchases.PurchaseId == purchase.PurchaseId
                                                         select new PurchaseDetailsResponseDTO
                                                         {
                                                             BookId = book.BookId,
                                                             BookName = book.BookTitle,
                                                             BookPurchasedPrice = purchases.BookPurchasedPrice ?? 0,
                                                             Quantity = purchases.Quantity ?? 0,
                                                         }
                                                   ).ToListAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Data = purchaseResponseDTO };
        }

        public async Task<CommonAPIResponseModel> GetPurchasedBooks(string userName)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);

            PurchaseResponseDTO purchaseResponseDTO = new PurchaseResponseDTO();

            purchaseResponseDTO.UserId = user.UserId;
            purchaseResponseDTO.UserName = userName;
            purchaseResponseDTO.PurchaseDetails = (from purchases in _dbContext.Purchases
                                                   join purchasesDetail in _dbContext.PurchaseDetails
                                                   on purchases.PurchaseId equals purchasesDetail.PurchaseId
                                                   join books in _dbContext.Books
                                                   on purchasesDetail.BookId equals books.BookId
                                                   where purchases.UserId == user.UserId
                                                   select new PurchaseDetailsResponseDTO
                                                   {
                                                       BookId = purchasesDetail.BookId ?? 0,
                                                       BookName = books.BookTitle,
                                                       BookPurchasedPrice = purchasesDetail.BookPurchasedPrice ?? 0,
                                                       Quantity = purchasesDetail.Quantity ?? 0,
                                                   }
                                                   ).ToList();

            return new CommonAPIResponseModel() { StatusCode = 0, Data = purchaseResponseDTO };
        }

        #endregion
    }
}
