using LogCargas.Dtos;

namespace LogCargas.Interfaces
{
    public interface IRedeFrotaService
    {
        public interface IRedeFrotaService
        {
            Task<ResponseGenerico<RedeFrotaResponse>> BuscarRedefrota(string cliente, string dta_inicio, string dta_final);
        }
    }
}
