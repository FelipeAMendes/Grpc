using Grpc.Core;
using Grpc.Server.Protos;

namespace Grpc.Client;

public class ChatClient(ChatService.ChatServiceClient client, string clientName)
{
    private readonly ChatService.ChatServiceClient _client = client;

    public async Task StartChatAsync()
    {
        using var call = _client.Chat();
        var responseReader = Task.Run(async () =>
        {
            while (await call.ResponseStream.MoveNext())
            {
                var message = call.ResponseStream.Current;
                Console.WriteLine($"{message.User}: {message.Message}");
            }
        });

        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrEmpty(line)) break;

            await call.RequestStream.WriteAsync(new ChatMessage
            {
                User = clientName,
                Message = line
            });
        }

        await call.RequestStream.CompleteAsync();
        await responseReader;
    }
}
