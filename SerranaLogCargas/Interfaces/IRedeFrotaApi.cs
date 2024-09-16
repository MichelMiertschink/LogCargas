using LogCargas.Dtos;
using LogCargas.Models;

namespace LogCargas.Interfaces
{
    public interface IRedeFrotaApi
    {
        Task<ResponseGenerico<RedeFrota>> FindBetweenDate(string dateBetween);
    }
}
