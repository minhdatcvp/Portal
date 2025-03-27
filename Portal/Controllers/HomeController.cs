using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.Services;
using PortalService.Services;
using System.Diagnostics;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataFileService _dataFileService;

        public HomeController(ILogger<HomeController> logger, IDataFileService dataFileService)
        {
            _logger = logger;
            _dataFileService = dataFileService;
        }

        public IActionResult Index()
        {
            try
            {
                var data = _dataFileService.GetData();

                if (data == null || !data.Data.Any())
                {
                    _logger.LogWarning("No data found for Index action.");
                    return NotFound("No data available.");
                }

                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public IActionResult Information()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
