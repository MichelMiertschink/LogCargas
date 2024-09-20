using AutoMapper;
using LogCargas.Data;
using LogCargas.Dtos;
using LogCargas.Interfaces;
using LogCargas.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;

namespace LogCargas.Services
{
    public class RedeFrotaService : IRedeFrotaService
    {

        private readonly LogCargasContext _context;
        private readonly IMapper _mapper;
        private readonly IRedeFrotaApi _redeFrotaApi;

        public RedeFrotaService(LogCargasContext context, IMapper mapper, IRedeFrotaApi redeFrotaApi)
        {
            _context = context;
            _mapper = mapper;
            _redeFrotaApi = redeFrotaApi;
        }

        public async Task<ResponseGenerico<RedeFrotaResponse>> FindRedeFrota(string filter)
        {
            var redeFrota = await _context.RedeFrota.FindAsync();
            return _mapper.Map<ResponseGenerico<RedeFrotaResponse>>(redeFrota);
        }

        public async Task<IQueryable<RedeFrota>> FindRedeFrotaBetweenDate(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.RedeFrota select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.dataTransacao.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.dataTransacao.Date <= maxDate.Value);
            }

            return result;
        }

        public async Task InsertAsync(RedeFrota redeFrota)
        {
            //redeFrota.IncludeDate = DateTime.Now;
            _context.Add(redeFrota);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseGenerico<RedeFrotaResponse>> BuscarRedeFrota(string dta_inicio, string dta_final)
        {
            var redeFrota = await _redeFrotaApi.BuscarPorData(dta_inicio, dta_final);

            return _mapper.Map<ResponseGenerico<RedeFrotaResponse>>(redeFrota);
        }

    }
}
