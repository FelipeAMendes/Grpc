using Grpc.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Grpc.Server.Data.Repositories;

public interface ITaskItemRepository
{
    Task<TaskItem> GetByIdAsync(string id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task AddAsync(TaskItem TaskItem);
    Task UpdateAsync(TaskItem TaskItem);
    Task RemoveAsync(TaskItem TaskItem);
}

public class TaskItemRepository(ApplicationDbContext context) : ITaskItemRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<TaskItem> GetByIdAsync(string id)
    {
        return await _context.Set<TaskItem>().FindAsync(id);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Set<TaskItem>().ToListAsync();
    }

    public async Task AddAsync(TaskItem TaskItem)
    {
        await _context.Set<TaskItem>().AddAsync(TaskItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem TaskItem)
    {
        _context.Set<TaskItem>().Update(TaskItem);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(TaskItem TaskItem)
    {
        _context.Set<TaskItem>().Remove(TaskItem);
        await _context.SaveChangesAsync();
    }
}
