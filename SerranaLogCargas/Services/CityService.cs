using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using LogCargas.Services.Exceptions;
using OfficeOpenXml;

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
        // importação das cidades

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

        // Ler a stream do arquivo e criar a lista
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
                            city.StateId = Convert.ToInt32(worksheet.Cells[linha, 2].Value);

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
                    _context.Add(cities);
                    await _context.SaveChangesAsync();
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
