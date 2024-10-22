using AutoMapper;
using ClassLibrary2.Entities;
using FluentValidation;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Errors;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

[Route("api/[controller]")]
[ApiController]
public class VehicleModelController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;
    private readonly Serilog.ILogger _logger;
    private readonly IVinGenerationService _vingenerationservice;
    private readonly UserManager<AppUsers> _userManager;
    private readonly IValidator<VehicleModels> _validator;

    public VehicleModelController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, IVinGenerationService vinGenerationService, UserManager<AppUsers> userManager, IValidator<VehicleModels> validator)
    {
        _logger = logger;
        _uow = uow;
        _mapper = mapper;
        _vingenerationservice = vinGenerationService;
        _userManager = userManager;
        _validator = validator;
    }

    [HttpGet("get-all-models")]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        _logger.Information("Retrieving all vehicle models.");
        var models = _uow.VehicleModelRepository.GetAll();

        if (models == null)
        {
            _logger.Warning("No models found in the system.");
            return NotFound(new ApiException(404, "There are no models in the system."));
        }

        var dtos = _mapper.Map<IEnumerable<VehicleModelDto>>(models);
        _logger.Information("All models retrieved successfully.");
        return Ok(dtos);
    }

    [HttpGet("get-models")]
    [Authorize]
    public async Task<IActionResult> GetAllPaginated([FromQuery] VehiclemodelParams modelParams)
    {
        _logger.Information("Retrieving paginated vehicle models. PageNumber: {PageNumber}, PageSize: {PageSize}", modelParams.PageNumber, modelParams.Pagesize);
        var src = await _uow.VehicleModelRepository.GetAllAsync(modelParams);
        var models = src.VehicleModelDtos.ToList() as IReadOnlyList<VehicleModelDto>;

        _logger.Information("Models retrieved successfully with total items: {TotalItems}", src.TotalItems);
        return Ok(new Pagination<VehicleModelDto>(modelParams.Pagesize, modelParams.PageNumber, src.PageItemCount, src.TotalItems, models));
    }

    [HttpPost("create-model")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAsync([FromForm] RegisterVehicleModelDto dto)
    {
        _logger.Information("Creating vehicle model with data: {@VehicleModelData}", dto);
        var modelCheck = _mapper.Map<VehicleModels>(dto);
        var validationResult = _validator.Validate(modelCheck);

        if (validationResult.IsValid)
        {
            try
            {
                var vehiclemodel = await _uow.VehicleModelRepository.AddAsync(dto);
                _logger.Information("Vehicle model created successfully: {@VehicleModel}", vehiclemodel);

                var modeldto = _mapper.Map<VehicleModelDto>(vehiclemodel);
                return Ok(modeldto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating vehicle model.");
                throw new Exception(ex.Message);
            }
        }

        _logger.Warning("Vehicle model creation failed due to validation errors: {@Errors}", validationResult.Errors);
        return BadRequest(new ApiValidationErrorResponse(validationResult.Errors));
    }

    [HttpGet("get-model/{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetModelAsync(int? Id)
    {
        _logger.Information("Retrieving vehicle model with Id: {ModelId}", Id);
        var model = await _uow.VehicleModelRepository.GetByIdAsync(Id);

        if (model == null)
        {
            _logger.Warning("Vehicle model with Id: {ModelId} could not be found.", Id);
            return NotFound(new ApiException(404, "This model could not be found in the system."));
        }

        _logger.Information("Vehicle model retrieved successfully: {@VehicleModel}", model);
        var modeldto = _mapper.Map<VehicleModelDto>(model);
        return Ok(modeldto);
    }

    [HttpPut("update-model")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateVehicleModelAsync([FromForm] UpdateVehicleModelDto dto)
    {
        _logger.Information("Updating vehicle model with Id: {ModelId}, Data: {@VehicleModelData}", dto.Id, dto);
        var modelCheck = _mapper.Map<VehicleModels>(dto);
        var validationResult = _validator.Validate(modelCheck);

        if (validationResult.IsValid)
        {
            try
            {
                var vehiclemodel = await _uow.VehicleModelRepository.UpdateAsync(dto);

                if (vehiclemodel == null)
                {
                    _logger.Warning("Vehicle model with Id: {ModelId} could not be found.", dto.Id);
                    return NotFound(new ApiException(404, "This model could not be found in the system."));
                }

                _logger.Information("Vehicle model updated successfully: {@VehicleModel}", vehiclemodel);
                var modeldto = _mapper.Map<VehicleModelDto>(vehiclemodel);
                return Ok(modeldto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while updating vehicle model with Id: {ModelId}", dto.Id);
                throw new Exception(ex.Message);
            }
        }

        _logger.Warning("Vehicle model update failed due to validation errors: {@Errors}", validationResult.Errors);
        return BadRequest(new ApiValidationErrorResponse(validationResult.Errors));
    }

    [HttpDelete("delete-model/{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteVehicleModelAsync(int? Id)
    {
        _logger.Information("Deleting vehicle model with Id: {ModelId}", Id);
        var vehiclemodel = await _uow.VehicleModelRepository.GetByIdAsync(Id);

        if (vehiclemodel == null)
        {
            _logger.Warning("Vehicle model with Id: {ModelId} could not be found.", Id);
            return NotFound(new ApiException(404, "This model could not be found in the system."));
        }

        try
        {
            if (await _uow.VehicleModelRepository.DeleteAsync(Id))
            {
                _logger.Information("Vehicle model deleted successfully with Id: {ModelId}", Id);
                return Ok("Model successfully deleted.");
            }

            _logger.Warning("Vehicle model deletion failed with Id: {ModelId}", Id);
            return BadRequest(new ApiException(404, "The delete action failed."));
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error occurred while deleting vehicle model with Id: {ModelId}", Id);
            throw new Exception(ex.Message);
        }
    }
}