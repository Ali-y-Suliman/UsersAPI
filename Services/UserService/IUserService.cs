using Users.Dtos.User;

namespace Users.Services.UserService
{
    public interface IUserService {
        Task <ServiceResponse<SignInResponseDto>> register (RegisterRequestDto user);
        Task <ServiceResponse<SignInResponseDto>> signIn (SignInRquestDto user);
        Task <ServiceResponse<SignInResponseDto>> getUser (string email);
        // Task <ServiceResponse<Boolean>> changePassword (ChangePasswordDto user);
    }
}