using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChurchSystem.Models;

//services
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["secretConnectionString"];
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
    services.AddTransient<IUserService, UserService>();
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
app.MapPost("/login", (UserLogin user, IUserService service) => Login(user, service));

// tithe endpoints
app.MapPost("/register", (User user, IUserService service) =>
{
    var result = service.Register(user);
    return Results.Ok(result);

});
app.MapPost("/tithe", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Tithe tithe, ITitheService service) =>
{
    var result = service.Create(tithe);
    return Results.Ok(result);
});

app.MapGet("/tithe/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, ITitheService service) =>
{
    var tithe = service.Get(id);
    if (tithe is null) return Results.NotFound("Tithe not found");
    return Results.Ok(tithe);
});

app.MapGet("/tithe", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(ITitheService service) =>
{
    var tithe = service.List();
    return Results.Ok(tithe);
});

app.MapPut("/tithe/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
app.MapPost("/children", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Children children, IChildrenService service) =>
{
    var result = service.Create(children);
    return Results.Ok(result);

});

app.MapGet("/children/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IChildrenService service) =>
{
    var children = service.Get(id);
    if (children is null) return Results.NotFound("Selected children attendance not found");
    return Results.Ok(children);
});

app.MapGet("/children", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IChildrenService service) =>
{
    var children = service.List();
    return Results.Ok(children);
});

app.MapPut("/children/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Children children, IChildrenService service) =>
{

    var updatedChildren = service.Update(id, children);
    if (updatedChildren is null) return Results.NotFound(" Selected children attendance not found");
    return Results.Ok(updatedChildren);
});

//youths endpoints
app.MapPost("/youths", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Youths youths, IYouthsService service) =>
{
    var result = service.Create(youths);
    return Results.Ok(result);

});

app.MapGet("/youths/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IYouthsService service) =>
{

    var youths = service.Get(id);
    if (youths is null) return Results.NotFound(" Selected youths attendance not found");
    return Results.Ok(youths);
});

app.MapGet("/youths", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IYouthsService service) =>
{
    var youths = service.List();
    return Results.Ok(youths);
});

app.MapPut("/youths/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Youths youths, IYouthsService service) =>
{
    var updatedYouths = service.Update(id, youths);
    if (updatedYouths is null) return Results.NotFound(" Selected youths attendance not found");
    return Results.Ok(updatedYouths);
});

//adults endpoints
app.MapPost("/adults", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(Adults adults, IAdultsService service) =>
{
    var results = service.Create(adults);
    return Results.Ok(results);
});

app.MapGet("/adults/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, IAdultsService service) =>
{
    var adults = service.Get(id);
    if (adults is null) return Results.NotFound(" Selected adults attendance not found");
    return Results.Ok(adults);

});

app.MapGet("/adults", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(IAdultsService service) =>
{
    var adults = service.List();
    return Results.Ok(adults);
});

app.MapPut("/adults/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
(int id, Adults adults, IAdultsService service) =>
{

    var updatedAdults = service.Update(id, adults);
    if (updatedAdults is null) return Results.NotFound(" Selected adults attendance not found");
    return Results.Ok(updatedAdults);
});

// end of endpoints //
IResult Login(UserLogin user, IUserService service)
{
    if (!string.IsNullOrEmpty(user.Username) &&
     !string.IsNullOrEmpty(user.Password)) ;

    {
        var loggedInUser = service.Get(user);
        if (loggedInUser is null) return Results.NotFound("User not found");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
            new  Claim(ClaimTypes.GivenName, loggedInUser.GivenName ),
            new Claim(ClaimTypes.Surname, loggedInUser.Surname),
            new Claim(ClaimTypes.Role, loggedInUser.Role),

        };
        var token = new JwtSecurityToken
        (
            issuer: builder.Configuration["jwt:Issuer"],
            audience: builder.Configuration["jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])),
                 SecurityAlgorithms.HmacSha256)

        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokenString);
    }
}

app.Run();


