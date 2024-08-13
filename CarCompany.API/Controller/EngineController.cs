using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Errors;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
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
    public class EngineController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;

        public EngineController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
        }


        [HttpGet("get-all-engines")]
        [Authorize]

        public async Task<IActionResult> GetAll()
        {
            var engines = _uow.EngineRepository.GetAll();
            _logger.Information("Engines are retrieving.");
            if (engines == null)
            {
                _logger.Warning("The Engine could nto be found in the system.");
                return NotFound(new ApiException(404, "There is no engine in the system."));
            }

            var dtos = _mapper.Map<IEnumerable<EngineDto>>(engines);
            return Ok(dtos);

        }


        [HttpPost("create-engine")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateAsync(RegisterEngineDto dto)
        {
            var engine = _mapper.Map<Engines>(dto);

            var validationerrorlist = EntityValidator.GetValidationResults(engine);

            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            try
            {
                await _uow.EngineRepository.AddAsync(engine);
                _logger.Information($"The Engine succesfully created.");
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var enginedto = _mapper.Map<EngineDto>(engine);
            return Ok(enginedto);

        }


        [HttpGet("get-model/{Id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetEngineAsync(int? Id)
        {
            var engine = await _uow.EngineRepository.GetByIdAsync(Id);
            _logger.Information("Engine is being retrieved.");

            if (engine == null)
            {
                _logger.Warning("This engine could not be found in the system.");
                return NotFound(new ApiException(404, "This engine did not found in the system."));
            }

            var dto = _mapper.Map<VehicleModelDto>(engine);

            return Ok(dto);

        }

        [HttpPut("update-engine")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateEngineAsync(EngineDto dto)
        {

            var engine = await _uow.EngineRepository.GetByIdAsync(dto.Id);
            _logger.Information("Engine is being retrieved.");
            if (engine == null)
            {
                _logger.Warning("This engine could not be found in the system.");
                return NotFound(new ApiException(404, "This engine did not found in the system."));
            }

            _mapper.Map(dto, engine);

            var validationerrorlist = EntityValidator.GetValidationResults(engine);

            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            try
            {
                await _uow.EngineRepository.UpdateAsync(engine);
                _logger.Information("Engine is successfully updated.");
                var enginedto = _mapper.Map<EngineDto>(engine);

                return Ok(enginedto);
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occured while updating the engine.");
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("delete-engine/{Id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteEngineAsync(int? Id)
        {
            var engine = await _uow.EngineRepository.GetByIdAsync(Id);

            if (engine == null)
            {
                _logger.Warning("This engine could not be found in the system.");
                return NotFound(new ApiException(404, "This engine did not found in the system."));
            }

            try
            {
                await _uow.EngineRepository.DeleteAsync(Id);
                _logger.Information("Engine is successfully deleted.");
                return Ok("Engine successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occured while deleting the engine.");
                throw new Exception(ex.Message);


            }
        }


    }
}
