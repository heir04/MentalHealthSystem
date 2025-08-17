using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<BaseResponse<CreateUserDto>> Create(CreateUserDto UsertDto);
        Task<BaseResponse<UserDto>> Delete(Guid id);
        Task<BaseResponse<UserDto>> Profile();
        Task<BaseResponse<IEnumerable<UserDto>>> GetAll();
        Task<BaseResponse<LoginDto>> Login(LoginDto loginDto);
        Task<BaseResponse<ChangePasswordDto>> ChangePassword(ChangePasswordDto changePasswordDto);
        Task<BaseResponse<UpdateUserDto>> Update(Guid id, UpdateUserDto UsertDto);
    }
}