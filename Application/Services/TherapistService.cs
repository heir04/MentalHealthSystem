using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Services
{
    public class TherapistService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper) : ITherapistService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ValidatorHelper _validatorHelper = validatorHelper;
        public async Task<BaseResponse<CreateTherapistDto>> Create(CreateTherapistDto therapistDto)
        {
            var response = new BaseResponse<CreateTherapistDto>();

            var ifEmailExist = await _unitOfWork.User.ExistsAsync(x => x.Email == therapistDto.Email);
            if (ifEmailExist)
            {
                response.Message = $"Therapist with email: {therapistDto.Email} already exist";
                return response;
            }
            
            string saltString = HashingHelper.GenerateSalt();
            string hashedPassword = HashingHelper.HashPassword("12345", saltString);
            var user = new User
            {
                Email = therapistDto.Email,
                Username = therapistDto.FullName.Replace(" ", "").ToLower(),
                Role = "Therapist",
                HashSalt = saltString,
                PasswordHash = hashedPassword,
                CreatedOn = DateTime.UtcNow
            };

            var therapist = new Therapist
            {
                FullName = therapistDto.FullName,
                Specialization = therapistDto.Specialization,
                CertificationLink = therapistDto.CertificationLink,
                Bio = therapistDto.Bio,
                ContactLink = therapistDto.ContactLink,
                UserId = user.Id
            };

            await _unitOfWork.Therapist.Register(therapist);
            await _unitOfWork.User.Register(user);
            await _unitOfWork.SaveChangesAsync();

            response.Data = therapistDto;
            response.Status = true;
            response.Message = "Therapist created successfully";
            return response;
        }

        public async Task<BaseResponse<TherapistDto>> Delete(Guid id)
        {
            var response = new BaseResponse<TherapistDto>();

            var therapist = await _unitOfWork.Therapist.Get(t => t.Id == id && !t.IsDeleted);
            if (therapist is null)
            {
                response.Message = "Therapist not found";
                return response;
            }

            var userId = _validatorHelper.GetUserId();
            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            if (userId != therapist.UserId && _validatorHelper.GetUserRole() != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }

            therapist.IsDeleted = true;
            therapist.IsDeletedOn = DateTime.UtcNow;
            therapist.IsDeletedBy = userId;

            await _unitOfWork.SaveChangesAsync();

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<TherapistDto>> Get(Guid id)
        {
            var response = new BaseResponse<TherapistDto>();

            var therapist = await _unitOfWork.Therapist.Get(t => t.Id == id && !t.IsDeleted);
            if (therapist is null)
            {
                response.Message = "Therapist not found";
                return response;
            }

            response.Data = new TherapistDto
            {
                Id = therapist.Id,
                UserId = therapist.UserId,
                FullName = therapist.FullName,
                Specialization = therapist.Specialization,
                CertificationLink = therapist.CertificationLink,
                Bio = therapist.Bio,
                ContactLink = therapist.ContactLink,
                Availability = therapist.Availability,
                UserName = therapist.User?.Username
            };

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<TherapistDto>> ApproveTherapist(Guid id)
        {
            var response = new BaseResponse<TherapistDto>();

            var therapist = await _unitOfWork.Therapist.Get(t => t.Id == id && !t.IsDeleted);
            if (therapist is null)
            {
                response.Message = "Therapist not found";
                return response;
            }

            therapist.IsAdminApproved = true;
            therapist.LastModifiedBy = _validatorHelper.GetUserId();
            therapist.LastModifiedOn = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<TherapistDto>>> GetAll()
        {
            var response = new BaseResponse<IEnumerable<TherapistDto>>();
            var therapists = await _unitOfWork.Therapist.GetAll(t => !t.IsDeleted);
            if (therapists is null || !therapists.Any())
            {
                response.Message = "No therapists found";
                return response;
            }

            response.Data = therapists.Select(t => new TherapistDto
            {
                Id = t.Id,
                UserId = t.UserId,
                FullName = t.FullName,
                Specialization = t.Specialization,
                CertificationLink = t.CertificationLink,
                Bio = t.Bio,
                ContactLink = t.ContactLink,
                Availability = t.Availability,
                UserName = t.User?.Username
            });

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<TherapistDto>>> GetAdminApproved()
        {
            var response = new BaseResponse<IEnumerable<TherapistDto>>();
            var therapists = await _unitOfWork.Therapist.GetAll(t => !t.IsDeleted && t.IsAdminApproved);
            if (therapists is null || !therapists.Any())
            {
                response.Message = "No therapists found";
                return response;
            }

            response.Data = therapists.Select(t => new TherapistDto
            {
                Id = t.Id,
                UserId = t.UserId,
                FullName = t.FullName,
                Specialization = t.Specialization,
                Bio = t.Bio,
                ContactLink = t.ContactLink,
                Availability = t.Availability,
                UserName = t.User?.Username
            });

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<TherapistDto>> Update(Guid id, UpdateTherapistDto therapistDto)
        {
            var response = new BaseResponse<TherapistDto>();

            var therapist = await _unitOfWork.Therapist.Get(t => t.Id == id && !t.IsDeleted);
            if (therapist is null)
            {
                response.Message = "Therapist not found";
                return response;
            }

            therapist.FullName = therapistDto.FullName;
            therapist.Specialization = therapistDto.Specialization;
            therapist.Bio = therapistDto.Bio;
            therapist.ContactLink = therapistDto.ContactLink;
            therapist.Availability = therapistDto.Availability;
            therapist.LastModifiedBy = _validatorHelper.GetUserId();
            therapist.LastModifiedOn = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            response.Message = "Success";
            response.Status = true;
            return response;
        }
    }
}