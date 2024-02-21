using Intercon.Application.Extensions;
using Intercon.Infrastructure.Extensions;
using Intercon.Infrastructure.Persistence;
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

var options = new DbContextOptionsBuilder<InterconDbContext>()
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .Options;

using (var context = new InterconDbContext(options))
{
    DataBaseSeeder.Seed(context);
}

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
