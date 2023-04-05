using AutomarketApi.Helpers;
using AutomarketApi.Helpers.Interfaces;
using AutomarketApi.Models.Car;
using AutomarketApi.Repositories;
using AutomarketApi.Repositories.Interfaces;
using AutomarketApi.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AutomarketApi.Services.Implementation
{
    public class CarService : ICarService
    {

        private readonly ICarRepository _carRepository;

        private readonly ICarViewHelper _carViewHelper;

        public CarService(ICarRepository carRepository, ICarViewHelper carViewHelper) 
        { 
            _carRepository = carRepository;
            _carViewHelper = carViewHelper;
        }
        public async Task<Guid> CreateCar(CarViewModel carViewModel)
        {

                var car = _carViewHelper.FromViewToEntity(carViewModel);
                var response = await _carRepository.Create(car);

                return response;            
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {

                var car = await _carRepository.Read(id);

                if (car == null) 
                {
                    return false;
                }

                _carRepository.Delete(car);
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<CarViewModel> Edit(Car model)
        {
            try
            {
                var car = await _carRepository.Read(model.Id);

                if (car == null)
                    return null;

                //var data = await _carRepository.Update(model);

                //return _carViewHelper.FromEntityToView(data);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<CarViewModel> GetByName(string name)
        //{
        //    try
        //    {
        //        var car = await _carRepository.Get().FirstOrDefaultAsync(x => x.Name.Equals(name));

        //        return _carViewHelper.FromEntityToView(car);
        //    }
        //    catch (Exception ex)
        //    {
        //        //ToDo logger
        //        return null;
        //    }
        //}

        public async Task<Result<Car>> GetCar(Guid id)
        {
                var car = await _carRepository.Read(id);

                if (car == null) 
                {
                    return Result.Failure<Car>("Not found");
                }

                return Result.Success(car);
        }

        public async Task<IEnumerable<CarViewModel>> GetCars()
        {
            try
            {
                var cars = (await _carRepository.ReadAll()).Select(x => _carViewHelper.FromEntityToView(x)).ToList();

                return cars;
            }
            catch (Exception ex)
            {
                //ToDo logger
                return null;
            }
        }
    }
}
