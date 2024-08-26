using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.DTO_Orders;
using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.Errors;
using Infrastucture.Extensions;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System.Linq.Expressions;
using WebAPI.Validation;
using static Models.Enums.OrderEnums;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly UserManager<AppUsers> _userManager;

        public OrderController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
        }


        //[HttpGet("get-all-ordervehicles")]
        //[Authorize]

        //public async Task<IActionResult> GetAll()
        //{
        //    var models = _uow.VehicleModelRepository.GetAll();
        //    _logger.Information("The Current User is retrieving.");
        //    if (models == null)
        //    {
        //        _logger.Warning("The Current User could not be found in the system.");
        //        return NotFound(new ApiException(404, "There is no model in the system."));
        //    }

        //    var dtos = _mapper.Map<IEnumerable<VehicleModelDto>>(models);
        //    return Ok(dtos);

        //}

        //[HttpGet("get-models")]
        //[Authorize]

        //public async Task<IActionResult> GetAllPaginated([FromQuery] VehiclemodelParams modelParams)
        //{
        //    var src = await _uow.VehicleModelRepository.GetAllAsync(modelParams);
        //    var models = src.VehicleModelDtos.ToList() as IReadOnlyList<VehicleModelDto>;

        //    return Ok(new Pagination<VehicleModelDto>(modelParams.Pagesize, modelParams.PageNumber, src.PageItemCount, src.TotalItems, models));
        //}



        [HttpPost("create-order")]
        [Authorize(Roles = "Seller")]

        public async Task<IActionResult> CreateAsync([FromForm] CreateOrderDto dto)
        {
            try
            {
                
                var createtuple = await _uow.OrderRepository.AddAsync(dto,_uow.OrderVehicleRepository.AddAsync);
                var order = createtuple.Item1;
                var validationerrorlist = createtuple.Item2;

                if (validationerrorlist.Any())
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist }); // no database operation
                }
                if (order == null)
                {
                    _logger.Warning("This order could not be found in the system.");
                    return NotFound(new ApiException(404, "This order did not found in the system."));
                }

                var modeldto = _mapper.Map<OrderDto>(order);
                return Ok(modeldto);

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("get-order/{Id}")]
        [Authorize]

        public async Task<IActionResult> GetOrderAsync(int? Id)
        {
            var order = await _uow.OrderRepository.GetByIdAsync(Id);
            _logger.Information("Order is being retrieved.");

            if (order == null)
            {
                _logger.Warning("This order could not be found in the system.");
                return NotFound(new ApiException(404, "This order did not found in the system."));
            }

            var dto = _mapper.Map<OrderDto>(order);

            return Ok(dto);

        }

        [HttpPut("update-order")]
        [Authorize(Roles = "Seller")]

        public async Task<IActionResult> UpdateOrderVehicleAsync([FromForm] UpdateOrderDto dto)
        {

            try
            {
                var updatetuple = await _uow.OrderRepository.UpdateAsync(dto, _uow.OrderVehicleRepository.UpdateAsync);
                var order = updatetuple.Item1;
                var validationerrorlist = updatetuple.Item2;

                if (validationerrorlist.Any())
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist }); // no database operation
                }

                if (order == null)
                {
                    _logger.Warning("This order could not be found in the system.");
                    return NotFound(new ApiException(404, "This order did not found in the system."));
                }
                if (order.OrderStatus == OrderStatus.Sold)
                {
                    var buyer = await _userManager.FindByEmailAsync(order.BuyerEmail);
                    var vehicle = await _uow.VehicleRepository.GetByIdAsync(order.Vehicle.VehicleId);
                    if (vehicle is null) { return BadRequest(new ApiValidationErrorResponse { Errors = ["Vehicle could not be found in the system."] }); }
                    vehicle.UserId = buyer.Id;
                    try
                    {
                        await _uow.VehicleRepository.UpdateAsync(vehicle);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                
                var modeldto = _mapper.Map<OrderDto>(order);

                return Ok(modeldto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("delete-order/{Id}")]
        [Authorize(Roles = "Seller")]

        public async Task<IActionResult> DeleteOrderAsync(int? Id)
        {

            var order = await _uow.OrderRepository.GetByIdAsync(Id);

            if (order == null)
            {
                _logger.Warning("This order could not be found in the system.");
                return NotFound(new ApiException(404, "This order did not found in the system."));
            }

            try
            {
                if (await _uow.OrderRepository.DeleteAsync(Id,_uow.OrderVehicleRepository.DeleteAsync))
                {
                    _logger.Information("Order is successfully deleted.");
                    return Ok("Order successfully deleted.");
                }
                return BadRequest(new ApiException(404, "The delete action failed."));
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occured while deleting the order.");
                throw new Exception(ex.Message);


            }
        }

        [HttpGet("get-orders")]
        [Authorize]

        public async Task<IActionResult> GetAllPaginated([FromQuery] OrderParams orderParams)
        {
            var src = await _uow.OrderRepository.GetAllAsync(orderParams);
            var ordervehicles = src.OrderDtos.ToList() as IReadOnlyList<OrderDto>;

            return Ok(new Pagination<OrderDto>(orderParams.Pagesize, orderParams.PageNumber, src.PageItemCount, src.TotalItems, ordervehicles));
        }

    }
}
