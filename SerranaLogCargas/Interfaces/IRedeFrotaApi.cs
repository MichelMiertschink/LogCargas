using LogCargas.Dtos;
using LogCargas.Models;

namespace LogCargas.Interfaces
{
    public interface IRedeFrotaApi
    {
        Task<ResponseGenerico<RedeFrota>> BuscarPorData(string dta_inicio, string dta_final);
    }
}
