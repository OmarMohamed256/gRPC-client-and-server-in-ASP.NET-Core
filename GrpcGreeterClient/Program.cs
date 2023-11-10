using Grpc.Net.Client;
using GrpcGreeterClient;

using var channel = GrpcChannel.ForAddress("https://localhost:7297");
var client = new Greeter.GreeterClient(channel);
Greeting greeting = new () { FirstName = "omar", LastName = "mohamed" };
//var reply = await client.GreetAsync(
//                  new GreetRequest { Greeting = greeting });
//var request = new GreetManyTimesRequest() { Greeting = greeting };
//var response = client.GreetManyTimes(request);

// Console.WriteLine("Greeting: " + reply.Result);
//while (await response.ResponseStream.MoveNext(new CancellationToken())) 
//{
//    Console.WriteLine(response.ResponseStream.Current.Result);
//    await Task.Delay(2000);
//}

var request = new LongGreetRequest() { Greeting = greeting };
var stream = client.LongGreet();
foreach (int i in Enumerable.Range(1, 10))
{
    await stream.RequestStream.WriteAsync(request);
}

await stream.RequestStream.CompleteAsync();
var response = await stream.ResponseAsync;
Console.WriteLine(response.Result);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();