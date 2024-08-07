using Infrastucture.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Configuration
{
    public static class APIRequestration
    {

        public static IServiceCollection AddApiReguestration(this IServiceCollection services)
        {
            //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); // ensured all the files are connected

            //FileProvide
            //services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
            //    Directory.GetCurrentDirectory(), "wwwroot"
            //    )));


            // The response when model validation fails in the API VERY IMPORTANT

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = context.ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .SelectMany(x => x.Value.Errors)
                                .Select(x => x.ErrorMessage).ToArray()
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //Enable CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", pol =>
                {
                    pol.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(
                        "http://localhost:5062",
                       "https://localhost:7064",
                       "http://localhost:16491",
                       "https://localhost:44351"
                       );
                });
            });
            return services;
        }
    }
}
