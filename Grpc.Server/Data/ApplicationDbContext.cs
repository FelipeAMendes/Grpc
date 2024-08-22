using Grpc.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Grpc.Server.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> TaskItem { get; set; }
}
