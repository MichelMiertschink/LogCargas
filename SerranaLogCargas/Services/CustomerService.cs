using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using System.ComponentModel;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using LogCargas.Services.Exceptions;

namespace LogCargas.Services
{
    public class CustomerService
    {
        private readonly LogCargasContext _context;
        public CustomerService (LogCargasContext context)
        {
            _context = context;
        }
        public async Task<List<Customer>> FindAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> FindByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task<Customer> FindByCnpjAsync(Customer customer)
        {
            return await _context.Customers.FirstOrDefaultAsync(obj => obj.CNPJ == customer.CNPJ);
        }
        // Create
        public async Task InsertAsync(Customer customer)
        {
            _context.Add(customer);
            await _context.SaveChangesAsync();
        }
        // Edit
        public async Task UpdateAsync(Customer customer)
        {
            bool hasAny = await _context.Customers.AnyAsync(x => x.Id == customer.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrada - UpdateAsyncService");
            }

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message + "UpdateAsync");
            }
        }

        // Importar arquivo de clientes (Excel)
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
        public List<Customer> LerXls(MemoryStream stream)
        {
            try
            {
                var resposta = new List<Customer>();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int numeroLinhas = worksheet.Dimension.End.Row;

                    for (int linha = 2; linha <= numeroLinhas; linha++)
                    {
                        var customer = new Customer();

                        if (worksheet.Cells[linha, 1].Value != null
                            && worksheet.Cells[linha, 2].Value != null)
                        {
                            customer.Id = 0;
                            customer.CNPJ = worksheet.Cells[linha, 1].Value.ToString();
                            customer.CorporateReason = worksheet.Cells[linha, 2].Value.ToString();
                            customer.CostCenter = worksheet.Cells[linha, 3].Value.ToString();

                            resposta.Add(customer);
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
        public async Task SalvarImportacao(List<Customer> customers)
        {
            try
            {
                foreach (var customer in customers)
                {
                    bool hasAny = await _context.Customers.AnyAsync(x => x.CNPJ == customer.CNPJ);
                    if (!hasAny)
                    {
                        await InsertAsync(customer);
                    }
                    else
                    {
                        Customer obj = await FindByCnpjAsync(customer);
                        customer.Id = obj.Id;
                        await UpdateAsync(customer);
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

