using Grpc.Server.Data;
using Grpc.Server.Data.Repositories;
using Grpc.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TasksDb"));
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();

var app = builder.Build();

app.MapGrpcService<TaskService>();
app.MapGet("/", () => "Hey!!");

app.Run();
