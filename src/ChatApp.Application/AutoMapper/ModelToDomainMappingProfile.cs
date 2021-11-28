using AutoMapper;
using ChatApp.Application.ViewModels;
using ChatApp.Domain.Entities;

namespace ChatApp.Application.AutoMapper
{
    public class ModelToDomainMappingProfile : Profile
    {
        public override string ProfileName => "ModelToDomainMappings";

        protected override void Configure()
        {
            CreateMap<UserViewModel, User>();

        }
    }
}
