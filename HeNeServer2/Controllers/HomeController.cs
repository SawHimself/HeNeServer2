using HeNeServer2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

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
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            double free = 0;
            double total = 0;
            string Vol = "";
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo MyDriverInfo in allDrives)
            {
                if (MyDriverInfo.IsReady)
                {
                    free = MyDriverInfo.AvailableFreeSpace;
                    total = MyDriverInfo.TotalSize;
                    free = (free / 1024) / 1024 / 1024;
                    total = (total / 1024) / 1024 / 1024;
                    double Persent = ((total - free)/total) * 100;
                    Vol += Persent.ToString("0.0") + "%";
                }
            }
            ViewBag.FreeSpace = free;
            ViewBag.Total = total;
            ViewBag.Vol = Vol;
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
        public IActionResult Files()
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files");
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
                string uploadPathImages = $"wwwroot/images/";
                string uploadPathFiles = $"wwwroot/files/";
                string fullPath;
                foreach (var file in files)
                {
                    if (ImageCheck.test(file.FileName))
                    {
                        fullPath = $"{uploadPathImages}/{file.FileName}";
                    }
                    else
                    {
                        fullPath = $"{uploadPathFiles}/{file.FileName}";
                    }
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