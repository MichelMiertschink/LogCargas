using System.Dynamic;
using System.Text.Json;
using LogCargas.Dtos;
using LogCargas.Interfaces;
using LogCargas.Models;
using NuGet.Packaging.Signing;


namespace LogCargas.REST
{
    public class RedeFrotaApiRest : IRedeFrotaApi
    {
        public async Task<ResponseGenerico<RedeFrota>> FindBetweenDate(string dateBetween)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://prd-redefrota-apim.azure-api.net/inteligencia/FormatoGestranTransacao?cliente=17595{dateBetween}");

            var response = new ResponseGenerico<RedeFrota>();
            using (var client = new HttpClient())
            {
                var responseRedeFrotaApiPrimeira = await client.SendAsync(request);
                var contentResp = await responseRedeFrotaApiPrimeira.Content.ReadAsStringAsync();
                var ojbResponse = JsonSerializer.Deserialize<RedeFrota>(contentResp);

                if (responseRedeFrotaApiPrimeira.IsSuccessStatusCode)
                {
                    response.CodigoHttp = responseRedeFrotaApiPrimeira.StatusCode;
                    response.DadosRetorno = ojbResponse;
                }
                else
                {
                    response.CodigoHttp = responseRedeFrotaApiPrimeira.StatusCode;
                    response.ErroRetorno = JsonSerializer.Deserialize<ExpandoObject>(contentResp);
                }
            }

            return response;
        }
    }
}
