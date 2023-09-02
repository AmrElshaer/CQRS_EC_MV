using System.Text.Json;
using Application.Query;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationQuery(builder.Configuration);
var app = builder.Build();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

app.UseSwaggerGen();

app.Run();
