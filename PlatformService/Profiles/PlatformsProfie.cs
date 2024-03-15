using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Protos;

namespace WebApplication1.Profiles
{
    public class PlatformsProfie : Profile
    {
        public PlatformsProfie()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();

            CreateMap<Platform, GrpcPlatformModel>()
                    .ForMember(destination => destination.PlatformId,
                        opt => opt.MapFrom(src => src.Id))
                    .ForMember(destination => destination.Name,
                        opt => opt.MapFrom(src => src.Name))
                    .ForMember(destination => destination.Publisher,
                        opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
