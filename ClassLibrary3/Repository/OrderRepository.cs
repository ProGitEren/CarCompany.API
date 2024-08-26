using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.DTO.DTO_Orders;
using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.Errors;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Validation;
using static Models.Enums.OrderEnums;

namespace Infrastucture.Repository
{
    public class OrderRepository : GenericRepository<Order, int?>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        public OrderRepository(ApplicationDbContext context, IMapper mapper, UserManager<AppUsers> userManager) : base(context)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Tuple<Order, List<string>>> AddAsync(CreateOrderDto dto, Func<CreateOrderVehicleDto, Task<Tuple<OrderVehicle, List<string>>>> orderVehicleFunc)
        {
            var ordererrorlist = new List<string>();
            if (dto is not null)
            {
                var buyer = await _userManager.FindByEmailAsync(dto.BuyerEmail);
                var seller = await _userManager.FindByEmailAsync(dto.SellerEmail);

                if (buyer != null)
                {
                    var buyerwithrole = await _userManager.AddRolestoUserAsync(buyer);
                    if (!buyerwithrole.roles.Contains("Buyer")) { ordererrorlist.Add("The entered user does not have Buyer role in the system"); }
                }
                
                if (seller != null)
                {

                    var sellerwithrole = await _userManager.AddRolestoUserAsync(seller);
                    if (!sellerwithrole.roles.Contains("Seller")) { ordererrorlist.Add("The entered user does not have Seller role in the system"); }
                }
                else
                {
                    ordererrorlist.Add("Seller could not be found in the system");
                }


                if (dto.OrderVehicleDto is not null)
                {
                    var ordervehicle = new OrderVehicle();
                    try
                    {
                        if (!ordererrorlist.Any())
                        {
                            var createtuple = await orderVehicleFunc(dto.OrderVehicleDto);
                            ordervehicle = createtuple.Item1;
                            var list_1 = createtuple.Item2;
                            ordererrorlist.AddRange(list_1);
                            var ordervehicledto = _mapper.Map<OrderVehicleDto>(ordervehicle);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    try
                    {
                        var order = _mapper.Map<Order>(dto);
                        if (order != null)
                        {
                            order.OrderVehicleId = ordervehicle.Id;
                            ordererrorlist.AddRange(EntityValidator.GetValidationResults(order));

                            if (!ordererrorlist.Any())
                            {
                                await _context.Orders.AddAsync(order);
                                await _context.SaveChangesAsync();
                            }

                            return Tuple.Create(order, ordererrorlist);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                return Tuple.Create(_mapper.Map<Order>(dto), ordererrorlist);
            }
            return Tuple.Create(new Order(), ordererrorlist);

        }

        public async Task<Tuple<Order, List<string>>> UpdateAsync(UpdateOrderDto dto, Func<UpdateOrderVehicleDto, Task<Tuple<OrderVehicle, List<string>>>> orderVehicleFunc)
        {
            var ordererrorlist = new List<string>();
            if (dto is not null)
            {
                var buyer = await _userManager.FindByEmailAsync(dto.BuyerEmail);
                var seller = await _userManager.FindByEmailAsync(dto.SellerEmail);

                if (buyer != null)
                {
                    var buyerwithrole = await _userManager.AddRolestoUserAsync(buyer);
                    if (!buyerwithrole.roles.Contains("Buyer")) { ordererrorlist.Add("The entered user does not have Buyer role in the system"); }
                }
                else
                {
                    if (dto.OrderStatus == OrderStatus.Sold)
                        ordererrorlist.Add("Buyer must be specified for sold orders.");
                }
            
                if (seller != null)
                {

                    var sellerwithrole = await _userManager.AddRolestoUserAsync(seller);
                    if (!sellerwithrole.roles.Contains("Seller")) { ordererrorlist.Add("The entered user does not have Seller role in the system"); }
                }
                else
                {
                    ordererrorlist.Add("Seller could not be found in the system");
                }



                if (dto.OrderVehicleDto is not null)
                {
                    var ordervehicle = await _context.OrderVehicles.FindAsync(dto.OrderVehicleDto.Id);

                    try
                    {
                        if (!ordererrorlist.Any())
                        {
                            var updatetuple = await orderVehicleFunc(dto.OrderVehicleDto);
                            ordervehicle = updatetuple.Item1;
                            var list_1 = updatetuple.Item2;
                            ordererrorlist.AddRange(list_1);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    try
                    {

                        var order = await _context.Orders.FindAsync(dto.Id);
                        _mapper.Map(dto, order);


                        if (order != null)
                        {
                            order.OrderVehicleId = ordervehicle.Id;
                            var validationerrorlist = EntityValidator.GetValidationResults(order);

                            if (validationerrorlist.Any())
                            {
                                _context.Orders.Update(order);
                                await _context.SaveChangesAsync();
                            }

                            return Tuple.Create(order, ordererrorlist);

                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                return Tuple.Create(await _context.Orders.FindAsync(dto.Id), ordererrorlist);
            }
            return Tuple.Create(new Order(), ordererrorlist);

        }
        public async Task<bool> DeleteAsync(int? Id, Func<int?, Task<bool>> orderVehicleFunc)
        {

            if (Id is not null)
            {
                var order = await _context.Orders.FindAsync(Id);

                if (order == null) { return false; }

                try
                {
                    //Delete already configured as cascade therefore when order vehicle is deleted the order automatically gets deleted
                    if (await orderVehicleFunc(order.OrderVehicleId))
                        return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);

                }

            }
            return false;
        }


        public async Task<ParamsOrderDto> GetAllAsync(OrderParams orderParams)
        {
            var result = new ParamsOrderDto();
            var query = _context.Orders
                .Include(x => x.Vehicle)
                .AsNoTracking();
            //var query = await _context.Products
            //    .Include(x => x.Category)
            //    .AsNoTracking()
            //    .ToListAsync();
            

            //search by Name
            if (!string.IsNullOrEmpty(orderParams.Search))
                query = query.Where(x => x.Vehicle.ModelName.ToLower().Contains(orderParams.Search));

            //filtering 

            if (!string.IsNullOrEmpty(orderParams.VehicleId))
                query = query.Where(x => x.Vehicle.VehicleId == orderParams.VehicleId);
            
            if (!string.IsNullOrEmpty(orderParams.SellerEmail))
                query = query.Where(x => x.SellerEmail == orderParams.SellerEmail);
            
            if (!string.IsNullOrEmpty(orderParams.BuyerEmail))
                query = query.Where(x => x.BuyerEmail == orderParams.BuyerEmail);
            
            if (!string.IsNullOrEmpty(orderParams.OrderStatus))
                query = query.Where(x => x.OrderStatus == (OrderStatus)Enum.Parse(typeof(OrderStatus),orderParams.BuyerEmail));
            
            if (!string.IsNullOrEmpty(orderParams.OrderType))
                query = query.Where(x => x.OrderType == (OrderType)Enum.Parse(typeof(OrderType),orderParams.BuyerEmail));
          
            if (!string.IsNullOrEmpty(orderParams.PaymentMethod))
                query = query.Where(x => x.PaymentMethod == (PaymentMethod)Enum.Parse(typeof(PaymentMethod),orderParams.BuyerEmail));


            //sorting
            if (!string.IsNullOrEmpty(orderParams.Sorting))
            {
                query = orderParams.Sorting switch
                {
                    "PriceAsc" => query.OrderBy(x => x.Vehicle.Price),
                    "PriceDesc" => query.OrderByDescending(x => x.Vehicle.Price),
                    "VehicleAsc" => query.OrderBy(x => x.Vehicle.VehicleId),
                    "VehicleDesc" => query.OrderByDescending(x => x.Vehicle.VehicleId),
                    "OrderDateASc" => query.OrderBy(x => x.OrderedDate),
                    "OrderDateDesc" => query.OrderByDescending(x => x.OrderedDate),
                    "SellerEmailAsc" => query.OrderBy(x => x.SellerEmail),
                    "SellerEmailDesc" => query.OrderByDescending(x => x.SellerEmail),
                    "OrderType" => query.OrderBy(x => x.OrderType),
                    "OrderStatus" => query.OrderBy(x => x.OrderStatus),
                    _ => query.OrderBy(x => x.Vehicle.ModelName)
                };
            }

            //paging
            result.TotalItems = query.Count();
            query = query.Skip((orderParams.Pagesize) * (orderParams.PageNumber - 1)).Take(orderParams.Pagesize);

            var list = await query.ToListAsync(); // the execution will be done at the end
            result.OrderDtos = _mapper.Map<List<OrderDto>>(list);
            result.PageItemCount = list.Count;
            return result;
        }
    }
}
