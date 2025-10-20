using AutoMapper;
using nbpTracker.Model.Dto;
using nbpTracker.Model.Entities;

namespace nbpTracker.Common
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ExchangeTable, ExchangeTableDto>();

            CreateMap<CurrencyRate, CurrencyRateDto>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.CurrencyCode))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.Currency.CurrencyName));

            CreateMap<Currency, CurrencyRateDto>()
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.CurrencyName, opt => opt.MapFrom(src => src.CurrencyName))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Mid, opt => opt.Ignore());
        }
    }
}
