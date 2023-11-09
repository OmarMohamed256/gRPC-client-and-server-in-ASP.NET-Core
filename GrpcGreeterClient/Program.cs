using Grpc.Net.Client;
using GrpcGreeterClient;

using var channel = GrpcChannel.ForAddress("https://localhost:7297");
var client = new Greeter.GreeterClient(channel);
Greeting greeting = new ()
{
    FirstName = "omar",
    LastName = "mohamed"
};
var reply = await client.GreetAsync(
                  new GreetRequest { Greeting = greeting });
Console.WriteLine("Greeting: " + reply.Result);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();