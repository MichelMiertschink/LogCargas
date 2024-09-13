using LogCargas.Dtos;
using LogCargas.Models;
using NuGet.Packaging.Signing;
using System.Dynamic;
using System.Text.Json;

namespace LogCargas.REST
{
    public class RedeFrotaApiRest
    {
        public Task<ResponseGenerico<List<RedeFrota>>> BuscarTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseGenerico<List<RedeFrota>>> BuscarPorData(string dtInicioDtFim) 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://prd-redefrota-apim.azure-api.net/inteligencia/FormatoGestranTransacao?cliente=17595{dtInicioDtFim}");

            var response = new ResponseGenerico<List<RedeFrota>>();
            using (var client = new HttpClient())
            {
                var responseRedeFrotaApiPrimeira = await client.SendAsync(request);
                var ContentResp = await responseRedeFrotaApiPrimeira.Content.ReadAsStringAsync();
                var ojbResponse = JsonSerializer.Deserialize<RedeFrota>(ContentResp);

                if (responseRedeFrotaApiPrimeira.IsSuccessStatusCode)
                {
                    response.StatusCode = responseRedeFrotaApiPrimeira.StatusCode;
                    response.ErroRetorno = ojbResponse;
                } else
                {
                    response.StatusCode = responseRedeFrotaApiPrimeira.StatusCode;
                    response.DadosRetorno = JsonSerializer.Deserialize<ExpandoObject>(ContentResp);
                }
            }

            return response;
        }
    }
}
