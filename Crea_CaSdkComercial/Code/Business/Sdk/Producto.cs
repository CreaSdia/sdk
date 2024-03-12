using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdkCreaComercial.Business.Sdk;
using SdkCreaComercial.Code.Business;

namespace Crea_CaSdkComercial.Code.Business.Sdk
{
    class Producto
    {
        private SdkCreaComercial.Sdk.Sdk _sdk;

        public Producto(SdkCreaComercial.Sdk.Sdk sdk)
        {
            _sdk = sdk;
        }

        public int ProductoId { get; set; }
        public string CodigoProducto { get; set; } = "";
        public tProducto Sdkproducto { get; set; }
        public bool EsCrear { get; set; } = true;
        public bool ExisteProducto { get; set; }
        public string Error { get; set; } = "";
        public int FilaExcel { get; set; }
        public List<CampoAdicional> CamposAdicionales { get; set; }

        public void Crear()
        {
            Buscar();

            if (EsCrear && Error != "")
            {
                var productoId = 0;
                var producto = this.Sdkproducto;
                Error = _sdk.AltaProducto(ref productoId, ref producto);
                this.ProductoId = productoId;
            }else if (Error != "")
            {
                ExisteProducto = true;
            }
        }

        public void Editar()
        {
            if (ExisteProducto)
            {
                Buscar();
                Error = _sdk.LlamarEditarCampo("Producto", CamposAdicionales);
            }
        }

        public void Buscar()
        {
            if (CodigoProducto != "")
            {
                Error = _sdk.BuscarProducto(CodigoProducto);
            }
        }
    }
}
