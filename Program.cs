using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using ChurchSystem.Models;
using ChurchSystem.Services;


//services
var builder = WebApplication.CreateBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
ConfigureServices(builder.Services);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>

    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });

builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Church API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = " Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http,

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
     {    new OpenApiSecurityScheme
         {
             Reference = new OpenApiReference
             {
             Id = "Bearer",
             Type = ReferenceType.SecurityScheme
             }
         },
         new List<string>()
     }
  });
});

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseCors("CorsPolicy");
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
app.UseAuthorization();
app.UseAuthentication();
void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<ITitheService, TitheService>();
    services.AddTransient<IChildrenService, ChildrenService>();
    services.AddTransient<IYouthsService, YouthsService>();
    services.AddTransient<IAdultsService, AdultsService>();
    services.AddTransient<IAuthService, AuthService>();
    services.AddEntityFrameworkNpgsql()
                 .AddDbContext<APIContext>(
                     opt => opt.UseNpgsql(connectionString));

    services.AddCors(options =>
     {
         options.AddPolicy("CorsPolicy",
             builder => builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
     });

}

// Api Endpoints
// authentication endpoints
app.MapPost("/api/register", (UserRegistration user, IAuthService service) => Register(user, service));
app.MapPost("/api/login", (UserLogin user, IAuthService service) => Login(user, service));

// tithe endpoints

app.MapPost("/api/tithe", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Tithe tithe, ITitheService service) =>
{
    var result = service.Create(tithe);
    return Results.Ok(result);
});

app.MapGet("/api/tithe/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, ITitheService service) =>
{
    var tithe = service.Get(id);
    if (tithe is null) return Results.NotFound("Tithe not found");
    return Results.Ok(tithe);
});

app.MapGet("/api/tithe", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(ITitheService service) =>
{
    var tithe = service.List();
    return Results.Ok(tithe);
});

app.MapPut("/api/tithe/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Tithe tithe, ITitheService service) =>
{
    var updatedTithe = service.Update(id, tithe);
    if (updatedTithe is null) return Results.NotFound(" Tithe not found ");
    return Results.Ok(updatedTithe);
});

// app.MapDelete("/tithe", (int id, ITitheService service) =>
// {

//     var tithe = service.Delete(id);
//     if (!tithe) return Results.BadRequest(" Something went wrong ");
//     return Results.Ok(tithe);
// });

//children endpoints
app.MapPost("/api/children", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Children children, IChildrenService service) =>
{
    var result = service.Create(children);
    return Results.Ok(result);

});

app.MapGet("/api/children/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IChildrenService service) =>
{
    var children = service.Get(id);
    if (children is null) return Results.NotFound("Selected children attendance not found");
    return Results.Ok(children);
});

app.MapGet("/api/children", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IChildrenService service) =>
{
    var children = service.List();
    return Results.Ok(children);
});

app.MapPut("/api/children/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Children children, IChildrenService service) =>
{

    var updatedChildren = service.Update(id, children);
    if (updatedChildren is null) return Results.NotFound(" Selected children attendance not found");
    return Results.Ok(updatedChildren);
});

//youths endpoints
app.MapPost("/api/youths", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Youths youths, IYouthsService service) =>
{
    var result = service.Create(youths);
    return Results.Ok(result);

});

app.MapGet("/api/youths/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IYouthsService service) =>
{

    var youths = service.Get(id);
    if (youths is null) return Results.NotFound(" Selected youths attendance not found");
    return Results.Ok(youths);
});

app.MapGet("/api/youths", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IYouthsService service) =>
{
    var youths = service.List();
    return Results.Ok(youths);
});

app.MapPut("/api/youths/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Youths youths, IYouthsService service) =>
{
    var updatedYouths = service.Update(id, youths);
    if (updatedYouths is null) return Results.NotFound(" Selected youths attendance not found");
    return Results.Ok(updatedYouths);
});

//adults endpoints
app.MapPost("/api/adults", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Adults adults, IAdultsService service) =>
{
    var results = service.Create(adults);
    return Results.Ok(results);
});

app.MapGet("/api/adults/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IAdultsService service) =>
{
    var adults = service.Get(id);
    if (adults is null) return Results.NotFound(" Selected adults attendance not found");
    return Results.Ok(adults);

});

app.MapGet("/api/adults", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IAdultsService service) =>
{
    var adults = service.List();
    return Results.Ok(adults);
});

app.MapPut("/api/adults/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Adults adults, IAdultsService service) =>
{

    var updatedAdults = service.Update(id, adults);
    if (updatedAdults is null) return Results.NotFound(" Selected adults attendance not found");
    return Results.Ok(updatedAdults);
});

// end of endpoints //
async Task<IResult> Login(UserLogin user, IAuthService service)
{
    if (string.IsNullOrEmpty(user.UserName))

    {
        return Results.BadRequest(new { message = "Email is incorrect" });
    }
    else if (string.IsNullOrEmpty(user.Password))
    {
        return Results.BadRequest(new { message = "Password is incorrect" });
    }

    User loggedInUser = await service.Login(user.UserName, user.Password);

    if (loggedInUser != null) return Results.Ok(loggedInUser);

    return Results.BadRequest(new { message = "User login failed" });
}

async Task<IResult> Register(UserRegistration user, IAuthService service)
{
    if (string.IsNullOrEmpty(user.Name))
    {
        return Results.BadRequest(new { message = "Name needs to be provided" });
    }
    else if (string.IsNullOrEmpty(user.UserName))
    {
        return Results.BadRequest(new { message = "User name needs to be provided" });
    }
    else if (string.IsNullOrEmpty(user.Password))
    {
        return Results.BadRequest(new { message = "Password needs to be provided" });
    }

    User userToRegister = new(user.UserName, user.Name, user.Password, user.Role);
    User registeredUser = await service.Register(userToRegister);
    User loggedInUser = await service.Login(registeredUser.UserName, user.Password);

    if (loggedInUser != null) return Results.Ok(loggedInUser);
    return Results.BadRequest(new { message = "User registration failed" });
}

app.Run();


