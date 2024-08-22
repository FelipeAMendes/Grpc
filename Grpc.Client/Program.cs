using Grpc.Net.Client;
using Grpc.Server.Protos;

using var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new TaskService.TaskServiceClient(channel);

TaskResponse createResponse = await CreateTaskAsync(client);

await ListTasksAsync(client);

await GetTaskAsync(client, createResponse);

await UpdateTaskAsync(client, createResponse);

await DeleteTaskAsync(client, createResponse);

//
// ----------------------------------------------------------------------------------
//

static async Task<TaskResponse> CreateTaskAsync(TaskService.TaskServiceClient client)
{
    var createResponse = await client.CreateTaskAsync(new CreateTaskRequest
    {
        Title = "Sample Task",
        Description = "This is a sample task."
    });
    Console.WriteLine($"Created Task: {createResponse.Id}, {createResponse.Title}");
    return createResponse;
}

static async Task ListTasksAsync(TaskService.TaskServiceClient client)
{
    var listResponse = await client.ListTasksAsync(new ListTasksRequest());
    foreach (var task in listResponse.Tasks)
    {
        Console.WriteLine($"Task: {task.Id}, {task.Title}, {task.Description}");
    }
}

static async Task GetTaskAsync(TaskService.TaskServiceClient client, TaskResponse createResponse)
{
    var getResponse = await client.GetTaskAsync(new GetTaskRequest { Id = createResponse.Id });
    Console.WriteLine($"Got Task: {getResponse.Id}, {getResponse.Title}");
}

static async Task UpdateTaskAsync(TaskService.TaskServiceClient client, TaskResponse createResponse)
{
    var updateResponse = await client.UpdateTaskAsync(new UpdateTaskRequest
    {
        Id = createResponse.Id,
        Title = "Updated Task",
        Description = "This task has been updated."
    });
    Console.WriteLine($"Updated Task: {updateResponse.Id}, {updateResponse.Title}");
}

static async Task DeleteTaskAsync(TaskService.TaskServiceClient client, TaskResponse createResponse)
{
    await client.DeleteTaskAsync(new DeleteTaskRequest { Id = createResponse.Id });
    Console.WriteLine("Task deleted.");
}