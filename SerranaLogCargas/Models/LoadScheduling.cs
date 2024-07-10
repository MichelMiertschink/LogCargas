using LogCargas.Models.Enums;
using System.ComponentModel.DataAnnotations;
using WebBase.Models;

namespace LogCargas.Models
{
    public class LoadScheduling : ModelBase
    {
       
        [Display(Name = "BOL")]
        public bool Bol {  get; set; }

        [Display(Name = "Cliente")]
        public Customer Customer { get; set; }
        [Display(Name = "Cliente")]
        public int CustomerId { get; set; }

        [Display(Name = "Origem")]
        public City CityOrigin { get; set; }
        [Display(Name = "Origem")]
        public int CityOriginId { get; set; }

        [Display(Name = "Destino")]
        public City CityDestiny { get; set; }
        [Display(Name = "Destino")]
        public int CityDestinyId { get; set; }

        [Display(Name = "Dt.Descarga")]
        [DataType(DataType.Date)]
        public DateTime UnloadDate { get; set; }
        [Display(Name ="PD")]
        // Peso de descarga necessário
        public bool PD { get; set; }

        [Display(Name = "Motorista")]
        public Driver Driver { get; set; }
        [Display(Name = "Motorista")]
        public int DriverId { get; set; }

        [Display(Name ="G")]
        public VehicleType VehicleType { get; set; }

        [Display(Name ="C")]
        public RiskManagement? RiskManagement { get; set; }

        // se é monitorado tem que ter Check list
        [Display(Name = "CK")]
        public bool CheckList { get; set; }

        [Display(Name ="M")]
        public bool Monitoring { get; set; }

        [Display(Name = "Peso")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public float Weigth { get; set; }

        [Display(Name = "Vl.Transp")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float VlTranspor {  get; set; }

        [Display(Name = "Vl.Contr.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float VlContract {  get; set; }

        [Display(Name = "Vl.Adiant.")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public float Vladvance {  get; set; }

        [Display(Name = "R$")]
        public bool Pay {  get; set; }

        [Display(Name = "Contrato")]
        public int ContractId { get; set; }

        [Display(Name = "CTe")]
        public bool Cte {  get; set; }

        public LoadScheduling ()
        {
        }


    }
}
