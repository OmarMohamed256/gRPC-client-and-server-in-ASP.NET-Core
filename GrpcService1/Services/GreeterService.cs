using Grpc.Core;
using GrpcService1;

namespace GrpcService1.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<GreetReply> Greet(GreetRequest request, ServerCallContext context)
        {
            string result = String.Format("hello {0} {1}", request.Greeting.FirstName,
                request.Greeting.LastName);
            return Task.FromResult(new GreetReply
            {
                Result = result,
            });
        }
        public override async Task GreetManyTimes(GreetManyTimesRequest request, IServerStreamWriter<GreetManyTimesReply> responseStream, ServerCallContext context)
        {
            Console.WriteLine("the server recived the request : ");
            Console.WriteLine(request.ToString());

            string result = String.Format("hello {0} {1}", request.Greeting.FirstName,
                request.Greeting.LastName);
            foreach(int i in Enumerable.Range(1, 10))
            {
                await responseStream.WriteAsync(new GreetManyTimesReply() { Result = result });
            }
        }
        public override async Task<LongGreetReply> LongGreet(IAsyncStreamReader<LongGreetRequest> requestStream,
            ServerCallContext context)
        {
            string result = "";

            while (await requestStream.MoveNext())
            {
                result += String.Format("Hello {0} {1} {2}", 
                    requestStream.Current.Greeting.FirstName, requestStream.Current.Greeting.LastName,
                    Environment.NewLine);
            }
            return new LongGreetReply { Result = result };
        }
    }
}