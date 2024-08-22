namespace Grpc.Server.Models;

public class TaskItem(string id, string title, string description)
{
    public string Id { get; private set; } = id;
    public string Title { get; private set; } = title;
    public string Description { get; private set; } = description;

    public void Update(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
