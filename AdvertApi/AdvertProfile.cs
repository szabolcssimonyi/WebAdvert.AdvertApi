using AdvertApi.Entitiess;
using AdvertApi.Models;
using AutoMapper;
using System;

namespace AdvertApi
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<AdvertModel, AdvertEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source=>Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreationDateTime, opt => opt.MapFrom(source => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => AdvertStatus.Pending));
        }
    }
}
