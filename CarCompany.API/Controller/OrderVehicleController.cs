using AutoMapper;
using ClassLibrary2.Entities;
using FluentValidation;
using Infrastucture.DTO.DTO_OrderVehicles;
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
public class OrderVehicleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly Serilog.ILogger _logger;
    private readonly UserManager<AppUsers> _userManager;
    private readonly IValidator<OrderVehicle> _validator;

    public OrderVehicleController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager, IValidator<OrderVehicle> validator)
    {
        _logger = logger;
        _uow = uow;
        _mapper = mapper;
        _userManager = userManager;
        _validator = validator;
    }

    [HttpPost("create-ordervehicle")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> CreateAsync([FromForm] CreateOrderVehicleDto dto)
    {
        _logger.Information("Creating order vehicle with data: {@OrderVehicleData}", dto);
        try
        {
            var createtuple = await _uow.OrderVehicleRepository.AddAsync(dto, _validator);
            var ordervehicle = createtuple.Item1;
            var list = createtuple.Item2;

            if (list.Any())
            {
                _logger.Warning("Order vehicle creation failed due to validation errors: {@Errors}", list);
                return BadRequest(new ApiValidationErrorResponse { Errors = list });
            }

            _logger.Information("Order vehicle created successfully: {@OrderVehicle}", ordervehicle);
            var modeldto = _mapper.Map<OrderVehicleDto>(ordervehicle);
            return Ok(modeldto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while creating the order vehicle.");
            throw new Exception(ex.Message);
        }
    }

    [HttpGet("get-ordervehicle/{Id}")]
    [Authorize]
    public async Task<IActionResult> GetOrderVehicleAsync(int? Id)
    {
        _logger.Information("Retrieving order vehicle with Id: {OrderVehicleId}", Id);
        var ordervehicle = await _uow.OrderVehicleRepository.GetByIdAsync(Id);

        if (ordervehicle == null)
        {
            _logger.Warning("Order vehicle with Id: {OrderVehicleId} could not be found in the system.", Id);
            return NotFound(new ApiException(404, "Order vehicle not found in the system."));
        }

        _logger.Information("Order vehicle retrieved successfully: {@OrderVehicle}", ordervehicle);
        var dto = _mapper.Map<OrderVehicleDto>(ordervehicle);
        return Ok(dto);
    }

    [HttpPut("update-ordervehicle")]
    [Authorize]
    public async Task<IActionResult> UpdateOrderVehicleAsync(UpdateOrderVehicleDto dto)
    {
        _logger.Information("Updating order vehicle with Id: {OrderVehicleId}, Data: {@OrderVehicleData}", dto.Id, dto);
        try
        {
            var updatetuple = await _uow.OrderVehicleRepository.UpdateAsync(dto, _validator);
            var ordervehicle = updatetuple.Item1;
            var list = updatetuple.Item2;

            if (list.Any())
            {
                _logger.Warning("Order vehicle update failed due to validation errors: {@Errors}", list);
                return BadRequest(new ApiValidationErrorResponse { Errors = list });
            }

            if (ordervehicle == null)
            {
                _logger.Warning("Order vehicle not found in the system during update with Id: {OrderVehicleId}", dto.Id);
                return NotFound(new ApiException(404, "Order vehicle not found in the system."));
            }

            _logger.Information("Order vehicle updated successfully: {@OrderVehicle}", ordervehicle);
            var modeldto = _mapper.Map<OrderVehicleDto>(ordervehicle);
            return Ok(modeldto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while updating the order vehicle with Id: {OrderVehicleId}", dto.Id);
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("delete-ordervehicle/{Id}")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> DeleteOrderVehicleAsync(int? Id)
    {
        _logger.Information("Deleting order vehicle with Id: {OrderVehicleId}", Id);
        var ordervehicle = await _uow.OrderVehicleRepository.GetByIdAsync(Id);

        if (ordervehicle == null)
        {
            _logger.Warning("Order vehicle with Id: {OrderVehicleId} could not be found in the system.", Id);
            return NotFound(new ApiException(404, "Order vehicle not found in the system."));
        }

        try
        {
            if (await _uow.OrderVehicleRepository.DeleteAsync(Id))
            {
                _logger.Information("Order vehicle deleted successfully with Id: {OrderVehicleId}", Id);
                return Ok("Order vehicle successfully deleted.");
            }

            _logger.Warning("Order vehicle deletion failed with Id: {OrderVehicleId}", Id);
            return BadRequest(new ApiException(404, "The delete action failed."));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while deleting the order vehicle with Id: {OrderVehicleId}", Id);
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("delete-file")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> DeleteFileAsync([FromQuery] DeleteFileDto dto)
    {
        _logger.Information("Deleting file with data: {@FileData}", dto);
        var result = await _uow.OrderVehicleRepository.DeleteFileAsync(dto);

        if (result.Succeeded)
        {
            _logger.Information("File deleted successfully: {@FileData}", dto);
            return Ok("The file successfully deleted.");
        }

        _logger.Warning("File deletion failed: {@FileData}", dto);
        return NotFound(new ApiException(404, result.Errors.FirstOrDefault()?.Description ?? "File deletion error"));
    }

    [HttpPost("add-file")]
    [Authorize(Roles = "Admin,Seller")]
    public async Task<IActionResult> AddFileAsync([FromForm] AddFileDto dto)
    {
        _logger.Information("Adding file with data: {@FileData}", dto);
        var result = await _uow.OrderVehicleRepository.AddFileAsync(dto);

        if (result.Succeeded)
        {
            _logger.Information("File added successfully: {@FileData}", dto);
            return Ok("The file successfully created.");
        }

        _logger.Warning("File addition failed: {@FileData}", dto);
        return NotFound(new ApiException(404, result.Errors.FirstOrDefault()?.Description ?? "File addition error"));
    }

    [HttpGet("get-ordervehicles")]
    [Authorize]
    public async Task<IActionResult> GetAllPaginated([FromQuery] OrderVehicleParams orderVehicleParams)
    {
        _logger.Information("Retrieving paginated order vehicles. PageNumber: {PageNumber}, PageSize: {PageSize}", orderVehicleParams.PageNumber, orderVehicleParams.Pagesize);
        var src = await _uow.OrderVehicleRepository.GetAllAsync(orderVehicleParams);
        var ordervehicles = src.OrderVehicleDtos.ToList() as IReadOnlyList<OrderVehicleDto>;

        _logger.Information("Order vehicles retrieved successfully with total items: {TotalItems}", src.TotalItems);
        return Ok(new Pagination<OrderVehicleDto>(orderVehicleParams.Pagesize, orderVehicleParams.PageNumber, src.PageItemCount, src.TotalItems, ordervehicles));
    }
}