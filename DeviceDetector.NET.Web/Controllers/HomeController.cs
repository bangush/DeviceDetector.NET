using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using Microsoft.AspNetCore.Mvc;

namespace DeviceDetectorNet.Web.Controllers
{
    [ApiController]

    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers.UserAgent; // change this to the useragent you want to parse
            var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);  // client hints are optional

            var result = DeviceDetector.GetInfoFromUserAgent(userAgent, clientHints);

            var output = result.Success ? result.ToString().Replace(Environment.NewLine, "<br />") : "Unknown";
            _logger.LogDebug(output);

            return result;
        }
    }
}