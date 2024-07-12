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
    public class CategoriesService : ICategoriesService
    {
        #region Private Fields
        private BookStoreDBContext _dbContext;
        #endregion

        #region Constructor
        public CategoriesService(BookStoreDBContext bookStoreDBContext)
        {
            this._dbContext = bookStoreDBContext;
        }

        public async Task<CommonAPIResponseModel> AddCategory(CategoryRequestDTO category)
        {

            Category Category = new Category()
            {
                CategoryName = category.CategoryName,
                InsertedDate = DateTime.UtcNow,
                IsDeleted = false,
            };
            await _dbContext.Categories.AddAsync(Category);
            await _dbContext.SaveChangesAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Data = Category.CategoryId, Message = ConstantValues.SuccessMSGAddCategory };

        }
        #endregion

        #region Public Methods


        public async Task<CommonAPIResponseModel> DeleteCategory(int categoryId)
        {
            // Validate Id
            IsCategoryIdValid isCategoryIdValid = new IsCategoryIdValid(_dbContext);
            if (!isCategoryIdValid.IsIDValid(categoryId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGCategory };

            var category = _dbContext.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefault();
            category.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            return new CommonAPIResponseModel() { StatusCode = 0, Message = ConstantValues.SuccessMSGDeleteCategory };

        }

        public async Task<CommonAPIResponseModel> GetCategory(int categoryId)
        {
            // Validate Id
            IsCategoryIdValid isCategoryIdValid = new IsCategoryIdValid(_dbContext);
            if (!isCategoryIdValid.IsIDValid(categoryId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGCategory };


            var category = await _dbContext.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
            CategoryResponseDTO categoryResponseDTO = new CategoryResponseDTO()
            { CategoryId = category.CategoryId, CategoryName = category.CategoryName };
            return new CommonAPIResponseModel() { StatusCode = 0, Data = categoryResponseDTO };
        }

        public async Task<CommonAPIResponseModel> UpdateCategory(int categoryId, CategoryRequestDTO category)
        {
            // Validate Id
            IsCategoryIdValid isCategoryIdValid = new IsCategoryIdValid(_dbContext);
            if (!isCategoryIdValid.IsIDValid(categoryId))
                return new CommonAPIResponseModel() { StatusCode = 1, Message = ConstantValues.NotFoundMSGCategory };

            var categoryObject = _dbContext.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefault();

            categoryObject.UpdatedDate = DateTime.Now;
            categoryObject.CategoryName = category.CategoryName;
            await _dbContext.SaveChangesAsync();

            return new CommonAPIResponseModel() { StatusCode = 0, Message = ConstantValues.SuccessMSGUpdatedCategory };
        }

        #endregion
    }
}
