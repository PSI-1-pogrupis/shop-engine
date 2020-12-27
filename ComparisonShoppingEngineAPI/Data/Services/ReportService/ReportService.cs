using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ComparisonShoppingEngineAPI.Data.Services.ReportService
{
    public partial class ReportService : IReportService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId => int.Parse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public async Task<ServiceResponse<GetReportDto>> Add(AddReportDto report)
        {
            ServiceResponse<GetReportDto> service = new ServiceResponse<GetReportDto>();

            try
            {
                Report _report = new Report { UserId = GetUserId, Problem = report.Problem, Date = DateTime.UtcNow };

                _context.Reports.Add(_report);
                await _context.SaveChangesAsync();
                service.Data = new GetReportDto { Id = _report.Id, Date = _report.Date };
                service.Message = ($"Report #{ _report.Id } has been submitted. Thank you!");
                service.Success = true;
            } catch (Exception)
            {
                service.Data = null;
                service.Message = "Could not submit report!";
            }

            return service;
        }
    }
}