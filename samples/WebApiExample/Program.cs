using MediatR.ParallelPublisher;
using Microsoft.EntityFrameworkCore;
using WebApiExample.Database;
using WebApiExample.Notifications;
using WebApiExample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MovieDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IMoviesService, MoviesService>();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<EntityAddedNotification>();
    configuration.UseParallelNotificationPublisher(builder.Services, options => options.RegisterExceptionHandler<MyNotificationExceptionHandler>());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
