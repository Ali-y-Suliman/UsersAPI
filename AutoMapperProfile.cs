using AutoMapper;
using Users.Dtos.User;

namespace Users
{
    public class AutoMapperProfile : Profile {
        public AutoMapperProfile()
        {
            CreateMap<User,SignInResponseDto>();  //
            CreateMap<SignInRquestDto,User>();
            CreateMap<RegisterRequestDto,User>();
            CreateMap<RegisterRequestDto,SignInResponseDto>(); //
            CreateMap<ChangePasswordDto,User>();
        }
    }
}