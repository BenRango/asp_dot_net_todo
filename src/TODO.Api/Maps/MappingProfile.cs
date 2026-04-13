using TODO.Api.Features.Tasks;
using AutoMapper;
using TODO.Api.Features.Users;

namespace TODO.Api.Maps
{
    public class MappingProfile: Profile
    {
        public MappingProfile (){
            CreateMap<TodoTask, TaskResponseDto>();
            CreateMap<TaskRegisterDto, TodoTask>();

            CreateMap<UserRegisterInfoDto, User>();
            CreateMap<User, UserResponseDto>();
        }
    }
}