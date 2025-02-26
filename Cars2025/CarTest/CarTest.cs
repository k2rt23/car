using AutoServices.Core.ServiceInterface;
using Cars.ApplicationServices.Services;
using Cars.Core.Domain;
using Cars.Core.Dto;
using Cars.Core.ServiceInterface;
using Cars.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Cars.CarTest
{
    public class CarTest : IDisposable
    {
        private readonly CarsContext _context;
        private readonly ICarsServices _carService;

        public CarTest()
        {
            _context = GetInMemoryDbContext();
            _carService = new Cars.ApplicationServices.Services.CarServices(_context); // Kasutame täielikku nime
        }

        private CarsContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CarsContext>()
                .UseInMemoryDatabase(databaseName: "CarTestDB")
                .Options;

            var context = new CarsContext(options);
            context.Database.EnsureCreated(); // Veendu, et andmebaas on loodud
            return context;
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Kustuta testandmebaas pärast testi
            _context.Dispose();
        }

        [Fact]
        public async Task Create_ShouldAddCarToDatabase()
        {
            var carDto = new CarDto
            {
                Make = "Ford",
                Model = "Mustang",
                Year = 2022
            };

            var createdCar = await _carService.Create(carDto);

            Assert.NotNull(createdCar);
            Assert.Equal("Ford", createdCar.Make);
            Assert.Equal("Mustang", createdCar.Model);
            Assert.Equal(2022, createdCar.Year);

            var carsInDb = await _context.Cars.ToListAsync();
            Assert.Single(carsInDb);
        }

        [Fact]
        public async Task Read_ShouldReturnCarById()
        {
            var carDto = MockCarData();
            var addedCar = await _carService.Create(carDto);

            var carFromDb = await _context.Cars.FindAsync(addedCar.Id);

            Assert.NotNull(carFromDb);
            Assert.Equal(addedCar.Id, carFromDb.Id);
        }

        [Fact]
        public async Task Update_ShouldModifyCarDetails()
        {
            var carDto = MockCarData();
            var addedCar = await _carService.Create(carDto);

            var updateDto = new CarDto
            {
                Id = addedCar.Id,
                Make = "Tesla",
                Model = "Model S",
                Year = 2023,
                ModifiedAt = DateTime.Now
            };

            var updatedCar = await _carService.Update(updateDto);

            Assert.NotNull(updatedCar);
            Assert.Equal("Tesla", updatedCar.Make);
            Assert.Equal("Model S", updatedCar.Model);
            Assert.Equal(2023, updatedCar.Year);

            var carsInDb = await _context.Cars.ToListAsync();
            Assert.Single(carsInDb);
        }

        [Fact]
        public async Task Delete_ShouldRemoveCarFromDatabase()
        {
            var carDto = MockCarData();
            var addedCar = await _carService.Create(carDto);

            var deletedCar = await _carService.Delete((Guid)addedCar.Id);

            Assert.NotNull(deletedCar);
            Assert.Equal(addedCar.Id, deletedCar.Id);

            var carsInDb = await _context.Cars.ToListAsync();
            Assert.Empty(carsInDb);
        }

        [Fact]
        public async Task Delete_ShouldNotRemoveCar_WhenInvalidId()
        {
            var carDto = MockCarData();
            await _carService.Create(carDto);

            var result = await _carService.Delete(Guid.NewGuid());

            Assert.Null(result);

            var carsInDb = await _context.Cars.ToListAsync();
            Assert.Single(carsInDb);
        }

        private CarDto MockCarData()
        {
            return new CarDto
            {
                Make = "Volkswagen",
                Model = "Golf",
                Year = 2015,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }
    }
}
