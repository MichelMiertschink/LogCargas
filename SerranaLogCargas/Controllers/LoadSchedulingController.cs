﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogCargas.Models;
using LogCargas.Models.ViewModels;
using LogCargas.Services;
using System.Diagnostics;

namespace LogCargas.Controllers
{
    public class LoadSchedulingController : Controller
    {
        private readonly LoadSchedulingService _loadSchedulingService;
        private readonly CustomerService _customerService;
        private readonly CityService _cityService;

        public LoadSchedulingController(LoadSchedulingService loadScheduling, CustomerService customerService, CityService cityService)
        {
            _loadSchedulingService = loadScheduling;
            _customerService = customerService;
            _cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _loadSchedulingService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 10);
            }
            ViewData["minDate"] = minDate.Value.ToString("dd/MM/yyyy");
            ViewData["maxDate"] = maxDate.Value.ToString("dd/MM/yyyy");
            var result = await _loadSchedulingService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        // GET: LoadScheduling/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var customers = await _customerService.FindAllAsync();
                var cities = await _cityService.FindAllAsync();
                var viewModel = new LoadSchedulingFormViewModel { Customer = customers, City = cities};
                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoadScheduling loadScheduling)
        {
            if (ModelState.IsValid)
            {
                var customers = _customerService.FindAll();
                var cities = await _cityService.FindAllAsync();
                var viewModel = new LoadSchedulingFormViewModel { Customer = customers, City = cities };
                return View(viewModel);
            }

            try
            {
                loadScheduling.IncludeDate = DateTime.Now;
                await _loadSchedulingService.InsertAsync(loadScheduling);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {

                return RedirectToAction(nameof(Error), new
                {
                    message = e.Message +
                    "CustomerID: " + loadScheduling.CustomerId.ToString() +
                    "\nOriginID: " + loadScheduling.CityDestinyId.ToString() +
                    "\nDestinyID: " + loadScheduling.CityOriginId.ToString()
                });
            }

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { mnessage = "Id não fornecido" });
            }
            var loadSchedule = await _loadSchedulingService.FindByIdAsync(id.Value);
            if (loadSchedule == null)
            {
                return RedirectToAction(nameof(Error), new { mnessage = "Id não encontrado" });
            }
            return View(loadSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (int id)
        {
            try
            {
                await _loadSchedulingService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details (int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { mnessage = "Id não fornecido" });
            }
            var obj = await _loadSchedulingService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { mnessage = "Id não encontrado" });
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { mnessage = "Id não fornecido" });
            }
            var loadSchedule = await _loadSchedulingService.FindByIdAsync(id.Value);
            if (loadSchedule == null)
            {
                return RedirectToAction(nameof(Error), new { mnessage = "Id não encontrado" });
            }
            List<Customer> customers = await _customerService.FindAllAsync();
            List<City> cities = await _cityService.FindAllAsync();
            var viewModel = new LoadSchedulingFormViewModel { LoadScheduling = loadSchedule, Customer = customers, City = cities };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, LoadScheduling loadScheduling)
        {
            if (ModelState.IsValid)
            {

                var customers = await _customerService.FindAllAsync();
                var cities = await _cityService.FindAllAsync();
                var viewModel = new LoadSchedulingFormViewModel { Customer = customers, City = cities };
                return View(viewModel);
                
            }
            if (id != loadScheduling.Id)
            {
                return RedirectToAction(nameof(Error), new
                {
                    message = "----- ID não fornecido  = ID: " + loadScheduling.Id.ToString()
                });
            }

            try
            {
                await _loadSchedulingService.UpdateAsync(loadScheduling);
                return RedirectToAction(nameof(Index));
            } catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
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

    }
}
