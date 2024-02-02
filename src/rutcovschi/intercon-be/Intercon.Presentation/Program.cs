using Intercon.Application.Extensions;
using Intercon.Infrastructure.Extensions;
using Intercon.Presentation.Extensions;
using Intercon.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var app = builder.Build();

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
