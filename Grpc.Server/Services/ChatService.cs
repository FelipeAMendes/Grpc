using Grpc.Core;
using Grpc.Server.Protos;
using System.Collections.Concurrent;

namespace Grpc.Server.Services;

public class ChatService : Protos.ChatService.ChatServiceBase
{
    private static readonly ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>> _clients = new();

    public override async Task Chat(IAsyncStreamReader<ChatMessage> requestStream, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
    {
        _clients.TryAdd(context.Peer, responseStream);

        try
        {
            while (await requestStream.MoveNext())
            {
                var message = requestStream.Current;

                foreach (var client in _clients.Values)
                {
                    Console.WriteLine(message);
                    if (client != responseStream)
                    {
                        await client.WriteAsync(message);
                    }
                }
            }
        }
        finally
        {
            _clients.TryRemove(context.Peer, out _);
        }
    }
}
