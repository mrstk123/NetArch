#if (IsClean || IsNTier)
#if (IsClean)
using NetArch.Template.Application;
#endif
#if (IsNTier)
using NetArch.Template.BusinessLogic;
#endif
using NetArch.Template.Infrastructure;
using NetArch.Template.WebAPI.Middlewares;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
#if (IsClean)
builder.Services.AddApplicationServices();
#endif
#if (IsNTier)
builder.Services.AddBusinessLogicServices();
#endif
builder.Services.AddInfrastructureServices(builder.Configuration);
#if (IsAngular)
const string CorsPolicy = "AngularDev";
builder.Services.AddCors(options => options.AddPolicy(CorsPolicy, policy =>
    policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()));
#endif

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

#if (IsAngular)
// Allow the Angular dev server (http://localhost:4200) to call this API during development.
app.UseCors(CorsPolicy);

// Serve the Angular SPA static files in production
app.UseStaticFiles();
#endif

app.MapControllers();

#if (IsAngular)
app.MapFallbackToFile("index.html");
#endif

app.Run();
#else
// Raw template source fallback: the real pipeline above is finalized
// when a project is generated with a chosen architecture.
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
var app = builder.Build();
app.MapGet("/health", () => Results.Ok("NetArch template source"));
app.Run();
#endif
