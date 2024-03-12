using System;

namespace SdkCreaComercial.Business.Sdk
{
    public class SerieCapa
    {
        public tSeriesCapas SdkSeriesCapas { get; set; }
        public double Unidades { get; set; }
        public string NumeroLote { get; set; }
        public string FechaCaducidad { get; set; }
        public string FechaFabricacion { get; set; }
        public double TipoCambio { get; set; }
        public string Series { get; set; }
        public string Pedimento { get; set; }
        public string Agencia { get; set; }
        public string FechaPedimento { get; set; }
        public string Error { get; set; }
        public int FilaExcel { get; set; }
        public string ClaveSat { get; set; } = "";

        public SerieCapa(int fila)
        {
            FilaExcel = fila;
        }

        public void CrearSerieCapaSdk(double unidades, string numeroLote, string fechaCaducidad, string fechaFabricacion,
            double tipoCambio, string series, string pedimento, string agencia, string fechaPedimento)
        {
            var sdkSerieCapa = new tSeriesCapas
            {
                aUnidades = unidades,
                aNumeroLote = numeroLote,
                aFechaCaducidad = fechaCaducidad,
                aFechaFabricacion = fechaFabricacion,
                aTipoCambio = tipoCambio,
                aSeries = series,
                aPedimento = pedimento,
                aAgencia = agencia,
                aFechaPedimento = fechaPedimento == "" ? "12/30/1899" : fechaPedimento
            };

            SdkSeriesCapas = sdkSerieCapa;
        }

        public void CrearSerieCapaSdk(double unidades, double tipoCambio, 
            string series, string pedimento, string agencia, string fechaPedimento)
        {
            var sdkSerieCapa = new tSeriesCapas
            {
                aUnidades = unidades,
                aTipoCambio = tipoCambio,
                aPedimento = pedimento,
                aAgencia = agencia,
                aFechaPedimento = fechaPedimento == "" ? DateTime.Today.ToString("MM/dd/yyyy") : fechaPedimento
            };

            if (!string.IsNullOrEmpty(series))
            {
                sdkSerieCapa.aSeries = series;
            }

            SdkSeriesCapas = sdkSerieCapa;
        }

        public void CrearSerieCapaSdk(double unidades, string series)
        {
            SdkSeriesCapas = new tSeriesCapas
            {
                aUnidades = unidades,
                aSeries = series
            }; ;
        }
    }
}
