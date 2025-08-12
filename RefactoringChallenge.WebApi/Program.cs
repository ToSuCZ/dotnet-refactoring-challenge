using RefactoringChallenge.Infrastructure;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

IHost host = builder.Build();

await host.RunAsync();
