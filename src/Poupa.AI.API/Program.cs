using Microsoft.EntityFrameworkCore;
using Poupa.AI.Infra.Data;
using Poupa.AI.IoC.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigrations(app);

await app.RunAsync();

static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PoupaAIDbContext>();
    dbContext.Database.Migrate();
}
