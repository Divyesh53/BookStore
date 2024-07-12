using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        #region Private Fields
        private readonly IRolesService _rolesService;
        private readonly ILogger<RolesController> _logger;
        #endregion

        #region Constructor
        public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
        {
            this._rolesService = rolesService;
            _logger = logger;
            _logger.LogInformation(1, "NLog injected into RolesController");
        }
        #endregion

        #region API Methods
        [Route("/getRoles")]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                if (_rolesService != null)
                {
                    var rolesList = await _rolesService.GetRoles();
                    commonAPIResponseModel.Data = rolesList;
                    commonAPIResponseModel.StatusCode = 0;
                }
                else
                {
                    commonAPIResponseModel.StatusCode = 1;
                    commonAPIResponseModel.Message = "Something Went Wrong!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("RolesController ->  GetRoles: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("RolesController ->  GetRoles: Finally executed: ");
            }
            return Ok(commonAPIResponseModel);
        }

        #endregion
    }
}
