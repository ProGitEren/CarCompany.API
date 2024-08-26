using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.Errors;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Validation;

namespace WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderVehicleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly UserManager<AppUsers> _userManager;

        public OrderVehicleController(IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager)
        {
            _logger = logger;
            _uow = uow;
            _mapper = mapper;
            _userManager = userManager;
        }



        [HttpPost("create-ordervehicle")]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> CreateAsync([FromForm] CreateOrderVehicleDto dto)
        {
            try
            {
                var createtuple = await _uow.OrderVehicleRepository.AddAsync(dto);
                var ordervehicle = createtuple.Item1;
                var list = createtuple.Item2;
                
                if (list.Any())
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = list }); // no database operation
                }

                var modeldto = _mapper.Map<OrderVehicleDto>(ordervehicle);
                return Ok(modeldto);

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpGet("get-ordervehicle/{Id}")]
        [Authorize]

        public async Task<IActionResult> GetOrderVehicleAsync(int? Id)
        {
            var ordervehicle = await _uow.OrderVehicleRepository.GetByIdAsync(Id);
            _logger.Information("Order Vehicle is being retrieved.");

            if (ordervehicle == null)
            {
                _logger.Warning("This order vehicle could not be found in the system.");
                return NotFound(new ApiException(404, "This order vehicle did not found in the system."));
            }

            var dto = _mapper.Map<OrderVehicleDto>(ordervehicle);

            return Ok(dto);

        }

        [HttpPut("update-ordervehicle")]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> UpdateOrderVehicleAsync(UpdateOrderVehicleDto dto)
        {

            try
            {
                var updatetuple = await _uow.OrderVehicleRepository.UpdateAsync(dto);
                var ordervehicle = updatetuple.Item1;
                var list = updatetuple.Item2;

                if (list.Any())
                {
                    return BadRequest(new ApiValidationErrorResponse { Errors = list }); // no database operation
                }

                if (ordervehicle == null)
                {
                    _logger.Warning("This order vehicle could not be found in the system.");
                    return NotFound(new ApiException(404, "This order vehicle did not found in the system."));
                }

                _logger.Information("Vehicle is successfully updated.");
                var modeldto = _mapper.Map<OrderVehicleDto>(ordervehicle);

                return Ok(modeldto);
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occured while updating the order vehicle.");
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("delete-ordervehicle/{Id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> DeleteOrderVehicleAsync(int? Id)
        {
            
            var ordervehicle = await _uow.OrderVehicleRepository.GetByIdAsync(Id);

            if (ordervehicle == null)
            {
                _logger.Warning("This order vehicle could not be found in the system.");
                return NotFound(new ApiException(404, "This order vehicle did not found in the system."));
            }

            try
            {
                if (await _uow.OrderVehicleRepository.DeleteAsync(Id))
                {
                    _logger.Information("Order vehicle is successfully deleted.");
                    return Ok("Order vehicle successfully deleted.");
                }
                return BadRequest(new ApiException(404, "The delete action failed."));
            }
            catch (Exception ex)
            {
                _logger.Warning("Problem occured while deleting the order vehicle.");
                throw new Exception(ex.Message);


            }
        }

        [HttpDelete("delete-file")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> DeleteFileAsync([FromQuery] DeleteFileDto dto) 
        {
            var result = await _uow.OrderVehicleRepository.DeleteFileAsync(dto);
            if (result.Succeeded)
            {
                return Ok("The file successfully deleted.");
            }
            return NotFound(new ApiException(404, result.Errors.FirstOrDefault().Description.ToString()));
        }

        [HttpPost("add-file")]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> AddFileAsync([FromForm] AddFileDto dto)
        {
            var result = await _uow.OrderVehicleRepository.AddFileAsync(dto);

            if (result.Succeeded)
            {
                return Ok("The file successfully created.");
            }
            return NotFound(new ApiException(404, result.Errors.FirstOrDefault().Description.ToString()));

        }

        [HttpGet("get-ordervehicles")]
        [Authorize(Roles = "Admin,Seller")]

        public async Task<IActionResult> GetAllPaginated([FromQuery] OrderVehicleParams orderVehicleParams)
        {
            var src = await _uow.OrderVehicleRepository.GetAllAsync(orderVehicleParams);
            var ordervehicles = src.OrderVehicleDtos.ToList() as IReadOnlyList<OrderVehicleDto>;

            return Ok(new Pagination<OrderVehicleDto>(orderVehicleParams.Pagesize, orderVehicleParams.PageNumber, src.PageItemCount, src.TotalItems, ordervehicles));
        }

    }
}
