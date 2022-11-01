using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ChurchSystem.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["secretConnectionString"];
ConfigureServices(builder.Services);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Church API", Version = "v1" });
});

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));


// Api Endpoints
// tithe endpoints
app.MapPost("/tithe", (Tithe tithe, ITitheService service) =>
{
    var result = service.Create(tithe);
    return Results.Ok(result);
});

app.MapGet("/tithe:{id}", (int id, ITitheService service) =>
{
    var tithe = service.Get(id);
    if (tithe is null) return Results.NotFound("Tithe not found");
    return Results.Ok(tithe);
});

app.MapGet("/tithe", (ITitheService service) =>
{
    var tithe = service.List();
    return Results.Ok(tithe);
});

app.MapPut("/tithe", (Tithe tithe, ITitheService service) =>
{
    var updatedTithe = service.Update(tithe);
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
app.MapPost("/children", (Children children, IChildrenService service) =>
{
    var result = service.Create(children);
    return Results.Ok(result);

});

app.MapGet("/children:{id}", (int id, IChildrenService service) =>
{
    var children = service.Get(id);
    if (children is null) return Results.NotFound("children attendance not found");
    return Results.Ok(children);
});

app.MapGet("/children", (IChildrenService service) =>
{
    var children = service.List();
    return Results.Ok(children);
});

app.MapPut("/children:{id}", (int id, Children children, IChildrenService service) =>
{

    var updatedChildren = service.Update(id, children);
    if (updatedChildren is null) return Results.NotFound(" Selected children attendance not found");
    return Results.Ok(updatedChildren);
});



app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<ITitheService, TitheService>();
    services.AddTransient<IChildrenService, ChildrenService>();
    services.AddEntityFrameworkNpgsql()
               .AddDbContext<APIContext>(
                   opt => opt.UseNpgsql(connectionString));

}