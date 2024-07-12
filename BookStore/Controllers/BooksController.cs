using BookStore.Models.Models;
using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using BookStore.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{

    [Authorize(Roles = "Seller,Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        #region Private Fields
        private readonly IBooksService _booksService;
        private readonly ILogger<BooksController> _logger;
        #endregion

        #region Constructor
        public BooksController(IBooksService booksService, ILogger<BooksController> logger)
        {
            this._booksService = booksService;
            _logger = logger;
            _logger.LogInformation(1, "NLog injected into BooksController");
        }
        #endregion

        #region API Methods

        [Route("/addBook")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookRequestDTO user)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                string userName = User.Identity.Name;
                commonAPIResponseModel = await _booksService.AddBook(user, userName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("BooksController ->  AddBook: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("BooksController ->  AddBook: Finally executed: ");
            }
            return Ok(commonAPIResponseModel);
        }

        [Route("/updateBook/{bookId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] BookRequestDTO user)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _booksService.UpdateBook(bookId, user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("BooksController ->  UpdateBook: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("BooksController ->  UpdateBook: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("/deleteBook/{bookId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _booksService.DeleteBook(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("BooksController ->  DeleteBook: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("BooksController ->  DeleteBook: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("/getBook/{bookId}")]
        [HttpGet]
        public async Task<IActionResult> GetBook(int bookId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _booksService.GetBook(bookId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("BooksController ->  GetBook: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("BooksController ->  GetBook: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Authorize(Roles = "Seller")]
        [Route("getSoldBooks")]
        [HttpGet]
        public async Task<IActionResult> GetSoldBooks()
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                string userName = User.Identity.Name;
                commonAPIResponseModel = await _booksService.GetSoldBooks(userName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("BooksController ->  GetSoldBooks: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("BooksController ->  GetSoldBooks: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }
        #endregion
    }
}