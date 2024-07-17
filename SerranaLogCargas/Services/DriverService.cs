using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using LogCargas.Services.Exceptions;
using System.Linq.Expressions;
using OfficeOpenXml;

namespace LogCargas.Services
{
    public class DriverService
    {
        private readonly LogCargasContext _context;

        public DriverService (LogCargasContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> FindAllAsync()
        {
            return await _context.Driver.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Driver> FindByIdAsync(int id)
        {
            return await _context.Driver.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task<Driver> FindByCpfAsync(string cpf)
        {
            return await _context.Driver.FirstOrDefaultAsync(obj => obj.CPF == cpf);
        }

        // Create
        public async Task InsertAsync(Driver obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }
                
        // Edit
        public async Task UpdateAsync (Driver driver)
        {
            bool hasAny = await _context.Driver.AnyAsync(x => x.Id == driver.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrada");
            }
            
            try
            {
                _context.Update(driver);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        // Delete
        public async Task Remove (int id)
        {
            var driver = _context.Driver.Find(id);
            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();
        }

        // Importar arquivo de motoristas (Excel)
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
        public List<Driver> LerXls(MemoryStream stream)
        {
            try
            {
                var resposta = new List<Driver>();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int numeroLinhas = worksheet.Dimension.End.Row;

                    for (int linha = 2; linha <= numeroLinhas; linha++)
                    {
                        var driver = new Driver();

                        if (worksheet.Cells[linha, 1].Value != null
                            && worksheet.Cells[linha, 2].Value != null)
                        {
                            driver.Id = 0;
                            driver.Name = worksheet.Cells[linha, 1].Value.ToString();
                            driver.CelPhone = worksheet.Cells[linha, 2].Value.ToString();
                            driver.CPF = worksheet.Cells[linha, 3].Value.ToString();

                            resposta.Add(driver);
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

        // Salvar a importação, se não existir o CNPJ
        public async Task SalvarImportacao(List<Driver> drivers)
        {
            try
            {
                foreach (var driver in drivers)
                {
                    bool hasAny = await  _context.Driver.AnyAsync(x => x.CPF == driver.CPF);
                    if (!hasAny)
                    {
                        InsertAsync(driver);
                    }
                    else
                    {
                        Driver obj = await FindByCpfAsync(driver.CPF);
                        driver.Id = obj.Id;
                        await UpdateAsync(driver);
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
