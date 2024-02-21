using Intercon.Application.Extensions;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence.DataSeeder;
using Intercon.Presentation.Extensions;
using Intercon.Presentation.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var app = builder.Build();

DataBaseSeeder.Seed(builder.Configuration.GetConnectionString("DefaultConnection"));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.UseRouting();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
