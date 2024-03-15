using AutoMapper;
using Grpc.Core;
using WebApplication1.Data;
using WebApplication1.Protos;

namespace WebApplication1.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private IPlatformRepo _repository;
        private IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo repository,
                IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();

            var platforms = _repository.GetAllPlatforms(); 

            foreach (var platform in platforms )
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}