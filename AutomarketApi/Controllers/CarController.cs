using AutomarketApi.Models.Car;
using AutomarketApi.Models.Discussions;
using AutomarketApi.Models.Identity;
using AutomarketApi.Services.Implementation;
using AutomarketApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomarketApi.Controllers
{
    [Route("/cars")]
    [ApiController]
    public class CarController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IChatService _chatService;

        public CarController(IUnitOfWork unitOfWork, ICarService carService, IChatService chatService) : base(unitOfWork)
        {
            _carService = carService;
            _chatService = chatService;
        }

        [HttpGet("getCar")]
        public async Task<IActionResult> GetCar(Guid id)
        {

            var response = await _carService.GetCar(id);

            return Ok(response);
        }

        [HttpGet("getCars")]
        public async Task<IActionResult> GetCars()
        {

            var response = await _carService.GetCars();

            return Ok(response);
        }

        [HttpPost("createCar"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCar(CarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {    
                var carId = await _carService.CreateCar(carViewModel);
                var carResult = await _carService.GetCar(carId);

                if (carResult.IsFailure) 
                {
                    return BadRequest();
                }

                var chatId = await _chatService.CreateChat(new ChatDto()
                {
                    Id = Guid.NewGuid(),
                    CarAbout = carResult.Value,
                    Users = new List<User>(), 
                    Messages = new List<MessageDto>()
                });


                return await ReturnSuccess(new CarResponseModel {CarId = carId, ChatId= chatId });
            }

            return BadRequest();
        }
    }
}
