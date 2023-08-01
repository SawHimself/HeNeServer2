using HeNeServer2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HeNeServer2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Images()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            string[] filePaths = Directory.GetFiles(directoryPath);
            string[] fileNames = new string[filePaths.Length];

            for (int i = 0; i < filePaths.Length; i++)
            {
                fileNames[i] = Path.GetFileName(filePaths[i]);
            }

            ViewBag.ImageFiles = fileNames;

            return View();
        }
        public async Task Upload()
        {
            //return "Test";
            var response = ControllerContext.HttpContext.Response;
            var request = ControllerContext.HttpContext.Request;

            response.ContentType = "text/html; charset=utf-8";
            if (request.Method == "POST")
            {
                IFormFileCollection files = request.Form.Files;
                string uploadPath = $"wwwroot/images/";
                Directory.CreateDirectory(uploadPath);

                foreach (var file in files)
                {
                    string fullPath = $"{uploadPath}/{file.FileName}";

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    await response.WriteAsync("файлы загружены, юху!");
                }
            }
            else
            {
                await response.WriteAsync("...");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}