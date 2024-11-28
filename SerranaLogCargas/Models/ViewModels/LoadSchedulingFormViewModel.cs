using LogCargas.Models.Enums;

namespace LogCargas.Models.ViewModels
{
    public class LoadSchedulingFormViewModel
    {
        public LoadScheduling LoadScheduling { get; set; }
        public ICollection<Customer> Customer { get; set; }
        public ICollection<City> City { get; set; }
        public ICollection<State> State { get; set; }
        public ICollection<Driver> Driver { get; set; }
        public VehicleType VehicleType { get; set; }
        public RiskManagement RiskManagement { get; set; }

    }
}
