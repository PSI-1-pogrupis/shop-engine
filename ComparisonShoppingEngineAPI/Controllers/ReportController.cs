using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ComparisonShoppingEngineAPI.Data.Services.ReportService;
using ComparisonShoppingEngineAPI.DTOs.Report;
using ComparisonShoppingEngineAPI.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("report")]

    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportService _reportService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportController(ILogger<ReportController> logger, IReportService reportService, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _reportService = reportService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> PostReport([FromBody] AddReportDto report)
        {
            ServiceResponse<GetReportDto> serviceResponse = await _reportService.Add(report);

            if (!serviceResponse.Success)
            {
                _logger.BeginScope($"{{{DateTime.UtcNow}}}:(Error)Report from user {int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)} has been rejected.");
                return NotFound(serviceResponse);
            }
            else
            {
                _logger.BeginScope($"{{{serviceResponse.Data.Date}}}:Report {serviceResponse.Data.Id} has been added.");
                return Ok(serviceResponse);
            }
        }
    }
}