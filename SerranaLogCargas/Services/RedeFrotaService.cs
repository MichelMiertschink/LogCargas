using AutoMapper;
using LogCargas.Data;
using LogCargas.Dtos;
using LogCargas.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LogCargas.Services
{
    public class RedeFrotaService
    {

        private readonly LogCargasContext _context;
        private readonly IMapper _mapper;

        public RedeFrotaService(LogCargasContext context , IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseGenerico<RedeFrotaResponse>> FindRedeFrota(string filter)
        {
            var redeFrota = await _context.RedeFrota.FindAsync();
            return _mapper.Map<ResponseGenerico<RedeFrotaResponse>>(redeFrota);
        }

        public async Task<List<RedeFrota>> FindAllAsync()
        {
            return await _context.RedeFrota.ToListAsync();
        }

        public async Task<IQueryable<RedeFrota>> FindRedeFrotaBetweenDate(string filter)
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
