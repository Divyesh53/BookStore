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
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        #region Private Fields
        private readonly IUsesrService _usersService;
        private readonly ILogger<UsersController> _logger;
        #endregion

        #region Constructor
        public UsersController(IUsesrService usersService, ILogger<UsersController> logger)
        {
            this._usersService = usersService;
            _logger = logger;
            _logger.LogInformation(1, "NLog injected into UsersController");
        }
        #endregion

        #region API Methods
        [Route("/addUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserRequestDTO user)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _usersService.AddUser(user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UsersController ->  AddUser: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("UsersController ->  AddUser: Finally executed: ");
            }
            return Ok(commonAPIResponseModel);
        }
        [Authorize]
        [Route("/updateUser/{userId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserRequestDTO user)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _usersService.UpdateUser(userId, user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UsersController ->  updateUser: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("UsersController ->  updateUser: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }
        [Authorize]
        [Route("/deleteUser/{userId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _usersService.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UsersController ->  deleteUser: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("UsersController ->  deleteUser: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Authorize]
        [Route("/getUser/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUser(int userId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _usersService.GetUser(userId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UsersController ->  GetUser: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("UsersController ->  GetUser: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }
        #endregion
    }
}
