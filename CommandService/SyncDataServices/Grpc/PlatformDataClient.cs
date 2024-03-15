using AutoMapper;
using Grpc.Net.Client;
using WebApplication2.Models;
using WebApplication2.Protos;

namespace WebApplication2.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcPlatform"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);

            var client = new GrpcPlatform.GrpcPlatformClient(channel);

            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);

                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"--> Couldn't call GRPC Server {ex.ToString()}");
                return null;
            }
        }
    }
}
