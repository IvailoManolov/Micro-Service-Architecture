using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using WebApplication2.Data;
using WebApplication2.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private ICommandRepo _repository;
        private IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine("--> Getting commands for platform.");

            if(!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var currentCommand = _repository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(currentCommand));
        }

        [HttpGet("{commandId}", Name ="GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform : {platformId} / {commandId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _repository.GetCommand(platformId, commandId);

            if(command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandCreateDto> CreateCommandForPlatform(int platformId, CommandCreateDto command)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform : {platformId}");

            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandMapped = _mapper.Map<Command>(command);

            _repository.CreateCommand(platformId, commandMapped);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandMapped);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                    new { platformId = platformId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}
