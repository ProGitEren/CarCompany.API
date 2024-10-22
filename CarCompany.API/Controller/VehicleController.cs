using AutoMapper;
using ClassLibrary2.Entities;
using FluentValidation;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Errors;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Params;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Infrastucture.Extensions;

[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly Serilog.ILogger _logger;
    private readonly IVinGenerationService _vingenerationservice;
    private readonly UserManager<AppUsers> _userManager;
    private readonly IValidator<Vehicles> _validator;

    public VehicleController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, IVinGenerationService vinGenerationService, UserManager<AppUsers> userManager, IValidator<Vehicles> validator)
    {
        _logger = logger;
        _uow = uow;
        _mapper = mapper;
        _vingenerationservice = vinGenerationService;
        _userManager = userManager;
        _validator = validator;
    }

    [HttpPost("create-vehicle")]
    [Authorize]
    public async Task<IActionResult> CreateAsync(RegisterVehicleDto dto)
    {
        var user = await _userManager.FindEmailByClaimWithDetailAsync(User);
        _logger.Information("Retrieving current user.");
        if (user == null)
        {
            _logger.Warning("Current user could not be found in the system.");
            return NotFound(new ApiException(404, "The current user could not be found in the system."));
        }

        var vehicle = _mapper.Map<Vehicles>(dto);
        var validationResult = _validator.Validate(vehicle);
        if (validationResult.IsValid)
        {
            var vehiclemodel = await _uow.VehicleModelRepository.GetByIdAsync(dto.ModelId);
            var engine = _uow.EngineRepository.GetByEngineCode(vehiclemodel.EngineCode);

            vehicle.ModelId = vehiclemodel.Id;
            vehicle.ModelName = vehiclemodel.ModelShortName;
            vehicle.EngineId = engine.Id;
            vehicle.EngineName = engine.EngineName;
            vehicle.Vin = _vingenerationservice.GenerateVin(vehiclemodel);

            vehicle.UserId = User.IsInRole("Admin") ? null : user.Id;

            try
            {
                await _uow.VehicleRepository.AddAsync(vehicle);
                _logger.Information("Vehicle created successfully for user {Email}, Data: {@Vehicle}", user.Email, vehicle);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating vehicle.");
                throw new Exception(ex.Message);
            }

            vehiclemodel.Quantity = _uow.VehicleRepository.GetAll().Where(x => x.ModelId == vehiclemodel.Id).Count();
            try
            {
                await _uow.VehicleModelRepository.UpdateAsync(vehiclemodel);
                _logger.Information("Vehicle model quantity updated successfully for model {ModelId}.", vehiclemodel.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating vehicle model quantity.");
                throw new Exception(ex.Message);
            }

            var vehicledto = _mapper.Map<VehicleDto>(vehicle);
            return Ok(vehicledto);
        }

        _logger.Warning("Vehicle creation failed due to validation errors: {@Errors}", validationResult.Errors);
        return BadRequest(new ApiValidationErrorResponse(validationResult.Errors));
    }

    [HttpGet("vehicle-detail/{Email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> VehicleDetailAsync(string? Email)
    {
        _logger.Information("Retrieving vehicle details for user {Email}.", Email);
        var user = await _userManager.FindEmailByEmailAsync(Email);
        if (user == null)
        {
            _logger.Warning("User with email {Email} could not be found.", Email);
            return NotFound(new ApiException(404, "User could not be found in the system."));
        }

        var userdto = _mapper.Map<UserwithdetailsDto>(user);
        _logger.Information("Vehicle details retrieved successfully for user {Email}.", Email);
        return Ok(userdto);
    }

    [HttpGet("get-vehicles")]
    [Authorize]
    public async Task<IActionResult> GetAllPaginated([FromQuery] VehicleParams vehicleParams)
    {
        _logger.Information("Retrieving paginated vehicles. PageNumber: {PageNumber}, PageSize: {PageSize}", vehicleParams.PageNumber, vehicleParams.Pagesize);
        var src = await _uow.VehicleRepository.GetAllAsync(vehicleParams);
        var vehicles = src.VehicleDtos.ToList() as IReadOnlyList<VehicleDto>;

        _logger.Information("Vehicles retrieved successfully with total items: {TotalItems}", src.TotalItems);
        return Ok(new Pagination<VehicleDto>(vehicleParams.Pagesize, vehicleParams.PageNumber, src.PageItemCount, src.TotalItems, vehicles));
    }

    [HttpGet("get-all-vehicles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllVehiclesAsync()
    {
        _logger.Information("Retrieving all vehicles.");
        var vehicles = _uow.VehicleRepository.GetAll();
        if (vehicles == null)
        {
            _logger.Warning("No vehicles found in the system.");
            return NotFound(new ApiException(404, "There are no vehicles in the system."));
        }

        var vehiclesdto = _mapper.Map<IReadOnlyList<VehicleDto>>(vehicles);
        _logger.Information("All vehicles retrieved successfully.");
        return Ok(vehiclesdto);
    }

    [HttpGet("get-vehicle/{Id}")]
    [Authorize]
    public async Task<IActionResult> GetVehicleAsync(string? Id)
    {
        _logger.Information("Retrieving vehicle with Id: {VehicleId}", Id);
        var vehicle = await _uow.VehicleRepository.GetByIdAsync(Id);
        if (vehicle == null)
        {
            _logger.Warning("Vehicle with Id: {VehicleId} could not be found.", Id);
            return NotFound(new ApiException(404, "Vehicle could not be found in the system."));
        }

        var vehicledto = _mapper.Map<VehicleDto>(vehicle);
        _logger.Information("Vehicle retrieved successfully: {@Vehicle}", vehicledto);
        return Ok(vehicledto);
    }

    [HttpPut("update-vehicle")]
    [Authorize]
    public async Task<IActionResult> UpdateVehicleAsync(VehicleDto dto)
    {
        _logger.Information("Updating vehicle with VIN: {Vin}, Data: {@VehicleData}", dto.Vin, dto);
        var vehicle = await _uow.VehicleRepository.GetByIdAsync(dto.Vin);

        if (vehicle == null)
        {
            _logger.Warning("Vehicle with VIN: {Vin} could not be found.", dto.Vin);
            return NotFound(new ApiException(404, "Vehicle could not be found in the system."));
        }

        _mapper.Map(dto, vehicle);

        try
        {
            await _uow.VehicleRepository.UpdateAsync(vehicle);
            _logger.Information("Vehicle updated successfully: {@Vehicle}", vehicle);
            var vehicledto = _mapper.Map<VehicleDto>(vehicle);
            return Ok(vehicledto);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while updating vehicle with VIN: {Vin}", dto.Vin);
            throw new Exception(ex.Message);
        }
    }

    [HttpDelete("delete-vehicle/{Id}")]
    [Authorize]
    public async Task<IActionResult> DeleteVehicleAsync(string? Id)
    {
        _logger.Information("Deleting vehicle with Id: {VehicleId}", Id);
        var vehicle = await _uow.VehicleRepository.GetByIdAsync(Id);

        if (vehicle == null)
        {
            _logger.Warning("Vehicle with Id: {VehicleId} could not be found.", Id);
            return NotFound(new ApiException(404, "Vehicle could not be found in the system."));
        }

        var vehiclemodel = await _uow.VehicleModelRepository.GetByIdAsync(vehicle.ModelId);

        try
        {
            await _uow.VehicleRepository.DeleteAsync(Id);
            _logger.Information("Vehicle deleted successfully with Id: {VehicleId}", Id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while deleting vehicle with Id: {VehicleId}", Id);
            throw new Exception(ex.Message);
        }

        vehiclemodel.Quantity = _uow.VehicleRepository.GetAll().Count(x => x.ModelId == vehiclemodel.Id);
        try
        {
            await _uow.VehicleModelRepository.UpdateAsync(vehiclemodel);
            _logger.Information("Vehicle model quantity updated successfully for model {ModelId}.", vehiclemodel.Id);
            return Ok("Vehicle successfully deleted and the quantity of the model updated.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while updating vehicle model quantity for model {ModelId}.", vehiclemodel.Id);
            throw new Exception(ex.Message);
        }
    }
}