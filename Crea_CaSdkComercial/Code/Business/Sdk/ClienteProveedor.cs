using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdkCreaComercial.Business.Sdk;
using SdkCreaComercial.Code.Business;

namespace Crea_CaSdkComercial.Code.Business.Sdk
{
    class ClienteProveedor
    {
        private SdkCreaComercial.Sdk.Sdk _sdk;

        public ClienteProveedor(SdkCreaComercial.Sdk.Sdk sdk)
        {
            _sdk = sdk;
        }

        public int ClienteId { get; set; }
        public string CodigoCliente { get; set; } = "";
        public tCteProv SdkClienteProveedor { get; set; }
        public bool EsCrear { get; set; } = true;
        public bool ExisteCliente { get; set; }
        public string Error { get; set; } = "";
        public int FilaExcel { get; set; }
        public List<CampoAdicional> CamposAdicionales { get; set; }

        public void Crear()
        {
            Buscar();

            if (EsCrear && Error != "")
            {
                var clienteId = 0;
                var clienteProveedor = this.SdkClienteProveedor;
                Error = _sdk.AltaCteProv(ref clienteId, ref clienteProveedor);
                this.ClienteId = clienteId;
            }
            else
            {
                ExisteCliente = true;
            }
        }

        public void Editar()
        {
            if (ExisteCliente)
            {
                Buscar();
                Error = _sdk.LlamarEditarCampo("CteProv", CamposAdicionales);
            }
        }

        public void Buscar()
        {
            if (CodigoCliente != "")
            {
                Error = _sdk.BuscarCteProv(CodigoCliente);
            }
        }
    }
}
