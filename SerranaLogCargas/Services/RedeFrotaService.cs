using LogCargas.Data;
using LogCargas.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LogCargas.Services
{
    public class RedeFrotaService
    {

        private readonly LogCargasContext _context;

        public RedeFrotaService(LogCargasContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<RedeFrota>> FindForAll(string filter)
        {
            var resultado = _context.RedeFrota.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                resultado = resultado.Where(r => r.Placa.Contains(filter) ||
                                                 r.codigoTransacao.ToString().Contains(filter));
            }
            return resultado.AsQueryable();
        }

        public async Task InsertAsync(RedeFrota redeFrota)
        {
            redeFrota.IncludeDate = DateTime.Now;
            _context.Add(redeFrota);
            await _context.SaveChangesAsync();
        }
    }
}
