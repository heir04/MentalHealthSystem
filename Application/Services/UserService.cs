using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ValidatorHelper _validatorHelper = validatorHelper;
        public async Task<BaseResponse<CreateUserDto>> Create(CreateUserDto userDto)
        {
            var response = new BaseResponse<CreateUserDto>();
            var ifEmailExist = await _unitOfWork.User.ExistsAsync(x => x.Email == userDto.Email&& !x.IsDeleted);
            if (ifEmailExist)
            {
                response.Message = $"User with email: {userDto.Email} already exist";
                return response;
            }

            var ifUsernameExist = await _unitOfWork.User.ExistsAsync(x => x.Username == userDto.Username && !x.IsDeleted);
            if (ifUsernameExist)
            {
                response.Message = $"User with Username: {userDto.Username} already exist";
                return response;
            }

            string saltString = HashingHelper.GenerateSalt();
            string hashedPassword = HashingHelper.HashPassword(userDto.Password, saltString);
            var user = new User
            {
                Email = userDto.Email.ToLower(),
                Username = userDto.Username,
                HashSalt = saltString,
                PasswordHash = hashedPassword,
                Role = "User"
            };

            await _unitOfWork.User.Register(user);
            await _unitOfWork.SaveChangesAsync();

            response.Data = userDto;
            response.Status = true;
            response.Message = "User created successfully";
            return response;
        }

        public async Task<BaseResponse<CreateUserDto>> CreateAdmin(CreateUserDto userDto)
        {
            var response = new BaseResponse<CreateUserDto>();
            var ifEmailExist = await _unitOfWork.User.ExistsAsync(x => x.Email == userDto.Email&& !x.IsDeleted);
            if (ifEmailExist)
            {
                response.Message = $"User with email: {userDto.Email} already exist";
                return response;
            }

            var ifUsernameExist = await _unitOfWork.User.ExistsAsync(x => x.Username == userDto.Username && !x.IsDeleted);
            if (ifUsernameExist)
            {
                response.Message = $"User with Username: {userDto.Username} already exist";
                return response;
            }

            string saltString = HashingHelper.GenerateSalt();
            string hashedPassword = HashingHelper.HashPassword(userDto.Password, saltString);
            var user = new User
            {
                Email = userDto.Email.ToLower(),
                Username = userDto.Username,
                HashSalt = saltString,
                PasswordHash = hashedPassword,
                Role = "Admin"
            };

            await _unitOfWork.User.Register(user);
            await _unitOfWork.SaveChangesAsync();

            response.Data = userDto;
            response.Status = true;
            response.Message = "User created successfully";
            return response;
        }

        public async Task<BaseResponse<UserDto>> Delete(Guid id)
        {
            var response = new BaseResponse<UserDto>();
            var user = await _unitOfWork.User.Get(u => u.Id == id && !u.IsDeleted);
            if (user is null)
            {
                response.Message = "User not found";
                return response;
            }

            var userId = _validatorHelper.GetUserId();
            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            if (userId != user.Id && _validatorHelper.GetUserRole() != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }
            user.IsDeleted = true;
            user.IsDeletedBy = userId;
            user.IsDeletedOn = DateTime.UtcNow;
            user.LastModifiedBy = userId;
            user.LastModifiedOn = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<UserDto>>> GetAll()
        {
            var response = new BaseResponse<IEnumerable<UserDto>>();
            var users = await _unitOfWork.User.GetAll(u => !u.IsDeleted);
            if (users is null || !users.Any())
            {
                response.Message = "No users found!";
                return response;
            }   

            response.Data = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            });
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<UserDto>> Login(LoginDto loginDto)
        {
            var response = new BaseResponse<UserDto>();
            var user = await _unitOfWork.User.Get(x => x.Email == loginDto.Email.ToLower() && !x.IsDeleted);

            if (user is null)
            {
                response.Message = $"Incorrect email or password!";
                return response;
            }

            if (user.HashSalt == null)
            {
                response.Message = $"Incorrect email or password!";
                return response;
            }

            string hashedPassword = HashingHelper.HashPassword(loginDto.Password, user.HashSalt);
            if (user.PasswordHash == null || !user.PasswordHash.Equals(hashedPassword))
            {
                response.Message = $"Incorrect email or password!";
                return response;
            }

            
            response.Data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };

            response.Message = "Welcome";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<UserDto>> Profile()
        {
            var response = new BaseResponse<UserDto>();
            var userId = _validatorHelper.GetUserId();

            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            var user = await _unitOfWork.User.Get(u => u.Id == userId && !u.IsDeleted);
            if (user is null)
            {
                response.Message = "User not found";
                return response;
            }

            response.Data = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role.ToLower(),
                CreatedOn = user.CreatedOn
            };
            
            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<UpdateUserDto>> Update(Guid id, UpdateUserDto userDto)
        {
            var response = new BaseResponse<UpdateUserDto>();
            var userId = _validatorHelper.GetUserId();

            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            var user = await _unitOfWork.User.Get(u => u.Id == userId && !u.IsDeleted);
            if (user is null)
            {
                response.Message = "User not found";
                return response;
            }

            user.Email = userDto.Email ?? user.Email;
            user.Username = userDto.Username ?? user.Username;

            await _unitOfWork.SaveChangesAsync();

            response.Data = new UpdateUserDto
            {
                Email = user.Email,
                Username = user.Username,
            };

            response.Status = true;
            response.Message = "User updated successfully";
            return response;
        }

        public async Task<BaseResponse<ChangePasswordDto>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response = new BaseResponse<ChangePasswordDto>();

            var userId = _validatorHelper.GetUserId();

            var user = await _unitOfWork.User.Get(u => u.Id == userId && !u.IsDeleted);
            if (user == null)
            {
                response.Message = "User not found";
                return response;
            }
            string hashedCurrentPassword = HashingHelper.HashPassword(changePasswordDto.CurrentPassword, user.HashSalt);
            if (user.PasswordHash == null || !user.PasswordHash.Equals(hashedCurrentPassword))
            {
                response.Message = "Current password is incorrect";
                return response;
            }

            string saltString = HashingHelper.GenerateSalt();
            string hashedPassword = HashingHelper.HashPassword(changePasswordDto.NewPassword, saltString);
            user.HashSalt = saltString;
            user.PasswordHash = hashedPassword;
            user.LastModifiedBy = user.Id; 
            user.LastModifiedOn = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();
            response.Message = "Password Updated";
            response.Status = true;
            return response;
        }
    }
}