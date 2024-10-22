using AutoMapper;
using ClassLibrary2.Entities;
using FluentValidation;
using Infrastucture.DTO.DTO_Orders;
using Infrastucture.Errors;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly Serilog.ILogger _logger;
    private readonly UserManager<AppUsers> _userManager;
    private readonly IValidator<Order> _orderValidator;
    private readonly IValidator<OrderVehicle> _orderVehicleValidator;

    public OrderController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager, IValidator<Order> orderValidator, IValidator<OrderVehicle> orderVehicleValidator)
    {
        _logger = logger;
        _uow = uow;
        _mapper = mapper;
        _userManager = userManager;
        _orderValidator = orderValidator;
        _orderVehicleValidator = orderVehicleValidator;
    }

    [HttpPost("create-order")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateOrderDto dto)
    {
        _logger.Information("Creating order with data: {@OrderData}", dto);
        try
        {
            var createtuple = await _uow.OrderRepository.AddAsync(dto, _uow.OrderVehicleRepository.AddAsync, _orderValidator, _orderVehicleValidator);
            var order = createtuple.Item1;
            var validationerrorlist = createtuple.Item2;

            if (validationerrorlist.Any())
            {
                _logger.Warning("Order creation failed due to validation errors: {@Errors}", validationerrorlist);
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            if (order == null)
            {
                _logger.Warning("Order not found in the system after creation.");
                return NotFound(new ApiException(404, "Order not found in the system."));
            }

            _logger.Information("Order created successfully: {@Order}", order);
            var modeldto = _mapper.Map<OrderDto>(order);
            return Ok(modeldto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating the order.");
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("get-order/{Id}")]
    [Authorize]
    public async Task<IActionResult> GetOrderAsync(int? Id)
    {
        _logger.Information("Retrieving order with Id: {OrderId}", Id);
        var order = await _uow.OrderRepository.GetByIdWithOrderVehicleAsync(Id);

        if (order == null)
        {
            _logger.Warning("Order with Id: {OrderId} could not be found in the system.", Id);
            return NotFound(new ApiException(404, "Order not found in the system."));
        }

        var dto = _mapper.Map<OrderDto>(order);
        _logger.Information("Order retrieved successfully: {@Order}", dto);
        return Ok(dto);
    }

    [HttpPut("update-order")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> UpdateOrderAsync(UpdateOrderDto dto)
    {
        _logger.Information("Updating order with Id: {OrderId}, Data: {@OrderData}", dto.Id, dto);
        try
        {
            var updatetuple = await _uow.OrderRepository.UpdateAsync(dto, _uow.OrderVehicleRepository.UpdateAsync, _orderValidator, _orderVehicleValidator);
            var order = updatetuple.Item1;
            var validationerrorlist = updatetuple.Item2;

            if (validationerrorlist.Any())
            {
                _logger.Warning("Order update failed due to validation errors: {@Errors}", validationerrorlist);
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            if (order == null)
            {
                _logger.Warning("Order not found in the system during update with Id: {OrderId}", dto.Id);
                return NotFound(new ApiException(404, "Order not found in the system."));
            }

            _logger.Information("Order updated successfully: {@Order}", order);
            var modeldto = _mapper.Map<OrderDto>(order);
            return Ok(modeldto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while updating the order with Id: {OrderId}", dto.Id);
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("delete-order/{Id}")]
    [Authorize(Roles = "Seller")]
    public async Task<IActionResult> DeleteOrderAsync(int? Id)
    {
        _logger.Information("Deleting order with Id: {OrderId}", Id);
        var order = await _uow.OrderRepository.GetByIdAsync(Id);

        if (order == null)
        {
            _logger.Warning("Order with Id: {OrderId} could not be found in the system.", Id);
            return NotFound(new ApiException(404, "Order not found in the system."));
        }

        try
        {
            if (await _uow.OrderRepository.DeleteAsync(Id, _uow.OrderVehicleRepository.DeleteAsync))
            {
                _logger.Information("Order deleted successfully with Id: {OrderId}", Id);
                return Ok("Order successfully deleted.");
            }
            return BadRequest(new ApiException(404, "Order delete action failed."));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while deleting the order with Id: {OrderId}", Id);
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("get-orders")]
    [Authorize]
    public async Task<IActionResult> GetAllPaginated([FromQuery] OrderParams orderParams)
    {
        _logger.Information("Retrieving paginated orders. PageNumber: {PageNumber}, PageSize: {PageSize}", orderParams.PageNumber, orderParams.Pagesize);
        var src = await _uow.OrderRepository.GetAllAsync(orderParams);
        var ordervehicles = src.OrderDtos.ToList() as IReadOnlyList<OrderDto>;

        _logger.Information("Orders retrieved successfully with total items: {TotalItems}", src.TotalItems);
        return Ok(new Pagination<OrderDto>(orderParams.Pagesize, orderParams.PageNumber, src.PageItemCount, src.TotalItems, ordervehicles));
    }
}