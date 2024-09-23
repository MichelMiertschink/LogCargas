using LogCargas.Dtos;
using LogCargas.Models;

namespace LogCargas.Interfaces
{
    public interface IRedeFrotaService
    {
        public interface IRedeFrotaService
        {
            Task<ResponseGenerico<RedeFrota>> BuscarRedefrota(string cliente, string dta_inicio, string dta_final);
        }
    }
}
