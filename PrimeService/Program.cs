using Grpc.Net.Client;
using static PrimeNumberDecomposerClient.PrimeNumberService;

using var channel = GrpcChannel.ForAddress("https://localhost:7297");
var client = new PrimeNumberServiceClient(channel);
var response = 
    client.PrimeNumberDecomposition(new PrimeNumberDecomposerClient.PrimeNumberDecompositionRequest { Number = 120 });
while (await response.ResponseStream.MoveNext(new CancellationToken()))
{
    Console.WriteLine(response.ResponseStream.Current.PrimeFactor);
    await Task.Delay(2000);
}