using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Report;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.ReportService
{
    public interface IReportService
    {
        Task<ServiceResponse<GetReportDto>> Add(AddReportDto receipt);
    }
}
