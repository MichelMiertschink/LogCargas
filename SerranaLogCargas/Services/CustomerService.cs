using Microsoft.EntityFrameworkCore;
using LogCargas.Data;
using LogCargas.Models;
using System.ComponentModel;
using OfficeOpenXml;

namespace LogCargas.Services
{
    public class CustomerService
    {
        private readonly LogCargasContext _context;
        public CustomerService(LogCargasContext context)
        {
            _context = context;
        }
        public async Task<List<Customer>> FindAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public List<Customer> FindAll()
        {
            return _context.Customers.ToList();
        }

        public async Task InsertAsync(Customer obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
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

        public void SalvarImportacao(List<Customer> customers)
        {
            try
            {
                foreach (var customer in customers)
                {
                    _context.Add(customer);
                    _context.SaveChangesAsync();
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

