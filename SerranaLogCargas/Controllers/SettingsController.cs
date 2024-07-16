using LogCargas.Data;
using LogCargas.Models.ViewModels;
using LogCargas.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LogCargas.Controllers
{
    public class SettingsController : Controller
    {
        private readonly SeedingService _seedingService; 
        private readonly StateService _stateService;
        private readonly CityService _cityService;  

        public SettingsController (SeedingService seedingService, StateService stateService, CityService cityService)
        {
            _seedingService = seedingService;
            _stateService = stateService;
            _cityService = cityService;
        }   

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StatePop()
        {
            _seedingService.Seed();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportCities(IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                var streamFile = _cityService.LerStream(formFile);
                var cities = _cityService.LerXls(streamFile);
                await _cityService.SalvarImportacao(cities);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
