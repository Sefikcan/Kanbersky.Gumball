using AutoMapper;
using Kanbersky.Gumball.Business.DTO.Request;
using Kanbersky.Gumball.Business.DTO.Response;
using Kanbersky.Gumball.Entities.Concrete;

namespace Kanbersky.Gumball.Business.Mappings.AutoMapper
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Application, ApplicationResponseModel>().ReverseMap();

            CreateMap<Application, CreateApplicationRequestModel>().ReverseMap();
            CreateMap<Application, CreateApplicationResponseModel>().ReverseMap();

            CreateMap<Config, ConfigResponseModel>().ReverseMap();

            CreateMap<Config, CreateConfigRequestModel>().ReverseMap();
            CreateMap<Config, CreateConfigResponseModel>().ReverseMap();

            CreateMap<Config, UpdateConfigResponseModel>().ReverseMap();
        }
    }
}
