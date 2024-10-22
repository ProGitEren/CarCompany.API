using AutoMapper;
using ClassLibrary2.Entities;
using FluentValidation;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.DTO.Dto_VehicleModels;
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
    [Authorize(Roles = "Admin")]
    public class EngineController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IValidator<Engines> _validator;

        public EngineController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, IValidator<Engines> validator)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _validator = validator;
        }


        [HttpGet("get-all-engines")]
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

        [HttpGet("get-engines")]
        [Authorize]

        public async Task<IActionResult> GetAllPaginated([FromQuery] EngineParams engineParams)
        {
            var src = await _uow.EngineRepository.GetAllAsync(engineParams);
            var engines = src.EngineDtos.ToList() as IReadOnlyList<EngineDto>;

            return Ok(new Pagination<EngineDto>(engineParams.Pagesize, engineParams.PageNumber,src.PageItemCount, src.TotalItems, engines));
        }


        [HttpPost("create-engine")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateAsync(RegisterEngineDto dto)
        {
            var engine = _mapper.Map<Engines>(dto);

            // Previous Implementation for validating the Entities this was related to built-in Data Annotations validator

            //var validationerrorlist = EntityValidator.GetValidationResults(engine);

            //if (validationerrorlist.Any())
            //{
            //    return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            //}

            var validationResult = _validator.Validate(engine);
            if (validationResult.IsValid)
            {
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

            return BadRequest(new ApiValidationErrorResponse(validationResult.Errors));

        }


        [HttpGet("get-engine/{Id}")]
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

            var dto = _mapper.Map<EngineDto>(engine);

            return Ok(dto);

        }

        [HttpPut("update-engine")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateEngineAsync(EngineDto dto)
        {
            var engineCheck = _mapper.Map<Engines>(dto);
            var validationResult = _validator.Validate(engineCheck);

            if (validationResult.IsValid)
            {

                var engine = await _uow.EngineRepository.GetByIdAsync(dto.Id);
                _logger.Information("Engine is being retrieved.");
                if (engine == null)
                {
                    _logger.Warning("This engine could not be found in the system.");
                    return NotFound(new ApiException(404, "This engine did not found in the system."));
                }

                _mapper.Map(dto, engine);

                //var validationerrorlist = EntityValidator.GetValidationResults(engine);

                //if (validationerrorlist.Any())
                //{
                //    return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
                //}

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
            return BadRequest(new ApiValidationErrorResponse(validationResult.Errors));

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
