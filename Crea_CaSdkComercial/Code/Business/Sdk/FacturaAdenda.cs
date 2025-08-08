using Crea_CaSdkComercial.Code.Data;
using SdkCreaComercial.Code.Business;
using System.Collections.Generic;
using Crea_CaSdkComercial.Code.Contpaqi;

namespace Crea_CaSdkComercial.Code.Business.Sdk
{
    public class FacturaAdenda
    {
        private readonly string _empresa;

        public FacturaAdenda(string empresa)
        {
            _empresa = empresa;
        }

        public void GuardarAdenda(int movimientoId, List<CampoAdicional> datosAddenda)
        {
            using (var sdkContext = new ContpaqiDbContext(_empresa))
            {
                foreach (var campoAdicional in datosAddenda)
                {
                    var datos = campoAdicional.Nombre.Split('-');
                    if (datos.Length < 2)
                    {
                        continue; // Skip if the format is incorrect
                    }

                    if (!int.TryParse(datos[0], out var idAddenda))
                    {
                        continue;
                    }

                    if (!int.TryParse(datos[1], out var numCampo))
                    {
                        continue;
                    }

                    var adenda = new AdmDatosAddenda
                    {
                        IDADDENDA = idAddenda,
                        TIPOCAT = 5,
                        IDCAT = movimientoId,
                        NUMCAMPO = numCampo,
                        VALOR = campoAdicional.Valor
                    };

                    sdkContext.DatosAddenda.Add(adenda);
                }

                sdkContext.SaveChanges();
            }
        }
    }
}
