using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Errors;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Entities;
using WebAPI.Validation;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IVinGenerationService _vingenerationservice;
        private readonly UserManager<AppUsers> _userManager;

        public VehicleController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, IVinGenerationService vinGenerationService, UserManager<AppUsers> userManager)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _vingenerationservice = vinGenerationService;
            _userManager = userManager;
        }


        [HttpPost("create-vehicle")]
        [Authorize]

        public async Task<IActionResult> CreateAsync(RegisterVehicleDto dto)
        {
            var user = await _userManager.FindEmailByClaimWithDetailAsync(User);
            _logger.Information("The Current User is retrieving.");
            if (user == null)
            {
                _logger.Warning("The Current User could nto be found in the system.");
                return NotFound(new ApiException(404, "The Current User could not be found in the system."));
            }
            var validationerrorlist = EntityValidator.GetValidationResults(_mapper.Map<Vehicles>(dto));

            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            var vehicle = _mapper.Map<Vehicles>(dto);
            var vehiclemodel = await _uow.VehicleModelRepository.GetByIdAsync(dto.ModelId);
            var engine = _uow.EngineRepository.GetByEngineCode(vehiclemodel.EngineCode);


            vehicle.ModelId = vehiclemodel.Id;
            vehicle.EngineId = engine.Id;
            vehicle.Vin = _vingenerationservice.GenerateVin(vehiclemodel);
            if (User.IsInRole("Admin"))
            {
                vehicle.UserId = null; // as the admin can not have its own vehicle it will be added as null userid
            }
            else
            {
                vehicle.UserId = user.Id;
            }
            try
            {
                await _uow.VehicleRepository.AddAsync(vehicle);
                _logger.Information($"The Vehicle successfully created for the User {user.Email}.");
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var vehicledto = _mapper.Map<VehicleDto>(vehicle);
            return Ok(vehicledto);

        }


        [HttpGet("vehicle-detail/{Email}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> VehicleDetailAsync(string? Email)
        {
            var user = await _userManager.FindEmailByEmailAsync(Email);
            _logger.Information("The Current User is retrieving.");
            if (user == null)
            {
                _logger.Warning("The Current User could not be found in the system.");
                return NotFound(new ApiException(404, "The Current User could not be found in the system."));
            }

            var userdto = _mapper.Map<UserwithdetailsDto>(user);

            return Ok(userdto);

        }

        [HttpGet("get-vehicles")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllVehiclesAsync()
        {
            var vehicles = _uow.VehicleRepository.GetAll();
            _logger.Information("Vehicle are being retrieved.");
            if (vehicles == null)
            {
                _logger.Warning("There is no vehicle in the application.");
                return NotFound(new ApiException(404, "There is no vehicle in the system."));
            }

            var vehiclesdto = _mapper.Map<IReadOnlyList<VehicleDto>>(vehicles);

            return Ok(vehiclesdto);

        }

        [HttpGet("get-vehicle/{Id}")]
        [Authorize]

        public async Task<IActionResult> GetVehicleAsync(string? Id)
        {
            var vehicle = await _uow.VehicleRepository.GetByIdAsync(Id);
            _logger.Information("Vehicle is being retrieved.");

            if (vehicle == null)
            {
                _logger.Warning("This vehicle could not be found in the system.");
                return NotFound(new ApiException(404, "This vehicle did not found in the system."));
            }

            var vehicledto = _mapper.Map<VehicleDto>(vehicle);

            return Ok(vehicledto);

        }

        [HttpPut("update-vehicle")]
        [Authorize]

        public async Task<IActionResult> UpdateVehicleAsync(VehicleDto dto)
        {

            var vehicle = await _uow.VehicleRepository.GetByIdAsync(dto.Vin);
            _logger.Information("Vehicle is being retrieved.");
            if (vehicle == null)
            {
                _logger.Warning("This vehicle could not be found in the system.");
                return NotFound(new ApiException(404, "This vehicle did not found in the system."));
            }
            _mapper.Map(dto, vehicle);

            var validationerrorlist = EntityValidator.GetValidationResults(vehicle);

            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            try
            {
                await _uow.VehicleRepository.UpdateAsync(vehicle);
                _logger.Information("Vehicle is successfully updated.");
                var vehicledto = _mapper.Map<VehicleDto>(vehicle);

                return Ok(vehicledto);
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occurder while udating the vehicle.");
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("delete-vehicle/{Id}")]
        [Authorize]

        public async Task<IActionResult> DeleteVehicleAsync(string? Id)
        {
            var vehicle = await _uow.VehicleRepository.GetByIdAsync(Id);

            if (vehicle == null)
            {
                _logger.Warning("This vehicle could not be found in the system.");
                return NotFound(new ApiException(404, "This vehicle did not found in the system."));
            }

            try
            {
                await _uow.VehicleRepository.DeleteAsync(Id);
                _logger.Information("Vehicle is successfully deleted.");
                return Ok("Vehicle successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occurder while udating the vehicle.");
                throw new Exception(ex.Message);


            }
        }
    }
}
