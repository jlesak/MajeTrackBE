using System.Reflection;
using System.Text;
using MajeTrack.Api.Common;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting MajeTrack API application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddSerilog();
    
// TODO Add DbContext
// if (builder.Environment.IsDevelopment())
//     builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

    builder.Services.AddAuthentication()
        .AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                // TODO: Dodelat JWT konfigurace 
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                //ValidIssuer = configuration["Jwt:Issuer"],
                //ValidAudience = configuration["Jwt:Audience"]
            };
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
