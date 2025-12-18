using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IDashboardService
    {
        Task<BaseResponse<UserDashboardDto>> GetUserDashboard();
        Task<BaseResponse<TherapistDashboardDto>> GetTherapistDashboard();
        Task<BaseResponse<AdminDashboardDto>> GetAdminDashboard();
        Task<BaseResponse<ReportsDashboardDto>> GetReportsDashboard();
        Task<BaseResponse<GeneralDashboardDto>> GetGeneralDashboard();
    }
}
