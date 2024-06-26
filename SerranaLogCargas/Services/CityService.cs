﻿using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using LogCargas.Services.Exceptions;

namespace LogCargas.Services
{
    public class CityService
    {
        private readonly LogCargasContext _context;
        public CityService(LogCargasContext context)
        {
            _context = context;
        }

        public async Task<List<City>> FindAllAsync()
        {
            return await _context.Cities.Include(obj => obj.State).ToListAsync();
        }

        public async Task InsertAsync(City obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<City> FindByIdAsync(int id)
        {
            return await _context.Cities.Include(obj => obj.State).FirstOrDefaultAsync(obj => obj.Id == id);
        }


        public async Task Remove(int id)
        {
            try
            {
                var obj = _context.Cities.Find(id);
                _context.Cities.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (IntegrityException e)
            {
                throw new DbConcurrencyException("Não é possível excluir, pois  a cidade possui CARGA cadastrada");
            }
        }

        public async Task UpdateAsync(City obj)
        {
            bool hasAny = await _context.Cities.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrada");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
