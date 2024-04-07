using AutoMapper;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Domain.Entities;

namespace BlackGuardApp.Mapper
{
    public class MapperProfile : Profile
    {
       
        public MapperProfile()
        {
            CreateMap<BlackList, BlacklistedProductDto>()
                .ForMember(dest => dest.BlacklistId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.CriteriaName, opt => opt.MapFrom(src => src.BlacklistCriteria.CategoryName))
                .ForMember(dest => dest.CriteriaDescription, opt => opt.MapFrom(src => src.BlacklistCriteria.CategoryDescription))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }

    }
}
