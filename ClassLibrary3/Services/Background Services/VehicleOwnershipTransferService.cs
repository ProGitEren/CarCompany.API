using ClassLibrary2.Entities;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;
using static Models.Enums.OrderEnums;

namespace Infrastucture.Services.Background_Services
{
    public class VehicleOwnershipTransferService : BackgroundService
    { 
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Serilog.ILogger _logger;

        public VehicleOwnershipTransferService(IServiceScopeFactory serviceScopeFactory,Serilog.ILogger logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await TransferVehicleOwnershipAsync();
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private async Task TransferVehicleOwnershipAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUsers>>();

                var SoldOrders = await unitOfWork.OrderRepository.GetSoldOrdersAsync();

                foreach (var order in SoldOrders)
                {
                    try
                    {
                        var seller = await userManager.FindByEmailAsync(order.SellerEmail);
                        var buyer = await userManager.FindByEmailAsync(order.BuyerEmail);
                        var orderVehicle = await unitOfWork.OrderVehicleRepository.GetByIdAsync(order.OrderVehicleId);
                        var vehicle = await unitOfWork.VehicleRepository.GetByIdAsync(orderVehicle.VehicleId);

                        if (vehicle != null && buyer != null)
                        {
                            vehicle.UserId = buyer.Id;
                            await unitOfWork.VehicleRepository.UpdateAsync(vehicle);

                            order.IsVehicleOwnerTransferred = true;
                            await unitOfWork.OrderRepository.UpdateAsync(order);

                            _logger.Information($"Ownership transferred from {seller.Email} to {buyer.Email} for Order ID {order.Id} and Vehicle ID {vehicle.Vin}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, $"Error transferring ownership for Order ID {order.Id}");
                    }
                }
            }
        }



    }
}
