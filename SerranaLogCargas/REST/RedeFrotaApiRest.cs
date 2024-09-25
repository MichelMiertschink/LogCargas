using System.Dynamic;
using LogCargas.Dtos;
using LogCargas.Interfaces;
using LogCargas.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;



namespace LogCargas.REST
{
    public class RedeFrotaApiRest : IRedeFrotaApi
    {
        public async Task<ResponseGenerico<RedeFrota>> BuscarPorData(string dta_inicio, string dta_final)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://prd-redefrota-apim.azure-api.net/inteligencia/FormatoGestranTransacao?" +
                $"cliente=17595" +
                $"&dta_inicio={dta_inicio}" +
                $"&dta_final{dta_final}");

            request.Headers.Add("Ocp-Apim-Subscription-Key", "7bb40cd8894b4f0a840726dba63cb543");
            request.Headers.Add("Ocp-Apim-Subscription-Key", "6d579d3d0f5947a1a1bc409d1302b8dd");
            request.Headers.Add("Ocp-Apim-Trace", "true");

            var content = new StringContent("{\r\n    \"cliente\":17595,\r\n    \"dta_inicio\":\"" + $"{dta_inicio}" + "\",\r\n    \"dta_fim\":\"" +
                $"{dta_final}" + "\"\r\n}", null, "application/json");

            request.Content = content;

            var response = new ResponseGenerico<RedeFrota>();
            using (var client = new HttpClient())
            {
                var ResponseApiRedefrota = await client.SendAsync(request);
                var contentResp = await ResponseApiRedefrota.Content.ReadAsStringAsync();
                var objResponse = JsonSerializer.Deserialize<List<RedeFrota>>(contentResp);

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
