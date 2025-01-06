using System.Collections.Generic;
using System.Linq;
using Crea_CaSdkComercial.Code.Business.Sdk;
using SdkCreaComercial.Code.Business;


namespace SdkCreaComercial.Business.Sdk
{
    public class Documento
    {
        public int DocumentoId { get; set; }
        public tDocumento SdkDocumento { get; set; }
        public bool EsTimbrar { get; set; }
        public string PasswordCsd { get; set; } = "";
        public string PlantillaDocumento { get; set; } = "";
        public string Error { get; set; } = "";
        public int FilaExcel { get; set; }
        public string SistemaOrigen { get; set; } = "5";
        public string Concepto { get; set; } = "";
        public string Fecha { get; set; } = "";
        public string CodigoCliente { get; set; } = "";
        public int Moneda { get; set; }
        public double TipoCambio { get; set; }
        public string Serie { get; set; } = "";
        public double Folio { get; set; }
        public string UsoCfdi { get; set; } = "";
        public ETipoDocumento TipoDocumento { get; set; }
        public string CuentaBancaria { get; set; } = "";
        public double Importe { get; set; } = 0;
        public string UuidsRelacionados { get; set; } = "";
        public string TipoRelacion { get; set; } = "";

        public List<CampoAdicional> CamposAdicionales { get; set; }
        public List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
        public List<MovimientoPago> MovimientosPago { get; set; } = new List<MovimientoPago>();

        public Documento()
        {
        }

        public Documento(ETipoDocumento tipoDocumento, string concepto, string codigoCliente, string fecha, bool timbrar,
             string passwordCsd, string plantillaDocumento, List<CampoAdicional> camposAdicionales, int fila = 0)
        {
            var camposAdicionalesCopia = new List<CampoAdicional>();

            foreach (var campo in camposAdicionales.Where(e => e.TipoCampo == "doc"))
            {
                camposAdicionalesCopia.Add(new CampoAdicional(campo));
            }
            
            TipoDocumento = tipoDocumento;
            Concepto = concepto;
            CodigoCliente = codigoCliente;
            Fecha = fecha;
            EsTimbrar = timbrar;
            PasswordCsd = passwordCsd;
            PlantillaDocumento = plantillaDocumento;
            CamposAdicionales = camposAdicionalesCopia;
            FilaExcel = fila;
        }

        public void CrearDocumntoSdk(int moneda, double tipoCambio, string codigoCliente)
        {
            var sdkDocumento = new tDocumento
            {
                aCodConcepto = Concepto,
                aSistemaOrigen = 5,
                aFecha = Fecha,
                aNumMoneda = moneda,
                aTipoCambio = tipoCambio
            };

            if (!string.IsNullOrEmpty(codigoCliente))
            {
                sdkDocumento.aCodigoCteProv = codigoCliente;
            }

            if (TipoDocumento == ETipoDocumento.Pago)
            {
                sdkDocumento.aImporte = Importe;
            }

            

            SdkDocumento = sdkDocumento;
            Moneda = moneda;
        }
    }

    public enum ETipoDocumento
    {
        Factura = 1,
        Pago = 2,
        Desconocido = 3
    }
}