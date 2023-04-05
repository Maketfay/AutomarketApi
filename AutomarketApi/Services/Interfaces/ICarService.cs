using AutomarketApi.Models.Car;
using CSharpFunctionalExtensions;

namespace AutomarketApi.Services.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarViewModel>> GetCars();
        Task<Result<Car>> GetCar(Guid id);

        Task<Guid> CreateCar(CarViewModel carViewModel);

        Task<bool> Delete(Guid id);

        //Task<CarViewModel> GetByName(string name);

        Task<CarViewModel> Edit(Car model);
    }
}
