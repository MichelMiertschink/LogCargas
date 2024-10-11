using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LogCargas.Models
{
    public class RedeFrota
    {
        [Display(Name = "Dt. Inclusão")]
        public DateTime includeDate {  get; set; }

        [JsonPropertyName("ownerId")]
        [Display(Name = "Proprietário")]
        public string? OwnerId { get; set; }

        [Key]
        [JsonPropertyName("codigoTransacao")]
        [Display(Name = "Cod")]
        public int? codigoTransacao { get; set; }
        
        [JsonPropertyName("dataTransacao")]
        [Display(Name = "Dt Trans")]
        [DataType(DataType.Date)]
        public DateTime dataTransacao { get; set; }
        
        [JsonPropertyName("numeroCartao")]
        [Display(Name = "Cartão")]
        public string? NumeroCartao { get; set; }
        
        [JsonPropertyName("placa")]
        [Display(Name = "Placa")]
        public string? Placa { get; set; }
       
        [JsonPropertyName("quilometragem")]
        [Display(Name = "Km")]
        public int? quilometragem { get; set; }
       
        [JsonPropertyName("tipoCombustivel")]
        [Display(Name = "Tp Combust")]
        public string? TipoCombustivel { get; set; }
        
        [JsonPropertyName("litros")]
        [Display(Name = "Litros")]
        public double? Litros { get; set; }
       
        [JsonPropertyName("valorTransacao")]
        [Display(Name = "Valor Abast.")]
        [DataType(DataType.Currency)]
        public double? valorTransacao { get; set; }
      
        [JsonPropertyName("odometro")]
        [Display(Name = "Odometro")]
        public int? odometro { get; set; }
       
        [JsonPropertyName("estabelecimentoCNPJ")]
        [Display(Name = "CNPJ Fornecedor")]
        public string? EstabelecimentoCNPJ { get; set; }
        
        [JsonPropertyName("nomeReduzido")]
        [Display(Name = "Nome Posto")]
        public string? NomeReduzido { get; set; }
       
        [JsonPropertyName("nomeCidade")]
        [Display(Name = "Cidade Abast.")]
        public string? NomeCidade { get; set; }
        
        [JsonPropertyName("parcial")]
        [Display(Name = "Parcial")]
		public bool? Parcial { get; set; }
        
        [JsonPropertyName("cpfMotorista")]
        [Display(Name = "CPF Motorista")]
        public string? CpfMmotorista { get; set; }
        
        [JsonPropertyName("requisicao")]
        [Display(Name = "Requisição")]
        public string? Requisicao { get; set; }
        
        [JsonPropertyName("manifesto")]
        [NotMapped]
        [Display(Name = "Manifesto")]
        public string? Manifesto { get; set; }
    }
}
