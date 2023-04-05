using AutomarketApi.Models.Enum;

namespace AutomarketApi.Models.Car
{
    public class CarViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Model { get; set; }

        public double Speed { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }

        public DateTime DateCreate { get; set; }

        public TypeCar TypeCar { get; set; }
    }
}
