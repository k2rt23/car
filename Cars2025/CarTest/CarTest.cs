using Cars.ApplicationServices.Services;
using Cars.Core.Domain;
using Cars.Core.Dto;
using Cars.Core.ServiceInterface;
using Cars.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Cars.CarTest
{
    public class CarTest
    {
        private CarsContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<CarsContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new CarsContext(options);
        }

        [Fact]
        public async Task Create_ShouldAddCarToDatabase()
        {
            var context = GetInMemoryDbContext();
            var carService = new CarsServices(context);
            var carDto = new CarDto
            {
                Make = "Ford",
                Model = "Mustang",
                Year = 2022
            };

            var createdCar = await carService.Create(carDto);

            Assert.NotNull(createdCar);
            Assert.Equal("Ford", createdCar.Make);
            Assert.Equal("Mustang", createdCar.Model);
            Assert.Equal(2022, createdCar.Year);
            Assert.Equal(1, context.Cars.Count());
        }

        [Fact]
        public async Task Should_AddCar_WhenValidDto()
        {
            var context = GetInMemoryDbContext();
            var carService = new CarsServices(context);
            var carDto = MockCarData();

            var result = await carService.Create(carDto);

            Assert.NotNull(result);
            Assert.Equal(carDto.Make, result.Make);
            Assert.Equal(carDto.Model, result.Model);
            Assert.Equal(carDto.Year, result.Year);
        }

        [Fact]
        public async Task Should_DeleteCarById_WhenValidId()
        {
            var context = GetInMemoryDbContext();
            var carService = new CarsServices(context);
            var carDto = MockCarData();
            var addedCar = await carService.Create(carDto);

            var result = await carService.Delete((Guid)addedCar.Id);

            Assert.NotNull(result);
            Assert.Equal(addedCar.Id, result.Id);
            Assert.Empty(context.Cars);
        }

        [Fact]
        public async Task ShouldNot_DeleteCar_WhenInvalidId()
        {
            var context = GetInMemoryDbContext();
            var carService = new CarsServices(context);
            var carDto = MockCarData();
            await carService.Create(carDto);

            var result = await carService.Delete(Guid.NewGuid());

            Assert.Null(result);
            Assert.Equal(1, context.Cars.Count());
        }

        [Fact]
        public async Task Should_UpdateCar_WhenValidData()
        {
            var context = GetInMemoryDbContext();
            var carService = new CarsServices(context);
            var carDto = MockCarData();
            var addedCar = await carService.Create(carDto);

            var updateDto = new CarDto
            {
                Id = addedCar.Id,
                Make = "Tesla",
                Model = "Model S",
                Year = 2023,
                ModifiedAt = DateTime.Now
            };

            var updatedCar = await carService.Update(updateDto);

            Assert.NotNull(updatedCar);
            Assert.Equal("Tesla", updatedCar.Make);
            Assert.Equal("Model S", updatedCar.Model);
            Assert.Equal(2023, updatedCar.Year);
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
