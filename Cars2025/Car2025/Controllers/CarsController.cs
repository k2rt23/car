using Car2025.Models.Cars;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Car2025.Controllers
{
    public class CarsController : Controller
    {
        private static List<CarsIndexViewModel> _cars = new List<CarsIndexViewModel>();

        public IActionResult Index()
        {
            return View(_cars);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CarCreateUpdateViewModel());
        }

        [HttpPost]
        public IActionResult Create(CarCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newCar = new CarsIndexViewModel
            {
                Id = _cars.Any() ? _cars.Max(c => c.Id) + 1 : 1,
                Manufacturer = model.Manufacturer,
                ModelName = model.ModelName,
                ProductionYear = model.ProductionYear > 0 ? model.ProductionYear : DateTime.UtcNow.Year,
                SalePrice = model.SalePrice
            };

            _cars.Add(newCar);
            TempData["Message"] = "Car created successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var car = _cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            var model = new CarCreateUpdateViewModel
            {
                Id = car.Id,
                Manufacturer = car.Manufacturer,
                ModelName = car.ModelName,
                ProductionYear = car.ProductionYear,
                SalePrice = car.SalePrice
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, CarCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var car = _cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            car.Manufacturer = model.Manufacturer;
            car.ModelName = model.ModelName;
            car.ProductionYear = model.ProductionYear;
            car.SalePrice = model.SalePrice;

            TempData["Message"] = "Car updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var car = _cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _cars.FirstOrDefault(x => x.Id == id);
            if (car != null)
            {
                _cars.Remove(car);
                TempData["Message"] = "Car deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var car = _cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
    }
}
