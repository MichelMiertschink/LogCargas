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
        public async Task<IActionResult> Index(DateTime? minDate, DateTime? maxDate)
        {

            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            ViewData["minDate"] = minDate.Value.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.Date.ToString("yyyy-MM-dd");
            var result = await _redeFrotaService.FindRedeFrotaBetweenDate(minDate, maxDate);
            return View(result);
        }

        [HttpPost("&cliente={cliente}&{dta_inicio}&{dta_final}")]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarRedeFrota([FromRoute] string dta_inicio, string dta_final)
        {
            var response = await _redeFrotaService.BuscarRedeFrota(dta_inicio, dta_final);

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
