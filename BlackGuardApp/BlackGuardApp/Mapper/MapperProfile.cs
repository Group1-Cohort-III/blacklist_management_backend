using AutoMapper;
using BlackGuardApp.Application.DTOs;
using BlackGuardApp.Common.Utilities;
using BlackGuardApp.Domain.Entities;

namespace BlackGuardApp.Mapper
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<AppUser, GetAllUsersDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.IsPasswordSet, opt => opt.MapFrom(src => src.IsPasswordSet));
            CreateMap<PageResult<IEnumerable<AppUser>>, PageResult<IEnumerable<GetAllUsersDto>>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data))
                .ForMember(dest => dest.TotalPageCount, opt => opt.MapFrom(src => src.TotalPageCount))
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.PerPage, opt => opt.MapFrom(src => src.PerPage))
                .ForMember(dest => dest.TotalCount, opt => opt.MapFrom(src => src.TotalCount));

            CreateMap<BlackList, BlacklistedProductDto>()
                .ForMember(dest => dest.BlacklistId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.CriteriaName, opt => opt.MapFrom(src => src.BlacklistCriteria.CategoryName))
                .ForMember(dest => dest.CriteriaDescription, opt => opt.MapFrom(src => src.BlacklistCriteria.CategoryDescription))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<BlackList, BlacklistedProductsDto>()
                .ForMember(dest => dest.blacklistId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.CriteriaName, opt => opt.MapFrom(src => src.BlacklistCriteria.CategoryName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ReverseMap();

            CreateMap<ProductResponseDto, Product>().ReverseMap();
            CreateMap<ProductRequestDto, Product>().ReverseMap();
            CreateMap<GetProductsDto, Product>().ReverseMap();



        }

    }
}
