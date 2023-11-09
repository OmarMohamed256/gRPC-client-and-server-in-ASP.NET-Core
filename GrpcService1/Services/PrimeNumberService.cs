using Grpc.Core;
using Prime;
using static Prime.PrimeNumberService;

namespace GrpcService1.Services
{
    public class PrimeNumberService : PrimeNumberServiceBase
    {
        public override async Task PrimeNumberDecomposition(PrimeNumberDecompositionRequest request,
            IServerStreamWriter<PrimeNumberDecompositionResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("the server recived the request : ");
            Console.WriteLine(request.ToString());
            int number = request.Number;
            int divisor = 2;
            while (number > 0)
            {
                if(number % divisor == 0)
                {
                    number /= divisor;
                    await responseStream.WriteAsync(new PrimeNumberDecompositionResponse() { PrimeFactor = divisor });
                }
                else
                {
                    divisor++;
                }
            }
        }
    }
}
