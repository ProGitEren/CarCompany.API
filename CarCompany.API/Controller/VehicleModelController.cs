using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Errors;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

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
                _logger.Warning("The Current User could nto be found in the system.");
                return NotFound(new ApiException(404, "There is no model in the system."));
            }

            var dtos = _mapper.Map<IEnumerable<VehicleModelDto>>(models);
            return Ok(dtos);

        }

    }
}
