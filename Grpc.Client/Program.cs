using Grpc.Client;
using Grpc.Net.Client;
using Grpc.Server.Protos;

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new TaskService.TaskServiceClient(channel);

var taskClient = new TaskClient(client);

TaskResponse createResponse = await taskClient.CreateTaskAsync();

await taskClient.ListTasksAsync();

await taskClient.GetTaskAsync(createResponse.Id);

await taskClient.UpdateTaskAsync(createResponse.Id);

await taskClient.DeleteTaskAsync(createResponse.Id);
