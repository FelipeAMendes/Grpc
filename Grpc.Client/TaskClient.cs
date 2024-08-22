using Grpc.Server.Protos;

namespace Grpc.Client;

public class TaskClient(TaskService.TaskServiceClient client)
{
    private readonly TaskService.TaskServiceClient _client = client;

    public async Task<TaskResponse> CreateTaskAsync()
    {
        var createResponse = await _client.CreateTaskAsync(new CreateTaskRequest
        {
            Title = "Sample Task",
            Description = "This is a sample task."
        });
        Console.WriteLine($"Created Task: {createResponse.Id}, {createResponse.Title}");
        return createResponse;
    }

    public async Task ListTasksAsync()
    {
        var listResponse = await _client.ListTasksAsync(new ListTasksRequest());
        foreach (var task in listResponse.Tasks)
        {
            Console.WriteLine($"Task: {task.Id}, {task.Title}, {task.Description}");
        }
    }

    public async Task GetTaskAsync(string id)
    {
        var getResponse = await _client.GetTaskAsync(new GetTaskRequest { Id = id });
        Console.WriteLine($"Got Task: {getResponse.Id}, {getResponse.Title}");
    }

    public async Task UpdateTaskAsync(string id)
    {
        var updateResponse = await _client.UpdateTaskAsync(new UpdateTaskRequest
        {
            Id = id,
            Title = "Updated Task",
            Description = "This task has been updated."
        });
        Console.WriteLine($"Updated Task: {updateResponse.Id}, {updateResponse.Title}");
    }

    public async Task DeleteTaskAsync(string id)
    {
        await _client.DeleteTaskAsync(new DeleteTaskRequest { Id = id });
        Console.WriteLine("Task deleted.");
    }
}
