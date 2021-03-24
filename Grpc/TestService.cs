using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace TF47_Backend.Grpc
{
    public class TestService : Greeter.GreeterBase
    {
        private readonly ILogger<TestService> _logger;

        public TestService(ILogger<TestService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Saying hello to {Name}", request.Name);
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }   
}