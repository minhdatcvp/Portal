using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using PortalCommon;
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
            ViewBag.BaseUrl = AppSettings.BaseUrl;
            return View();
        }

        public IActionResult Information()
        {
            ViewBag.BaseUrl = AppSettings.BaseUrl;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
