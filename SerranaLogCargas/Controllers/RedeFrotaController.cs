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

        
        public async Task<IActionResult> Index()
        {
            var list = await _redeFrotaService.FindAllAsync();
            return View(list);
        }

        //public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "dataTransacao")
        //{
        //    var resultado = await _redeFrotaService.FindForAll(filter);

        //    var model = await PagingList.CreateAsync(resultado.Result, 5, pageindex, sort, "dataTransacao");
        //    model.RouteValue = new RouteValueDictionary { { "filter", filter } };
        //    return View(model);
        //}

        [HttpGet("/{datebetween}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarRedeFrota(DateTime? minDate, DateTime? maxDate)
        {
            string cliente = "17595";
            ViewData["minDate"] = minDate.Value.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.Date.ToString("yyyy-MM-dd");

            string consulta = "cliente=" + cliente
                            + "dta_inicio=" + minDate 
                            + "dta_final="  + maxDate;


                var response = await _redeFrotaService.FindRedeFrota(consulta);

            

            if (response.CodigoHttp == System.Net.HttpStatusCode.OK)
            {
                return Ok(response.DadosRetorno);
            }
            else
            {
                return StatusCode((int)response.CodigoHttp, response.ErroRetorno);
            }
        }

    }
}
