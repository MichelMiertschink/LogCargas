using AspNetCore.Mvc;
using LogCargas.Models;
using LogCargas.Services;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace LogCargas.Controllers
{
    public class RedeFrotaController : Controller
    {
        private readonly RedeFrotaService _redeFrotaService;

        public RedeFrotaController(RedeFrotaService redeFrota)
        {
            _redeFrotaService = redeFrota;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "dataTransacao")
        {
            var resultado = await _redeFrotaService.FindForAll(filter);

            var model = await PagingList.CreateAsync(resultado.Result, 5, pageindex, sort, "dataTransacao");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(model);
        }
    }
}
