using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using LogCargas.Services.Exceptions;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;


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

        public async Task<City> FindByNameAsync(string name)
        {
            return await _context.Cities.Include(obj => obj.State).FirstOrDefaultAsync(obj => obj.Name == name);
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

        // Importar arquivo Excel
        public MemoryStream LerStream(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                formFile?.CopyTo(stream);
                var ListBytes = stream.ToArray();
                return new MemoryStream(ListBytes);
            }
        }

        public List<City> LerXls(MemoryStream stream)
        {
            try
            {
                var resposta = new List<City>();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int numeroLinhas = worksheet.Dimension.End.Row;

                    for (int linha = 2; linha <= numeroLinhas; linha++)
                    {
                        var city = new City();

                        if (worksheet.Cells[linha, 1].Value != null
                            && worksheet.Cells[linha, 2].Value != null)
                        {
                            city.Id = 0;
                            city.Name = worksheet.Cells[linha, 1].Value.ToString();
                            city.StateId = int.Parse(worksheet.Cells[linha, 2].Value.ToString());

                            resposta.Add(city);
                        }
                    }
                }

                return resposta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SalvarImportacao(List<City> cities)
        {
            try
            {
                foreach (var city in cities)
                {
                    bool hasAny = await _context.Cities.AnyAsync(x => x.Name == city.Name);
                    if (!hasAny)
                    {
                        await InsertAsync(city);
                    }
                    else
                    {
                        City obj = await FindByNameAsync(city.Name);
                        city.Id = obj.Id;
                        await UpdateAsync(city);
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
