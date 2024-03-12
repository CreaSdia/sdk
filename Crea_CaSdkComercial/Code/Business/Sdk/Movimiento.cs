using System.Collections.Generic;
using System.Linq;
using SdkCreaComercial.Code.Business;

namespace SdkCreaComercial.Business.Sdk
{
    public class Movimiento
    {
        public int MovimientoId { get; set; }
        public tMovimiento SdkMovimiento { get; set; }
        public string CodigoProducto { get; set; }
        public double Unidades { get; set; }
        public double Precio { get; set; }
        public string CodigoAlmacen { get; set; }
        public int FilaExcel { get; set; }
        public string Error { get; set; } = "";
        public bool AsignarLote { get; set; }
        public List<CampoAdicional> CamposAdicionales { get; set; }
        public List<SerieCapa> SerieCapas { get; set; } = new List<SerieCapa>();
        public SerieCapa SerieCapa { get; set; }

        public Movimiento (){}

        public Movimiento(bool asignarLote, List<CampoAdicional> camposAdicionales, int fila = 0)
        {
            var camposAdicionalesCopia = new List<CampoAdicional>();

            foreach (var campo in camposAdicionales.Where(e => e.TipoCampo == "mov"))
            {
                camposAdicionalesCopia.Add(new CampoAdicional(campo));
            }

            AsignarLote = asignarLote;
            FilaExcel = fila;
            CamposAdicionales = camposAdicionalesCopia;
        }

        public void CrearMovimintoSdk(string codigoProducto, double unidades, double precio, string codigoAlmacen,
            bool esFactura)
        {
            var sdkMoviminto = new tMovimiento
            {
                aConsecutivo = 1,
                aCodProdSer = codigoProducto,
                aUnidades = unidades,
                aPrecio = precio,
                aCodAlmacen = codigoAlmacen
            };

            if (esFactura)
            {
                sdkMoviminto.aPrecio = precio;
            }
            else
            {
                sdkMoviminto.aCosto = precio;
            }

            CodigoProducto = codigoProducto;
            SdkMovimiento = sdkMoviminto;
        }
    }

    public enum ETipoControlExistencia
    {
        Unidad = 1,
        Pedimento = 9,
        Serie = 4,
        SeriePedimento = 12
    }
}
