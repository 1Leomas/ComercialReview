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
    .AddPresentation(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    DataBaseSeeder.Seed(builder.Configuration.GetConnectionString("DefaultConnection"));
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
