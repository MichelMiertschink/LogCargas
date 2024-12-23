﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using LogCargas.Models;
using LogCargas.Models.ViewModels;
using LogCargas.Services;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LogCargas.Controllers
{
    public class LoadSchedulingController : Controller
    {
        private readonly LoadSchedulingService _loadSchedulingService;
        private readonly CustomerService _customerService;
        private readonly CityService _cityService;
        private readonly StateService _stateService;
        private readonly DriverService _driverService;

        public LoadSchedulingController(LoadSchedulingService loadScheduling, CustomerService customerService, CityService cityService, DriverService driverService, StateService stateService)
        {
            _loadSchedulingService = loadScheduling;
            _customerService = customerService;
            _cityService = cityService;
            _driverService = driverService;
            _stateService = stateService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _loadSchedulingService.FindAllAsync();
            return View(list);
        }
                
        // Lista pela data de inclusão
        public async Task<IActionResult> DateSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            ViewData["minDate"] = minDate.Value.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.Date.ToString("yyyy-MM-dd");
            var result = await _loadSchedulingService.FindByIncludeDateAsync(minDate, maxDate);
            return View(result);
        }
          
        //GET to index -- Paging and filter -- Buscar cidade de origem
        public async Task<IActionResult> OthersSearch(string filter, int pageindex = 1, string sort = "IncludeDate")
        {
            var resultado = _loadSchedulingService.FindByOriginDestinyDriverAsync(filter);
            
            var model = await PagingList.CreateAsync(resultado.Result, 5, pageindex, sort, "IncludeDate");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(model);
        }

        // GET: LoadScheduling/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.states = await _stateService.FindAllAsync();
                
                var citiesList = new List<City>();
                citiesList.Add(new City());
                
                ViewBag.cities = citiesList;
                
                var customers = await _customerService.FindAllAsync();
                var drivers = await _driverService.FindAllAsync();
                var viewModel = new LoadSchedulingFormViewModel { Customer = customers, Driver = drivers };
                return View(viewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoadScheduling loadScheduling)
        {
            if (ModelState.IsValid)
            {
                var customers = await _customerService.FindAllAsync();
                var cities = await _cityService.FindAllAsync();
                var drivers = await _driverService.FindAllAsync();
                var viewModel = new LoadSchedulingFormViewModel { Customer = customers, City = cities, Driver = drivers };
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
                    message = e.Message + "\n" + 
                    "Dados obrigatórios não preenchidos"
                    + "Cliente: " + loadScheduling.CustomerId.ToString() 
                    + "Origem : " + loadScheduling.CityDestinyId.ToString()
                    + "Destino: " + loadScheduling.CityOriginId.ToString()
                    + "Motorista: " + loadScheduling.DriverId.ToString()
                });
            }

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" });
            }
            var loadSchedule = await _loadSchedulingService.FindByIdAsync(id.Value);
            if (loadSchedule == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }
            return View(loadSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não fornecido" });
            }
            var obj = await _loadSchedulingService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
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
            List<Driver> drivers = await _driverService.FindAllAsync();
            var viewModel = new LoadSchedulingFormViewModel { LoadScheduling = loadSchedule, Customer = customers, City = cities, Driver = drivers };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LoadScheduling loadScheduling)
        {
            if (ModelState.IsValid)
            {

                var customers = await _customerService.FindAllAsync();
                var cities = await _cityService.FindAllAsync();
                var drivers = await _driverService.FindAllAsync();
                var viewModel = new LoadSchedulingFormViewModel { Customer = customers, City = cities, Driver = drivers };
                return View(viewModel);

            }
            if (id != loadScheduling.Id)
            {
                return RedirectToAction(nameof(Error), new
                {
                    message = "ID não fornecido  = ID: " + loadScheduling.Id.ToString()
                });
            }

            try
            {
                await _loadSchedulingService.UpdateAsync(loadScheduling);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
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
