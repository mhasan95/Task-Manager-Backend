using Task_Manager_Backend.Models;
using Microsoft.AspNetCore.Builder;
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Task = Task_Manager_Backend.Models.Task;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("Tasks"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

//Serilog Logger Configuration *MiddleWare
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSerilogRequestLogging();

app.MapGet("/tasks", async (AppDbContext db) =>
    await db.Tasks.ToListAsync());

app.MapGet("/tasks/{id}", async (int id, AppDbContext db) =>
    await db.Tasks.FindAsync(id)
        is Task task
            ? Results.Ok(task)
            : Results.NotFound());

app.MapPost("/tasks/{id}", async (int id, Task task, AppDbContext db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return Results.Created($"/tasks/{id}", task);
});


app.MapPut("/tasks/{id}", async (int id, Task task, AppDbContext db) =>
{
    var tasklist = await db.Tasks.FindAsync(id);

    if (tasklist is null) return Results.NotFound();

    tasklist.Title = task.Title;
    tasklist.Description = task.Description;
    tasklist.Status = task.Status;
    tasklist.DueDate = task.DueDate;
    tasklist.Priority = task.Priority;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tasks/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Tasks.FindAsync(id) is Task task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.UseHttpsRedirection();
app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
