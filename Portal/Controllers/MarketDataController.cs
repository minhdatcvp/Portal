using Microsoft.AspNetCore.Mvc;
using PortalService.Services;

namespace Portal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketDataController : ControllerBase
    {
        private readonly IDataFileService _dataFileService;
        private readonly ILogger<MarketDataController> _logger;

        public MarketDataController(IDataFileService dataFileService, ILogger<MarketDataController> logger)
        {
            _dataFileService = dataFileService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetMarketData(int page = 1, int pageSize = 10, string sortBy = "Date", string sortOrder = "desc", string? search = null)
        {
            _logger.LogInformation("GetMarketData called with page={Page}, pageSize={PageSize}, sortBy={SortBy}, sortOrder={SortOrder}, search={Search}",
                                    page, pageSize, sortBy, sortOrder, search);

            try
            {
                var (totalRecords, data) = _dataFileService.PagingData(page, pageSize, sortBy, sortOrder, search);

                _logger.LogInformation("Successfully retrieved market data. Total records: {TotalRecords}", totalRecords);

                return Ok(new
                {
                    totalRecords,
                    data
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument provided for GetMarketData");
                return BadRequest(new { message = "Invalid request parameters", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching market data");
                return StatusCode(500, new { message = "An unexpected error occurred. Please try again later." });
            }
        }
    }
}
