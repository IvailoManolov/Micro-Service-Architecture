using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.AsyncDataServices;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.SyncDataServices.Http;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _dataClient;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo platformRepo,
            IMapper mapper,
            ICommandDataClient dataClient,
            IMessageBusClient messageBusClient)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
            _dataClient = dataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var platformItems = _platformRepo.GetAllPlatforms();

            // Map Platform to PlatformDTO.
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _platformRepo.GetPlatformById(id);

            if(platformItem != null)
            {
                // Map Platform to PlatformDTO.
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformTemplate)
        {
            var mappedPlatform = _mapper.Map<Platform>(platformTemplate);

            _platformRepo.CreatePlatform(mappedPlatform);
            _platformRepo.SaveChanges();

            var mappedResult = _mapper.Map<PlatformReadDto>(mappedPlatform);

            // Send sync message.
            try
            {
                await _dataClient.SendPlatformToCommand(mappedResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldn't send synchronous call! : {ex.Message}");
            }

            // Send Async message.

            try
            {
                var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(mappedResult);

                platformPublishedDto.Event = "Platform_Published";

                _messageBusClient.PublishNewPlatform(platformPublishedDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Couldn't send synchronous call! : {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = mappedResult.Id}, mappedResult);
        }
    }
}
