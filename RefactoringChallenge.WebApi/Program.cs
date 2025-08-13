using RefactoringChallenge.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

app.MapControllers();

await app.RunAsync();
