using LogCargas.Data;
using LogCargas.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace LogCargas.Models
{
    public class Import
    {

        private readonly CityService _cityService;

        public Import(CityService cityService, SeedingService seedingService)
        {
            _cityService = cityService;
            SeedingService = seedingService;
        }

        public SeedingService SeedingService { get; }
        public Import()
        {
        }
        public Import(SeedingService seedingService)
        {
            SeedingService = seedingService;
        }        
    }
}
