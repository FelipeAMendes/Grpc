using Grpc.Client;
using Grpc.Net.Client;
using Grpc.Server.Protos;

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new ChatService.ChatServiceClient(channel);

var chatClient = new ChatClient(client, args.Length > 0 ? args[0] : "user");
await chatClient.StartChatAsync();
