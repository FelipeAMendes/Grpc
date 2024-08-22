using Grpc.Core;
using Grpc.Server.Data.Repositories;
using Grpc.Server.Models;
using Grpc.Server.Protos;

namespace Grpc.Server.Services;

public partial class TaskService : Protos.TaskService.TaskServiceBase
{
    private readonly ITaskItemRepository _repository;

    public TaskService(ITaskItemRepository repository)
    {
        _repository = repository;
    }

    public override async Task<TaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var task = new TaskItem(Guid.NewGuid().ToString(), request.Title, request.Description);
        await _repository.AddAsync(task);

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description
        };
    }

    public override async Task<TaskResponse> GetTask(GetTaskRequest request, ServerCallContext context)
    {
        if (await _repository.GetByIdAsync(request.Id) is var task && task is not null)
        {
            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description
            };
        }

        throw new RpcException(new Status(StatusCode.NotFound, "Task not found"));
    }

    public override async Task<ListTasksResponse> ListTasks(ListTasksRequest request, ServerCallContext context)
    {
        var tasks = (await _repository.GetAllAsync()).Select(task => new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description
        }).ToList();

        return new ListTasksResponse { Tasks = { tasks } };
    }

    public override async Task<TaskResponse> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
    {
        if (await _repository.GetByIdAsync(request.Id) is var task && task is not null)
        {
            task.Update(request.Title, request.Description);
            await _repository.UpdateAsync(task);

            return new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description
            };
        }

        throw new RpcException(new Status(StatusCode.NotFound, "Task not found"));
    }

    public override async Task<Empty> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
    {
        if (await _repository.GetByIdAsync(request.Id) is var task && task is not null)
        {
            await _repository.RemoveAsync(task);
            return new Empty();
        }

        throw new RpcException(new Status(StatusCode.NotFound, "Task not found"));
    }
}
