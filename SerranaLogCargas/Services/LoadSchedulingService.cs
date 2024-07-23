using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using LogCargas.Services.Exceptions;
using System.Text.RegularExpressions;

namespace LogCargas.Services
{
    public class LoadSchedulingService
    {
        private readonly LogCargasContext _context;
        public LoadSchedulingService(LogCargasContext context)
        {
            _context = context;
        }

        public async Task<List<LoadScheduling>> FindAllAsync()
        {
            return await _context.LoadScheduling
                .Include(cityDestiny => cityDestiny.CityDestiny)
                .Include(cityOrigin => cityOrigin.CityOrigin)
                .Include(customerId => customerId.Customer)
                .Include(driverId => driverId.Driver)
                .OrderByDescending(x => x.IncludeDate)
                .ToListAsync();
        }

        // Filtro pela data de inclusão
        public async Task<List<LoadScheduling>> FindByIncludeDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.LoadScheduling select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.IncludeDate.Date >= minDate.Value);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.IncludeDate.Date <= maxDate.Value);
            }

            return await result
                .Include(cityDestiny => cityDestiny.CityDestiny)
                .Include(cityOrigin => cityOrigin.CityOrigin)
                .Include(customerId => customerId.Customer)
                .Include(driverId => driverId.Driver)
                .ToListAsync();
        }

        // Filtro pela origem OU destino OU motorista
        public async Task<IQueryable<LoadScheduling>> FindByOriginDestinyDriverAsync(string filter)
        {
            var result = from obj in _context.LoadScheduling select obj;
            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(x => x.CityOrigin.Name.Contains(filter));
            }

            return result.AsQueryable()
                .Include(cityDestiny => cityDestiny.CityDestiny)
                .Include(cityOrigin => cityOrigin.CityOrigin)
                .Include(customerId => customerId.Customer)
                .Include(driverId => driverId.Driver);
        }

        // Ordenado por data de descarregamento
        public async Task<List<LoadScheduling>> FindAllOrderCustomerAsync()
        {
            return await _context.LoadScheduling
                .Include(cityDestiny => cityDestiny.CityDestiny)
                .Include(cityOrigin => cityOrigin.CityOrigin)
                .Include(customerId => customerId.Customer)
                .Include(driverId => driverId.Driver)
                .OrderBy(x => x.UnloadDate).ToListAsync();
        }

        public async Task InsertAsync(LoadScheduling loadScheduling)
        {

            loadScheduling.IncludeDate = DateTime.Now;
            _context.Add(loadScheduling);
            await _context.SaveChangesAsync();
        }

        public async Task<LoadScheduling> FindByIdAsync(int id)
        {
            return await _context.LoadScheduling
                .Include(obj => obj.CityOrigin)
                .Include(obj => obj.CityDestiny)
                .Include(obj => obj.Customer)
                .Include(obj => obj.Driver)
                .FirstOrDefaultAsync(obj => obj.Id == id);
        }


        public async Task RemoveAsync(int id)
        {
            var loadScheduling = _context.LoadScheduling.Find(id);
            _context.LoadScheduling.Remove(loadScheduling);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoadScheduling loadScheduling)
        {
            bool hasAny = await _context.LoadScheduling.AnyAsync(x => x.Id == loadScheduling.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrada");
            }

            try
            {
                _context.Update(loadScheduling);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
