using Application;
using Application.Common.models;
using CQRS_EC_MV;
using CQRS_EC_MV.Endpoints;
using CQRS_EC_MV.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(builder.Configuration, typeof(CQRS_EC_MVRoot).Assembly);
builder.Services.AddCustomVersioning();
builder.Services.AddApplication(builder.Configuration);
builder.AddMinimalEndpoints(assemblies: typeof(CQRS_EC_MVRoot).Assembly);
var app = builder.Build();
app.MapMinimalEndpoints();
app.Run();
