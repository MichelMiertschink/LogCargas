using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LogCargas.Models
{
    public class RedeFrota
    {
        // not Rede Frota
        public DateTime IncludeDate { get; set; }

        // Rede Frota
        [Key]
        [JsonPropertyName("codigoTransacao")]
        public int codigoTransacao { get; set; }

        [JsonPropertyName("OwnerId")]
        public string OwnerId { get; set; }
        
        [JsonPropertyName("dataTransacao ")]
        public DateTime dataTransacao { get; set; }
        
        [JsonPropertyName("NumeroCartao")]
        public string NumeroCartao { get; set; }
        
        [JsonPropertyName("Placa")]
        public string Placa { get; set; }
       
        [JsonPropertyName("quilometragem")]
		public int quilometragem { get; set; }
       
        [JsonPropertyName("TipoCombustivel")]
		public string TipoCombustivel { get; set; }
        
        [JsonPropertyName("Litros")]
		public int Litros { get; set; }
       
        [JsonPropertyName("valorTransacao")]
		public float valorTransacao { get; set; }
      
        [JsonPropertyName("odometro")]
		public int odometro { get; set; }
       
        [JsonPropertyName("EstabelecimentoCNPJ")]
		public string EstabelecimentoCNPJ { get; set; }
        
        [JsonPropertyName("NomeReduzido")]
		public string NomeReduzido { get; set; }
       
        [JsonPropertyName("NomeCidade")]
		public string NomeCidade { get; set; }
        
        [JsonPropertyName("Parcial")]
		public bool Parcial { get; set; }
        
        [JsonPropertyName("CpfMmotorista")]
		public string CpfMmotorista { get; set; }
        
        [JsonPropertyName("Requisicao")]
		public string Requisicao { get; set; }
        
        [JsonPropertyName("Manifesto")]
        public string Manifesto { get; set; }
    }
}
