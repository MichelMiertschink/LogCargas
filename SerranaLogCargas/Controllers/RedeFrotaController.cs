using ClosedXML.Excel;
using LogCargas.Models;
using LogCargas.Services;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics.Metrics;
using Humanizer;
using DocumentFormat.OpenXml.Bibliography;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using LogCargas.Mappings;

namespace LogCargas.Controllers
{
    public class RedeFrotaController : Controller
    {
        private readonly RedeFrotaService _redeFrotaService;

        public RedeFrotaController(RedeFrotaService redeFrota)
        {
            _redeFrotaService = redeFrota;
        }

        public async Task<IActionResult> Index(DateTime? minDate, DateTime? maxDate, int qtdPaging, int pageindex = 1, string sort = "dataTransacao")
        {
            if(qtdPaging == null)
            {
                qtdPaging = 20;
            }

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
            var resultado = await _redeFrotaService.FindRedeFrotaBetweenDate(minDate, maxDate);

            var model = await PagingList.CreateAsync(resultado, qtdPaging, pageindex, sort, "dataTransacao");
            model.RouteValue = new RouteValueDictionary { { "minDate", minDate }, { "maxDate", maxDate}, { "qtdPaging", qtdPaging }, { "total", resultado.Count()} };
            return View(model);
        }

        // Busca inteligente
        public async Task<IActionResult> FindAllRefuelling(string filter, int pageindex = 1, string sort = "dataTransacao")
        {
            var resultado = _redeFrotaService.FindAllRefuelling(filter);

            var model = await PagingList.CreateAsync(resultado.Result, 50, pageindex, sort, "dataTransacao");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };
            return View(model);
        }

        public async Task<IActionResult> BuscarRedeFrota(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            string iniDate = minDate.Value.Date.ToString("yyyy-MM-ddT00:00:00.000Z");
            string fimDate = maxDate.Value.Date.ToString("yyyy-MM-ddT23:59:59.999Z");
            var result = await _redeFrotaService.BuscarRedeFrota(iniDate, fimDate);
            return View(result);
        }


        [HttpPost("&{dta_inicio}&{dta_final}")]
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
        public async Task<IActionResult> ExportaBsoftXLSX(DateTime? minDate, DateTime? maxDate)

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

            // Faz a busca no banco de dados
            var result = await _redeFrotaService.FindRedeFrotaBetweenDate(minDate, maxDate);

            // Monta o DataTable para exportação através de XLSX -- Inicio
            DataTable dataTable = new DataTable();
            dataTable.TableName = "Exporta RedeFrota"
                + minDate
                + "_"
                + maxDate;

            dataTable.Columns.Add("DOCUMENTO", typeof(string));
            dataTable.Columns.Add("DATA", typeof(string));
            dataTable.Columns.Add("PLACA_VEICULO", typeof(string));
            dataTable.Columns.Add("CODIGO_DESPESA", typeof(string));
            dataTable.Columns.Add("DESCRICAO_DESPESA", typeof(string));
            dataTable.Columns.Add("CNPJ_FORNECEDOR", typeof(string));
            dataTable.Columns.Add("QUANTIDADE", typeof(string));
            dataTable.Columns.Add("VALOR_UNITARIO", typeof(string));
            dataTable.Columns.Add("VALOR_TOTAL", typeof(string));
            dataTable.Columns.Add("TIPO_PAGAMENTO", typeof(string));
            dataTable.Columns.Add("PREVISAO_PAGAMENTO", typeof(string));
            dataTable.Columns.Add("HODOMETRO", typeof(string));
            dataTable.Columns.Add("HORIMETRO", typeof(string));
            dataTable.Columns.Add("DESCONTAR_COMISSAO", typeof(string));
            dataTable.Columns.Add("ABASTECIMENTO_COMPLETO", typeof(string));
            dataTable.Columns.Add("OBSERVACAO", typeof(string));
            dataTable.Columns.Add("CPF_MOTORISTA", typeof(string));


            string vazio = "";
            string codDespesaDiesel = "173";
            string codDespesaArla = "10";

            if (result.Count() > 0)
            {
                foreach (var abastecimentos in result)
                {
                    dataTable.Rows.Add(abastecimentos.codigoTransacao
                                       , abastecimentos.dataTransacao.ToString("dd/MM/yyyy")
                                       , abastecimentos.Placa.Replace("-", "").ToString()
                                       , abastecimentos.TipoCombustivel.Equals("DIESEL") ? codDespesaDiesel : codDespesaArla
                                       , vazio
                                       , abastecimentos.EstabelecimentoCNPJ.PadLeft(14, '0')

                                       // Codificação do CNPJ para mandar formatado.

                                       //abastecimentos.EstabelecimentoCNPJ.Substring(0, 2)
                                       //+ "." + abastecimentos.EstabelecimentoCNPJ.Substring(3, 3)
                                       //+ "." + abastecimentos.EstabelecimentoCNPJ.Substring(7, 3)
                                       //+ "/" + abastecimentos.EstabelecimentoCNPJ.Substring(8, 4)
                                       //+ "-" + abastecimentos.EstabelecimentoCNPJ.Substring(9, 2)

                                       , abastecimentos.Litros
                                       , abastecimentos.valorTransacao / abastecimentos.Litros
                                       , abastecimentos.valorTransacao
                                       , vazio
                                       , vazio
                                       , abastecimentos.odometro
                                       , vazio
                                       , vazio
                                       , abastecimentos.Parcial.Equals(true) ? "S" : "N"
                                       , "Cidade: " + abastecimentos.NomeCidade + " - Importado pelo Rede Frota"
                                       );
                }
            }
            // Monta o DataTable para exportação através de XLSX -- FIM

            // Cria o arquivo XLSX
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dataTable, "Importar no Bsoft");
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.ms-excel", "Importacao Rede Frota.XLSX");
                }
            }
        }
        public async void ExportaBsoftCSV(DateTime? minDate, DateTime? maxDate)

        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            }
            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            if (minDate > maxDate) minDate = maxDate;

            ViewData["minDate"] = minDate.Value.Date.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.Date.ToString("yyyy-MM-dd");

            // Faz a busca no banco de dados
            var result = await _redeFrotaService.FindRedeFrotaBetweenDate(minDate, maxDate);

            var nomePasta = "C:\\BsoftSistemas\\";
            var nomeArquivo = "Arquivo_123456.csv";
            

            using (var streamWriter = new StreamWriter(Path.Combine(nomePasta, nomeArquivo)))
            using (var csvWriter = new CsvWriter(streamWriter, new CultureInfo("pt-BR", true)))
            {
                
                csvWriter.Context.RegisterClassMap<CsvRedeFrota>();
                
                csvWriter.WriteRecords(result);
                streamWriter.Flush();
            }
        }
    }
}
