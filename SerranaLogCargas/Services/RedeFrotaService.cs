using AutoMapper;
using LogCargas.Data;
using LogCargas.Dtos;
using LogCargas.Interfaces;
using LogCargas.Models;
using LogCargas.Models.ViewModels;
using LogCargas.Services.Exceptions;
using MessagePack.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

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

        public async Task<List<RedeFrota>> ExportRedeFrotaBetwewDate(DateTime? minDate, DateTime? maxDate)
        {
            var result = await _context.RedeFrota.Include(
                x => x.dataTransacao>=minDate || 
                x.dataTransacao<=maxDate).ToListAsync();
            
            return result;
        }

        public async Task InsertAsync(RedeFrota redeFrota)
        {
             redeFrota.includeDate = DateTime.Now;
            _context.Add(redeFrota);
            await _context.SaveChangesAsync();
        }

        public async Task<RedeFrota> FindByCodTransacaoAsync(int codTtransacao)
        {
            return await _context.RedeFrota.FirstOrDefaultAsync(obj => obj.codigoTransacao == codTtransacao);
        }

        public async Task RemoveAsync(int id)
        {
            var abastecimento = _context.RedeFrota.Find(id);
            _context.RedeFrota.Remove(abastecimento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RedeFrota abastecimento)
        {
            bool hasAny = await _context.RedeFrota.AnyAsync(x => x.codigoTransacao == abastecimento.codigoTransacao);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrada");
            }

            try
            {
                _context.Update(abastecimento);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        // Busca dos dados da API do Redefrota
        public async Task<ResponseGenerico<RedeFrota>> BuscarRedeFrota(string dta_inicio, string dta_final)
        {
            var redeFrota = await _redeFrotaApi.BuscarPorData(dta_inicio, dta_final);
            var abastecimentos = redeFrota.DadosRetorno;
            foreach (var item in abastecimentos)
            {
                try
                {
                    await UpdateAsync(item);
                }catch
                {
                    await InsertAsync(item);
                }
            }
            return redeFrota;
        }

       
    }
}
