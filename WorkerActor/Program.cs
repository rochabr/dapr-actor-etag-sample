using WorkerActorN.Actors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();

builder.Services.AddActors(options =>
{
    Console.WriteLine("Adding actors...");
    // Register actor types and configure actor settings
    options.Actors.RegisterActor<WorkerActor>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapActorsHandlers();

app.Run();
