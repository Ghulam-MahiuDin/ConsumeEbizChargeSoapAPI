using ConsumeEbizCharge.API.Model;
using ConsumeEbizCharge.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConsumeEbizCharge.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SoapController : ControllerBase
    {
        private readonly SoapService _soapService;

        public SoapController(SoapService soapService)
        {
            _soapService = soapService;
        }

        [HttpGet("GetMerchantIntegrationSettings")]
        public async Task<IActionResult> GetMerchantIntegrationSettings()
        {
            var request = new SoapRequest
            {
                SecurityKey = "bf2a208f-794b-4cee-b4df-6318a4007523",
                Username = "businesscentral1",
                Password = "businesscentral1"
            };

            var response = await _soapService.GetMerchantIntegrationSettingsAsync(request);
            return Ok(response);
        }
    }
}
