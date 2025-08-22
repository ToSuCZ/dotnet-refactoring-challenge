using RefactoringChallenge.Application;
using RefactoringChallenge.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplication(builder.Configuration);

WebApplication app = builder.Build();

app.MapControllers();

await app.RunAsync();

#pragma warning disable S1118
public partial class Program { }
#pragma warning restore S1118
