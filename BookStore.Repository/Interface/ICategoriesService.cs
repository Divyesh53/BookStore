using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Repository.Interface
{
    public interface ICategoriesService
    {
        Task<CommonAPIResponseModel> AddCategory(CategoryRequestDTO category);
        Task<CommonAPIResponseModel> UpdateCategory(int categoryId, CategoryRequestDTO category);
        Task<CommonAPIResponseModel> DeleteCategory(int categoryId);
        Task<CommonAPIResponseModel> GetCategory(int categoryId);
    }
}