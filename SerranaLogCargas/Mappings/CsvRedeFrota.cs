using CsvHelper.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
using LogCargas.Models;

namespace LogCargas.Mappings
{
    public class CsvRedeFrota : ClassMap<RedeFrota>
    {
        public CsvRedeFrota()
        {
            Map(m => m.codigoTransacao).Name("DOCUMENTO");
            Map(m => m.dataTransacao.Date).Name("DATA");
            Map(m => m.Placa).Name("PLACA");
            Map(m => m.TipoCombustivel).Name("COD_DESPESA");
            Map().Name("DESCRICAO_DESPESA");
            Map(m => m.EstabelecimentoCNPJ).Name("CNPJ_FORNECEDOR");
            Map(m => m.Litros).Name("QUANTIDADE");
            Map().Name("VALOR_UNITARIO");
            Map(m => m.valorTransacao).Name("VALOR_TOTAL");
            Map().Name("TIPO_PAGAMENTO");
            Map().Name("PREVISAO_PAGAMENTO");
            Map(m => m.odometro).Name("HODOMETRO");
            Map().Name("HORIMETRO");
            Map().Name("DESCONTAR_COMISSAO");
            Map(m => m.Parcial).Name("ABASTECIMENTO_COMPLETO");
            Map().Name("OBSERVACAO");
            Map().Name("CPF_MOTORISTA");
        }
    }
}
