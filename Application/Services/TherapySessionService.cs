using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Application.Services
{
    public class TherapySessionService(IUnitOfWork unitOfWork,ValidatorHelper validatorHelper) : ITherapySessionService
    {
        private readonly ValidatorHelper _validatorHelper = validatorHelper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<BaseResponse<TherapySessionDto>> BookSession(Guid therapistid)
        {
            var response = new BaseResponse<TherapySessionDto>();
            var userId = _validatorHelper.GetUserId();

            var therapist = await _unitOfWork.Therapist.Get(t => t.Id == therapistid);
            if (therapist is null)
            {
                response.Message = "Therapist not found";
                return response;
            }

            if (!therapist.IsAdminApproved )
            {
                response.Message = "Therapist is not approved";
                return response;
            }

            var sessionExist = await _unitOfWork.TherapySession.ExistsAsync(ts => ts.UserId == userId && ts.TherapistId == therapistid && (ts.Status == TherapySessionStatus.Pending || ts.Status == TherapySessionStatus.Scheduled));
            if (sessionExist)
            {
                response.Message = "You have a pending or scheduled session with the Therapist";
                return response;   
            }

            var session = new TherapySession
            {
                UserId = userId,
                TherapistId = therapistid
            };

            await _unitOfWork.TherapySession.Register(session);
            await _unitOfWork.SaveChangesAsync();
            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<TherapySessionDto>> Get(Guid id)
        {
            var response = new BaseResponse<TherapySessionDto>();
            var userId = _validatorHelper.GetUserId();
            var user = await _unitOfWork.User.Get(u => u.Id == userId && !u.IsDeleted);
            if (user is null)
            {
                response.Message = "User not found";
                return response;
            }

            var session = await _unitOfWork.TherapySession.Get(ts => ts.Id == id);
            if (session is null)
            {
                response.Message = "Session not found";
                return response;
            }

            var therapist = await _unitOfWork.Therapist.Get(th => th.Id == session.TherapistId && !th.IsDeleted);

            bool isSessionOwner = user.Id == session.UserId;
            bool isAssignedTherapist = therapist != null && therapist.UserId == user.Id;
            bool isAdmin = user.Role.ToString() == "Admin";

            if (!isSessionOwner && !isAssignedTherapist && !isAdmin)
            {
                response.Message = "Unauthorized access to session";
                return response;
            }

            response.Data = new TherapySessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                TherapistId = session.TherapistId,
                Status = session.Status.ToString()
            };
            response.Status = true;
            response.Message = "Success";

            return response;
        }

        public async Task<BaseResponse<IEnumerable<TherapySessionDto>>> GetAllByActiveUser()
        {
            var response = new BaseResponse<IEnumerable<TherapySessionDto>>();
            var userId = _validatorHelper.GetUserId();
            var sessions = await _unitOfWork.TherapySession.GetAll(ts => ts.UserId == userId);
            if (!sessions.Any())
            {
                response.Message = "No session found";
                return response;
            }
           
            response.Data = sessions.Select(ts => new TherapySessionDto
            {
                Id = ts.Id,
                UserId = ts.UserId,
                TherapistId = ts.TherapistId,
                Status = ts.Status.ToString()
            }).ToList();
            response.Status = true;
            response.Message = "Success";

            return response;
        }

        public async Task<BaseResponse<IEnumerable<TherapySessionDto>>> GetAllByTherapist()
        {
            var response = new BaseResponse<IEnumerable<TherapySessionDto>>();
            var userId = _validatorHelper.GetUserId();

            var therapist = await _unitOfWork.Therapist.Get(th => th.UserId == userId);
            var sessions = await _unitOfWork.TherapySession.GetAll(ts => ts.TherapistId == therapist.Id);

            if (!sessions.Any())
            {
                response.Message = "No session found";
                return response;
            }

            response.Data = [.. sessions.Select(ts => new TherapySessionDto
            {
                Id = ts.Id,
                UserId = ts.UserId,
                TherapistId = ts.TherapistId,
                Status = ts.Status.ToString()
            })];
            response.Status = true;
            response.Message = "Success";

            return response;
        }

        public async Task<BaseResponse<TherapySessionDto>> UpdateSessionStatus(Guid id, TherapySessionStatus status)
        {
            var response = new BaseResponse<TherapySessionDto>();

            var session = await _unitOfWork.TherapySession.Get(ts => ts.Id == id);
            if (session is null)
            {
                response.Message = "Session not found";
                return response;
            }

            session.Status = status;
            await _unitOfWork.SaveChangesAsync();

            response.Data = new TherapySessionDto
            {
                Id = session.Id,
                UserId = session.UserId,
                TherapistId = session.TherapistId,
                Status = session.Status.ToString()
            };
            response.Status = true;
            response.Message = "Success";

            return response;
        }
    }
}