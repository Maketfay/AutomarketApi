using AutomarketApi.Helpers.Interfaces;
using AutomarketApi.Models.Car;
using AutomarketApi.Models.Enum;

namespace AutomarketApi.Helpers
{
    public class CarViewHelper : ICarViewHelper
    {
        public CarViewModel FromEntityToView(Car entity)
        {
            return new CarViewModel()
            {
                Name= entity.Name,
                Description = entity.Description,
                Model = entity.Model,
                Speed = entity.Speed,
                Price = entity.Price,
                Color= entity.Color,
                DateCreate = entity.DateCreate,
                TypeCar = entity.TypeCar
            };
        }

        public Car FromViewToEntity(CarViewModel view)
        {
            return new Car()
            {
                Name =view.Name,
                Description = view.Description,
                Model = view.Model,
                Speed = view.Speed,
                Color = view.Color,
                Price = view.Price,
                DateCreate = view.DateCreate,
                TypeCar = view.TypeCar
            };
        }
    }
}
