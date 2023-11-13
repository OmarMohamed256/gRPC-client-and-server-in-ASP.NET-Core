using Grpc.Net.Client;
using GrpcGreeterClient;

using var channel = GrpcChannel.ForAddress("https://localhost:7297");
var client = new Greeter.GreeterClient(channel);
await DoGreetEveryone(client);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

static void DoSimpleGreet(Greeter.GreeterClient client)
{
    var greeting = new Greeting()
    {
        FirstName = "omar",
        LastName = "mohamed"
    };



    var request = new GreetRequest { Greeting = greeting };
    var response = client.Greet(request);

    Console.WriteLine(response.Result);
}

static async Task DoManyGreetings(Greeter.GreeterClient client)
{
    var greeting = new Greeting()
    {
        FirstName = "omar",
        LastName = "mohamed"
    };

    var request = new GreetManyTimesRequest() { Greeting = greeting };
    var response = client.GreetManyTimes(request);

    while (await response.ResponseStream.MoveNext(new CancellationToken()))
    {
        Console.WriteLine(response.ResponseStream.Current.Result);
        await Task.Delay(200);
    }
}

static async Task DoLongGreet(Greeter.GreeterClient client)
{
    var greeting = new Greeting()
    {
        FirstName = "omar",
        LastName = "mohamed"
    };

    var request = new LongGreetRequest() { Greeting = greeting };
    var stream = client.LongGreet();

    foreach (int i in Enumerable.Range(1, 10))
    {
        await stream.RequestStream.WriteAsync(request);
    }

    await stream.RequestStream.CompleteAsync();

    var response = await stream.ResponseAsync;

    Console.WriteLine(response.Result);
}

static async Task DoGreetEveryone(Greeter.GreeterClient client)
{
    var stream = client.GreetEveryone();
    var responseReaderTask = Task.Run(async () =>
    {
        while(await stream.ResponseStream.MoveNext(new CancellationToken()))
        {
            Console.WriteLine("Recieved : " + stream.ResponseStream.Current.Result);
        }
    });
    Greeting[] greetings = new Greeting[]
    {
        new Greeting() { FirstName = "omar", LastName = "mohamed" },
        new Greeting() { FirstName = "john", LastName = "doe" },
        new Greeting() { FirstName = "Jean", LastName = "john" }
    };
    foreach(Greeting greeting in greetings)
    {
        Console.WriteLine("Sending : " + greeting.ToString());
        await stream.RequestStream.WriteAsync(new GreetEveryoneRequest()
        {
            Greeting = greeting
        });
    }
    await stream.RequestStream.CompleteAsync();
    await responseReaderTask;
}