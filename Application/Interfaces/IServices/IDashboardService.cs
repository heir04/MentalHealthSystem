using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<BaseResponse<UserDashboardDto>> GetUserDashboard();
        Task<BaseResponse<AdminDashboardDto>> GetAdminDashboard();
        Task<BaseResponse<ReportsDashboardDto>> GetReportsDashboard();
    }
}
