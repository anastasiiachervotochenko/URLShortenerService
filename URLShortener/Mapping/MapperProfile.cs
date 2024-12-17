using AutoMapper;
using URLShortener.Data.Entity;
using URLShortener.Domain.Contracts.Models.DomainModels;
using URLShortener.Domain.Contracts.Models.RequestModels;

namespace URLShortener.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<UrlLog, UrlLogModel>().ReverseMap();
        CreateMap<CreateUserRequestModel, User>()
            .ForMember(u => u.Id, opt => opt.MapFrom(cu => Guid.NewGuid().ToString()));
    }
}