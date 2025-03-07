﻿using AutoServices.Core.ServiceInterface;
using Cars.Core.Domain;
using Cars.Core.Dto;
using Cars.Core.ServiceInterface;
using Cars.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Cars.ApplicationServices.Services;

public class CarServices : ICarsServices  
{
    private readonly CarsContext _context;

    public CarServices(CarsContext context)
    {
        _context = context;
    }

    public async Task<Car> Create(CarDto dto)
    {
        var car = new Car
        {
            Id = Guid.NewGuid(),
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task<Car> Update(CarDto dto)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (car == null) return null;

        car.Make = dto.Make;
        car.Model = dto.Model;
        car.Year = dto.Year;
        car.ModifiedAt = DateTime.Now;

        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task<Car> Delete(Guid id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
        if (car == null) return null;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task<Car> DetailAsync(Guid id)
    {
        return await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
    }
}
