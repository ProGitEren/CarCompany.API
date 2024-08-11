
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastucture.Data;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using CarCompany.API.MiddleWare;
using Microsoft.OpenApi.Models;
using Infrastucture.Configuration;

var builder = WebApplication.CreateBuilder(args);


//Configure serilog
builder.SeriLogConfiguration();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddApiReguestration();

builder.Services.AddSwaggerGen(s =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWt Auth Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme,

        }
    };
    s.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
    s.AddSecurityRequirement(securityRequirement);
});



//builder.Services.AddEndpointsApiExplorer();
builder.Services.InfraStructureConfiguration(builder.Configuration);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Create Async action is asynchronous
await app.SeedRoles();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

//Seed The first User Admin
InfraStructureRequistration.InfraStructureConfigurationMiddleware(app);

app.Run();

