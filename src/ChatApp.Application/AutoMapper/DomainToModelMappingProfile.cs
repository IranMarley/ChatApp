using AutoMapper;
using ChatApp.Application.ViewModels;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.AutoMapper
{
    public class DomainToModelMappingProfile : Profile
    {
        public override string ProfileName => "DomainToModelMappings";

        protected override void Configure()
        {
            CreateMap<User, UserViewModel>();           
        }
    }
}
