using BookStore.Models.Models;
using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        #region Private Fields
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            this._authService = authService;
            _logger = logger;
            _logger.LogInformation(1, "NLog injected into AuthController");
        }
        #endregion

        #region API Methods
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestModel user)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _authService.Login(user);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("AuthController ->  Login: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("AuthController ->  Login: Finally executed: ");
            }
            return Ok(commonAPIResponseModel);
        }
        #endregion
    }
}
