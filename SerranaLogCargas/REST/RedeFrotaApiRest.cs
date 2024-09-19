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
        public async Task<ResponseGenerico<RedeFrotaResponse>> BuscarPorData(string dta_inicio, string dta_final)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://prd-redefrota-apim.azure-api.net/inteligencia/FormatoGestranTransacao?" +
                $"cliente=17595" +
                $"&dta_inicio={dta_inicio}" +
                $"&dta_final{dta_final}");

            request.Headers.Add("Ocp-Apim-Subscription-Key", "1f568d7faeec4d069b7f74343ecfdc5c");
            request.Headers.Add("Ocp-Apim-Trace", "true");

            var response = new ResponseGenerico<RedeFrotaResponse>();
            using (var client = new HttpClient())
            {
                var ResponseApiRedefrota = await client.SendAsync(request);
                var contentResp = await ResponseApiRedefrota.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<RedeFrotaResponse>(contentResp);

                if (ResponseApiRedefrota.IsSuccessStatusCode)
                {
                    response.CodigoHttp = ResponseApiRedefrota.StatusCode;
                    response.DadosRetorno = objResponse;
                }
                else
                {
                    response.CodigoHttp = ResponseApiRedefrota.StatusCode;
                    response.ErroRetorno = JsonSerializer.Deserialize<ExpandoObject>(contentResp);
                }
            }
            return response;
        }
    }
}
