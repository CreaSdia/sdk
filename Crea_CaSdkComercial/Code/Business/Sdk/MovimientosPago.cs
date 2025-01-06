using SdkCreaComercial.Business.Sdk;

namespace Crea_CaSdkComercial.Code.Business.Sdk
{
    public class MovimientoPago
    {
        public string Concepto{ get; set; }
        public string Serie { get; set; } = "";
        public double Folio { get; set; }
        public double Importe { get; set; }
        public string Error { get; set; } = "";
        public int FilaExcel { get; set; }
        public int Moneda { get; set; }
    }
}
