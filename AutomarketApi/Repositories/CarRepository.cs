using AutomarketApi.Context;
using AutomarketApi.Models.Car;
using AutomarketApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutomarketApi.Repositories
{
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext context) : base(context)
        {
        }
        //private readonly ApplicationDbContext _db;

        //public CarRepository(ApplicationDbContext db)
        //{
        //    _db = db;
        //}
        //public async Task<bool> Create(Car entity)
        //{
        //    try
        //    {
        //        await _db.Car.AddAsync(entity);
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception ex) 
        //    {

        //        return false;
        //    }            
        //}

        //public async Task<bool> Delete(Car entity)
        //{
        //    try 
        //    {
        //        _db.Car.Remove(entity);
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch(Exception ex) 
        //    {

        //        return false;
        //    }
        //}

        //public IQueryable<Car> Get()
        //{
        //    return _db.Car;
        //}
    }
}
