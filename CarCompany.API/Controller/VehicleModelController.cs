using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Errors;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Mapping;
using WebAPI.Validation;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IVinGenerationService _vingenerationservice;
        private readonly UserManager<AppUsers> _userManager;

        public VehicleModelController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, IVinGenerationService vinGenerationService, UserManager<AppUsers> userManager)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _vingenerationservice = vinGenerationService;
            _userManager = userManager;
        }


        [HttpGet("get-all-models")]
        [Authorize]

        public async Task<IActionResult> GetAll()
        {
            var models = _uow.VehicleModelRepository.GetAll();
            _logger.Information("The Current User is retrieving.");
            if (models == null)
            {
                _logger.Warning("The Current User could not be found in the system.");
                return NotFound(new ApiException(404, "There is no model in the system."));
            }

            var dtos = _mapper.Map<IEnumerable<VehicleModelDto>>(models);
            return Ok(dtos);

        }

        [HttpGet("get-models")]
        [Authorize]

        public async Task<IActionResult> GetAllPaginated([FromQuery] VehiclemodelParams modelParams)
        {
            var src =  await _uow.VehicleModelRepository.GetAllAsync(modelParams);
            var models = src.VehicleModelDtos.ToList() as IReadOnlyList<VehicleModelDto>;

            return Ok(new Pagination<VehicleModelDto>(modelParams.Pagesize, modelParams.PageNumber, src.PageItemCount, src.TotalItems, models));
        }



        [HttpPost("create-model")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateAsync([FromForm] RegisterVehicleModelDto dto)
        {
            try
            {
                var vehiclemodel = await _uow.VehicleModelRepository.AddAsync(dto);
                _logger.Information($"The Model succesfully created.");
                var validationerrorlist = EntityValidator.GetValidationResults(vehiclemodel);

                if (validationerrorlist.Any())
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
                }
                var modeldto = _mapper.Map<VehicleModelDto>(vehiclemodel);
                return Ok(modeldto);

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("get-model/{Id}")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> GetModelAsync(int? Id)
        {
            var model = await _uow.VehicleModelRepository.GetByIdAsync(Id);
            _logger.Information("Model is being retrieved.");

            if (model == null)
            {
                _logger.Warning("This model could not be found in the system.");
                return NotFound(new ApiException(404, "This model did not found in the system."));
            }

            var modeldto = _mapper.Map<VehicleModelDto>(model);

            return Ok(modeldto);

        }

        [HttpPut("update-model")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> UpdateVehicleModelAsync([FromForm] UpdateVehicleModelDto dto)
        {

            
           
            try
            {
                var vehiclemodel = await _uow.VehicleModelRepository.UpdateAsync(dto);

                if (vehiclemodel == null)
                {
                    _logger.Warning("This model could not be found in the system.");
                    return NotFound(new ApiException(404, "This model did not found in the system."));
                }

                var validationerrorlist = EntityValidator.GetValidationResults(vehiclemodel);
                if (validationerrorlist.Any())
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
                }

                _logger.Information("Vehicle is successfully updated.");
                var modeldto = _mapper.Map<VehicleModelDto>(vehiclemodel);

                return Ok(modeldto);
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occurder while updating the model.");
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("delete-model/{Id}")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> DeleteVehicleModelAsync(int? Id)
        {
            var vehiclemodel = await _uow.VehicleModelRepository.GetByIdAsync(Id);

            if (vehiclemodel == null)
            {
                _logger.Warning("This model could not be found in the system.");
                return NotFound(new ApiException(404, "This model did not found in the system."));
            }
            
            try
            {
                if(await _uow.VehicleModelRepository.DeleteAsync(Id))
                {
                    _logger.Information("Model is successfully deleted.");
                    return Ok("Model successfully deleted.");
                }
                return BadRequest(new ApiException(404,"The delete action failed."));
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occured while deleting the model.");
                throw new Exception(ex.Message);


            }
        }

    }
}
