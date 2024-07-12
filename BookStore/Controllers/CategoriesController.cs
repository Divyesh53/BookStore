using BookStore.Models.Helpers;
using BookStore.Models.Models;
using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Authorize(Policy = "Admin")]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        #region Private Fields
        private readonly ICategoriesService _categoryService;
        private readonly ILogger<CategoriesController> _logger;
        #endregion

        #region Constructor
        public CategoriesController(ICategoriesService categoryService, ILogger<CategoriesController> logger)
        {
            this._categoryService = categoryService;
            _logger = logger;
            _logger.LogInformation(1, "NLog injected into CategoriesController");
        }
        #endregion

        #region API Methods
        [Route("/addCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDTO category)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _categoryService.AddCategory(category);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CategoriesController ->  AddUser: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("CategoriesController ->  AddUser: Finally executed: ");
            }
            return Ok(commonAPIResponseModel);
        }
        [Route("/updateCategory/{categoryId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateCategory(int userId, [FromBody] CategoryRequestDTO category)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _categoryService.UpdateCategory(userId, category);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CategoriesController ->  UpdateCategory: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("CategoriesController ->  UpdateCategory: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("/deleteCategory/{categoryId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _categoryService.DeleteCategory(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CategoriesController ->  DeleteCategory: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("CategoriesController ->  DeleteCategory: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("/getCategory/{categoryId}")]
        [HttpGet]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _categoryService.GetCategory(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("CategoriesController ->  GetCategory: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("CategoriesController ->  GetCategory: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }
        #endregion
    }
}
