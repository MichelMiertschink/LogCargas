﻿using LogCargas.Models;

namespace LogCargas.Data
{
    public class SeedingService
    {
        private LogCargasContext _context;

        public SeedingService(LogCargasContext context)
        {
            _context = context; 
        }

        public void Seed()
        {
            if (_context.States.Any())
            {
                return; // Banco de dados já está polulado
            }

            State e1 = new State(1, "AC", "Acre");
            State e2 = new State(2, "AL", "Alagoas");
            State e3 = new State(3, "AP", "Amapá");
            State e4 = new State(4, "AM", "Amazonas");
            State e5 = new State(5, "BA", "Bahia");
            State e6 = new State(6, "CE", "Ceará");
            State e7 = new State(7, "DF", "Distrito Federal");
            State e8 = new State(8, "ES", "Espírito Santo");
            State e9 = new State(9, "GO", "Goiás");
            State e10 = new State(10, "MA", "Maranhão");
            State e11 = new State(11, "MT", "Mato Grosso");
            State e12 = new State(12, "MS", "Mato Grosso do Sul");
            State e13 = new State(13, "MG", "Minas Gerais");
            State e14 = new State(14, "PA", "Pará");
            State e15 = new State(15, "PB", "Paraíba");
            State e16 = new State(16, "PR", "Paraná");
            State e17 = new State(17, "PE", "Pernambuco");
            State e18 = new State(18, "PI", "Piauí");
            State e19 = new State(19, "RJ", "Rio de Janeiro");
            State e20 = new State(20, "RN", "Rio Grande do Norte");
            State e21 = new State(21, "RS", "Rio Grande do Sul");
            State e22 = new State(22, "RO", "Rondônia");
            State e23 = new State(23, "RR", "Roraima");
            State e24 = new State(24, "SC", "Santa Catarina");
            State e25 = new State(25, "SP", "São Paulo");
            State e26 = new State(26, "SE", "Sergipe");
            State e27 = new State(27, "TO", "Tocantins");

            _context.AddRange(e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, e16, e17, e18, e19, e20, e21, e22, e23, e24, e25, e26, e27);

            _context.SaveChanges();
        }
    }
}
