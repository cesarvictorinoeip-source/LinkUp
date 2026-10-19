using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship.User;
using LinkUp.Core.Application.ViewModels.User;
using LinkUp.Core.Application.ViewModels.User.LinkUp.Core.Application.ViewModels.User;
using LinkUp.Core.Domain.Entities;


namespace LinkUp.Core.Application.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, AuthenticationResponse>().ReverseMap();
            CreateMap<AppUser, UserViewModel>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();

            CreateMap<LoginViewModel, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterUserViewModel, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateUserViewModel, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ForgotPasswordRequestViewModel, AppUser>().ReverseMap();
            CreateMap<ResetPasswordRequestViewModel, AppUser>().ReverseMap();
            CreateMap<DeleteUserViewModel, AppUser>().ReverseMap();
        }
    }
}