using System.Dynamic;
using System.Net;

namespace LogCargas.Dtos
{
    public class ResponseGenerico<T> where T : class
    {
        public HttpStatusCode StatusCode { get; set; }
        public T? DadosRetorno { get; set; }
        public ExpandoObject? ErroRetorno { get; set; }
    }
}
