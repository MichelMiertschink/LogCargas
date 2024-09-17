using LogCargas.Dtos;
using LogCargas.Models;

namespace LogCargas.Interfaces
{
    public interface IRedeFrotaApi
    {
        Task<ResponseGenerico<RedeFrotaResponse>> BuscarPorData(string cliente, string dta_inicio, string dta_final);
    }
}
