using Microsoft.Build.Framework.Profiler;
using NuGet.Packaging.Signing;
using System.Text.Json.Serialization;

namespace LogCargas.Dtos
{
    public class RedeFrotaResponse 
    {
        public int codigoTransacao { get; set; }
        public string OwnerId { get; set; }
        public DateTime dataTransacao { get; set; }
        public string NumeroCartao { get; set; }
        public string Placa { get; set; }
        public int quilometragem { get; set; }
        public string TipoCombustivel { get; set; }
        public int Litros { get; set; }
        public float valorTransacao { get; set; }
        public int odometro { get; set; }
        public string EstabelecimentoCNPJ { get; set; }
        public string NomeReduzido { get; set; }
        public string NomeCidade { get; set; }
        public bool Parcial { get; set; }
        public string CpfMmotorista { get; set; }
        public string Requisicao { get; set; }
        public string Manifesto { get; set; }

    }
}
