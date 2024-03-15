using AutoMapper;
using WebApplication2.Dtos;
using WebApplication2.Models;
using WebApplication2.Protos;

namespace WebApplication2.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();

            CreateMap<PlatformPublishedDto, Platform>()
                .ForMember(
                destination => destination.ExternalID,
                opt => opt.MapFrom(src => src.Id)
                );

            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(destination => destination.ExternalID,
                    opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(destination => destination.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(destination => destination.Commands,
                    opt => opt.Ignore());
        }
    }
}
