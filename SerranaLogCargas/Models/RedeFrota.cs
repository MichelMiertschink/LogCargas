using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LogCargas.Models
{
    public class RedeFrota
    {
        //public DateTime includeDate {  get; set; }
        [Key]
        [JsonPropertyName("codigoTransacao")]
        [Display(Name = "Cod")]
        public int? codigoTransacao { get; set; }

        [JsonPropertyName("OwnerId")]
        [Display(Name = "Proprietário")]
        public string? OwnerId { get; set; }
        
        [JsonPropertyName("dataTransacao ")]
        [Display(Name = "Dt Trans")]
        public DateTime dataTransacao { get; set; }
        
        [JsonPropertyName("NumeroCartao")]
        [Display(Name = "Cartão")]
        public string NumeroCartao { get; set; }
        
        [JsonPropertyName("Placa")]
        [Display(Name = "Placa")]
        public string Placa { get; set; }
       
        [JsonPropertyName("quilometragem")]
        [Display(Name = "Km")]
        public int quilometragem { get; set; }
       
        [JsonPropertyName("TipoCombustivel")]
        [Display(Name = "Tp Combust")]
        public string TipoCombustivel { get; set; }
        
        [JsonPropertyName("Litros")]
        [Display(Name = "Litros")]
        public int Litros { get; set; }
       
        [JsonPropertyName("valorTransacao")]
        [Display(Name = "Valor Abast.")]
        public float valorTransacao { get; set; }
      
        [JsonPropertyName("odometro")]
        [Display(Name = "Odometro")]
        public int odometro { get; set; }
       
        [JsonPropertyName("EstabelecimentoCNPJ")]
        [Display(Name = "CNPJ Fornecedor")]
        public string EstabelecimentoCNPJ { get; set; }
        
        [JsonPropertyName("NomeReduzido")]
        [Display(Name = "Nome Posto")]
        public string NomeReduzido { get; set; }
       
        [JsonPropertyName("NomeCidade")]
        [Display(Name = "Cidade Abast.")]
        public string NomeCidade { get; set; }
        
        [JsonPropertyName("Parcial")]
		public bool Parcial { get; set; }
        
        [JsonPropertyName("CpfMmotorista")]
        [Display(Name = "CPF Motorista")]
        public string CpfMmotorista { get; set; }
        
        [JsonPropertyName("Requisicao")]
        [Display(Name = "Requisição")]
        public string Requisicao { get; set; }
        
        [JsonPropertyName("Manifesto")]
        [Display(Name = "Manifesto")]
        public string Manifesto { get; set; }
    }
}
