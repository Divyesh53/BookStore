using BookStore.Models.Models;
using BookStore.Models.RequestModels;
using BookStore.Models.ResponseModels;
using BookStore.Repository.Interface;
using BookStore.Repository.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize(Policy = "Buyer")]
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        #region Private Fields
        private readonly IPurchasesService _purchase;
        private readonly ILogger<PurchaseController> _logger;
        #endregion

        #region Constructor
        public PurchaseController(IPurchasesService purchase, ILogger<PurchaseController> logger)
        {
            this._purchase = purchase;
            _logger = logger;
            _logger.LogInformation(1, "NLog injected into PurchaseController");
        }
        #endregion

        #region API Methods
        [Route("/addPurchase")]
        [HttpPost]
        public async Task<IActionResult> AddPurchase([FromBody] PurchaseRequestDTO purchase)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                string userName = User.Identity.Name;
                commonAPIResponseModel = await _purchase.AddPurchase(purchase, userName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PurchaseController ->  AddPurchase: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("PurchaseController ->  AddPurchase: Finally executed: ");
            }
            return Ok(commonAPIResponseModel);
        }

        [Route("/updatePurchase/{purchaseId}")]
        [HttpPatch]
        public async Task<IActionResult> UpdatePurchase(int purchaseId, [FromBody] PurchaseRequestDTO purchase)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();
                commonAPIResponseModel = await _purchase.UpdatePurchase(purchaseId, purchase);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PurchaseController ->  UpdatePurchase: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("PurchaseController ->  UpdatePurchase: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("/deletePurchase/{purchaseId}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePurchase(int purchaseId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();

            try
            {
                commonAPIResponseModel = await _purchase.DeletePurchase(purchaseId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PurchaseController ->  DeletePurchase: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("PurchaseController ->  DeletePurchase: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("/getPurchase/{purchaseId}")]
        [HttpGet]
        public async Task<IActionResult> GetPurchase(int purchaseId)
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                commonAPIResponseModel = await _purchase.GetPurchase(purchaseId);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PurchaseController ->  GetPurchase: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("PurchaseController ->  GetPurchase: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }

        [Route("purchasedBooks")]
        [HttpGet]
        public async Task<IActionResult> GetPurchasedBooks()
        {
            CommonAPIResponseModel commonAPIResponseModel = new CommonAPIResponseModel();
            try
            {
                string userName = User.Identity.Name;
                commonAPIResponseModel = await _purchase.GetPurchasedBooks(userName);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PurchaseController ->  GetPurchasedBooks: Exception occur: ", ex.Message);
                return BadRequest(commonAPIResponseModel);
            }
            finally
            {
                _logger.LogInformation("PurchaseController ->  GetPurchasedBooks: Finally executed: ");
            }
            if (commonAPIResponseModel.StatusCode == 0)
                return Ok(commonAPIResponseModel);
            else
                return NotFound(commonAPIResponseModel);
        }
        #endregion
    }
}