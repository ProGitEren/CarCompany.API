using Infrastucture.Services.Background_Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Configuration
{
    public static class BackgroundServiceRequistration
    {
        public static IServiceCollection BackgroundServiceConfiguration(this IServiceCollection services) 
        {
            services.AddHostedService<VehicleOwnershipTransferService>();

            return services;
        
        }

    }
}
