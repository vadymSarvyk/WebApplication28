using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication28.Models;

namespace WebApplication28.Controllers
{
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    class Error
    {
        public string Message { get; set; }
    }

    public class HomeController : Controller
    {

        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;

        }
        public IActionResult GetFile()
        {
            // Путь к файлу
            string file_path = Path.Combine(_appEnvironment.ContentRootPath, "Files/25121697-keep-calm-and-stay-strong.jpg");
            // Тип файла - content-type
            string file_type = "application/jpg";
            // Имя файла - необязательно
            string file_name = "25121697-keep-calm-and-stay-strong.jpg";
            return PhysicalFile(file_path, file_type, file_name);
        }

        /*   public IActionResult Index()
           {
               return View();
           }*/
        public IActionResult Index(int age)
        {
            if (age < 18)
                return Unauthorized(new Error { Message = "параметр age содержит недействительное значение" });
            return View("Index");
        }
        [HttpGet]
        public IActionResult AddFile()
        {
            return View("AddFile");
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
               /* FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                _context.Files.Add(file);
                _context.SaveChanges();*/
            }
            return RedirectToAction("Index");
        }
    
    public IActionResult Privacy()
        {
            return View();
        }
        public ViewResult ShowAll()
        {
            return View();
        }
        public JsonResult GetUser()
        {
            User user = new User { Name = "Vadym", Age = 37 };
            return Json(user);
        }

        public ContentResult Area(int altitude, int height)
        {
            double area = altitude * height / 2;
            return Content($"Площадь треугольника с основанием {altitude} и высотой {height} равна {area}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
