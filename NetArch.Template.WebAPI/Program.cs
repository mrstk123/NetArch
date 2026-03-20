#if (IsClean)
using NetArch.Template.Application;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic;
#endif
using NetArch.Template.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
#if (IsClean)
builder.Services.AddApplicationServices();
#endif
#if (IsNTier)
builder.Services.AddBusinessLogicServices();
#endif
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

#if (IsAngular)
// Serve the Angular SPA static files in production
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
#endif

app.Run();
