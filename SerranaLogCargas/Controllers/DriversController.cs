using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LogCargas.Models;
using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models.ViewModels;
using LogCargas.Services;

namespace LogCargas.Controllers
{
    public class DriversController : Controller
    {
        private readonly DriverService _driverService;
        public DriversController(DriverService driverService)
        {
            _driverService = driverService;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            var list = await _driverService.FindAllAsync();
            return View(list);
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não fornecido" });
            }

            var driver = await _driverService.FindByIdAsync(id.Value);
            if (driver == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não encontrado" });
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Drivers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Driver driver)
        {
            await _driverService.InsertAsync(driver);
            return RedirectToAction(nameof(Index));
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não fornecido" });
            }

            var driver = await _driverService.FindByIdAsync(id.Value);
            if (driver == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não encontrado" });
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CPF,CelPhone")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _driverService.UpdateAsync(driver);
                }
                catch (ApplicationException e)
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não fornecido" });
            }

            var driver = await _driverService.FindByIdAsync(id.Value);
            if (driver == null)
            {
                return RedirectToAction(nameof(Error), new { message = "ID não encontrado" });
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _driverService.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportDrivers(IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                var streamFile = _driverService.LerStream(formFile);
                var drivers= _driverService.LerXls(streamFile);
                await _driverService.SalvarImportacao(drivers);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
