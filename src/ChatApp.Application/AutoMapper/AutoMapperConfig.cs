using AutoMapper;

namespace ChatApp.Application.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(m =>
            {
                m.AddProfile<DomainToModelMappingProfile>();
                m.AddProfile<ModelToDomainMappingProfile>();
            });
        }
    }
}
