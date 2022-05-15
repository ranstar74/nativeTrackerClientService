using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using nativeTrackerClientService.Credentials;
using nativeTrackerClientService.Services;

var builder = WebApplication.CreateBuilder();

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireClaim(ClaimTypes.Name);
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateActor = false,
                ValidateLifetime = true,
                IssuerSigningKey = AuthorizationManager.SecurityKey
            };
    });

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<TrackingService>();
app.MapGrpcService<VehicleService>();
app.MapGrpcService<ClientService>();

// app.MapGet("/authorize", async context =>
// {
//     string login = context.Request.Query["name"];
//     string password = context.Request.Query["password"];
//
//     (bool authorized, string token) = await AuthorizationManager.GenerateJwtToken(login, password);
//     if (authorized)
//     {
//         await context.Response.WriteAsync(token);
//     }
//     else
//     {
//         context.Response.StatusCode = StatusCodes.Status400BadRequest;
//     }
// });

app.Run();