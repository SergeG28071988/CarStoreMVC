using CarStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarStoreMVC.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
 

        public CarsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;      
        }


        public IActionResult Index()
        {
            var cars = context.Cars.OrderByDescending(c => c.Id).ToList();
            return View(cars);
        }

        public IActionResult Create()
        {
            CarDto carDto = new CarDto();
            return View(carDto);
        }

        [HttpPost]
        public IActionResult Create(CarDto carDto)
        {
                       
            if (carDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(carDto);
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(carDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/images/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                carDto.ImageFile.CopyTo(stream);
            }

            // save the new car in the database
            var car = new Car()
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                Desc = carDto.Desc,
                Availability = carDto.Availability,
                Price = carDto.Price,
                Image = newFileName,
                CreatedAt = DateTime.Now
            };

            context.Cars.Add(car);
            context.SaveChanges();

            return RedirectToAction("Index", "Cars");
        }

        public void CheckAvailability(CarDto carDto)
        {
            if (carDto != null)
            {
                string availabilityText = carDto.Availability ? "Есть в наличии" : "Продано";
                // Дальнейший код, который использует availabilityText
            }
        }

        public IActionResult Edit(int id)
        {
            var car = context.Cars.Find(id);
            if (car == null)
            {
                return RedirectToAction("Index", "Cars");
            }

            // create carDto from car
            var carDto = new CarDto()
            {
                Brand = car.Brand,
                Model = car.Model,
                Desc = car.Desc,
                Price= car.Price,
            };

            ViewData["CarId"] = car.Id;
            ViewData["Image"] = car.Image;
            ViewData["CreatedAt"] = car.CreatedAt.ToString("dd/MM/yyyy");

            return View(carDto);  
        }

        [HttpPost]
        public IActionResult Edit(int id, CarDto carDto)
        {
            var car = context.Cars.Find(id);
            if (car == null)
            {
                return RedirectToAction("Index", "Cars");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CarId"] = car.Id;
                ViewData["Image"] = car.Image;
                ViewData["CreatedAt"] = car.CreatedAt.ToString("dd/MM/yyyy");

                return View(carDto);
            }

            // update the image file if we have a new image file
            string newFileName = car.Image;
            if (carDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(carDto.ImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/images/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    carDto.ImageFile.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/images/" + car.Image;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the car in the database
            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.Desc = carDto.Desc;
            car.Availability = carDto.Availability; 
            car.Price = carDto.Price;
            car.Image = newFileName;

            context.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }

        public IActionResult Delete(int id)
        {
            var car = context.Cars.Find(id);

            if (car == null) {

                return RedirectToAction("Index", "Cars");
            }

            string imageFullPath = environment.WebRootPath + "/images/" + car.Image;
            System.IO.File.Create(imageFullPath);

            context.Remove(car);

            context.SaveChanges();

            return RedirectToAction("Index", "Cars");
        }

    }
}
