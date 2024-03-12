using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OfficeOpenXml;
using SdkCreaComercial.Business.Sdk;
using SdkCreaComercial.Code.Business;

namespace Crea_CaSdkComercial.Code.Business
{
    class ExcelLayout
    {
        public string Empresa { get; set; }
        public int FilaCabecera { get; set; }
        public string PasswordCsd { get; set; }
        public string PlantillaDocumento { get; set; }
        public string AsuntoCorreo { get; set; }
        public string PlantillaCorreo { get; set; }
        public bool TimbrarDocumento { get; set; }
        public bool AsignarFolio { get; set; }
        public bool AsignarLote { get; set; }
        public bool EsFactura { get; set; }
        public bool EsDocumentoPago { get; set; }
        public int UltimaColumna { get; set; }
        public ETipoDocumento TipoDocumento { get; set; }
        public Dictionary<string, int> CamposEnLayout { get; set; } = new Dictionary<string, int>();
        public List<string> CamposQueFaltan { get; } = new List<string>();
        public List<CampoAdicional> CamposAdicionales { get; set; } = new List<CampoAdicional>();
        public ExcelWorksheet Worksheet { get; set; }

        public Dictionary<string, int> CamposDocumento { get; set; }

        //Estas propiedades indican el nombre del campo y el número de columna donde se encuentran,
        //Son obligatorios, excepto la propiedad CamposOpcionales
        public Dictionary<string, int> CamposFactura = new Dictionary<string, int>
        {
            {"cfecha", 0},
            {"cfolio", 0},
            {"ccodigoconcepto", 0},
            {"ccodigoproducto", 0},
            {"cunidades", 0},
            {"cprecio", 0},
            {"ccodigocliente", 0},
            {"cusocfdi", 0} // Pedro
        };

        public Dictionary<string, int> CamposPago = new Dictionary<string, int>
        {
            {"ccodigoconcepto", 0},
            {"cfolio", 0},
            {"cfecha", 0},
            {"ccodigocliente", 0},
            {"cmetodopag", 0},
            {"cusocfdi", 0},
            {"ctotalpago", 0},
            {"cconceptofactura", 0},
            {"cseriefactura", 0},
            {"cfoliofactura", 0},
            {"abono", 0}
        };

        public Dictionary<string, int> CamposDesconocido = new Dictionary<string, int>
        {
            {"ccodigoconcepto", 0},
            {"cfolio", 0},
            {"cfecha", 0},
            {"ccodigoproducto", 0},
            {"ccosto", 0},
            {"cunidades", 0}
        };

        public Dictionary<string, int> CamposLote = new Dictionary<string, int>
        {
            {"cnumerolote", 0},
            {"cfechacaducidad", 0},
            {"cfechafabricacion", 0}
        };

        public Dictionary<string, int> CamposFolio = new Dictionary<string, int>
        {
            {"cseriedocumento", 0}
        };

        public Dictionary<string, int> CamposOpcionales = new Dictionary<string, int>
        {
            {"atipocambio", 0},
            {"aseries", 0},
            {"apedimento", 0},
            {"aagencia", 0},
            {"nocuenta", 0},
            {"clientenocuenta", 0},
            {"afechapedimento", 0},
            {"cclavesat", 0}
        };  

        public List<string> CamposFecha = new List<string>
        {
            "cfecha", "cfechavencimiento", "cfechacaducidad", "cfechafabricacion", "aFechapedimento"
        };

        public ExcelLayout(ExcelWorksheet ws)
        {
            Worksheet = ws;
            UltimaColumna = Worksheet.Dimension.End.Column;
            Empresa = Worksheet.Cells["B1"].Text;
            FilaCabecera = Convert.ToInt16(Worksheet.Cells["B3"].Text);
            PasswordCsd = Worksheet.Cells["E1"].Text;
            PlantillaDocumento = Worksheet.Cells["E3"].Text;
            AsuntoCorreo = Worksheet.Cells["H2"].Text;
            PlantillaCorreo = Worksheet.Cells["H3"].Text;
            TimbrarDocumento = Worksheet.Cells["L2"].Text == "1";
            AsignarFolio = Worksheet.Cells["L1"].Text == "1";
            AsignarLote = Worksheet.Cells["L3"].Text == "1";
            EsFactura = true;
            EsDocumentoPago = false;
            TipoDocumento = (ETipoDocumento)Convert.ToInt16(ws.Cells["B2"].Value);

            var tipoDocumentoCampo = new Dictionary<ETipoDocumento, Dictionary<string, int>>
            {
                { ETipoDocumento.Factura, CamposFactura },
                { ETipoDocumento.Pago, CamposPago},
                { ETipoDocumento.Desconocido, CamposDesconocido}
            };
            CamposDocumento = tipoDocumentoCampo[TipoDocumento];

            EncontrarPosicionCampos();
            ObtenerCamposQueFaltan(CamposDocumento);
            ObtenerCamposQueFaltan(CamposFolio, AsignarFolio);
            ObtenerCamposQueFaltan(CamposLote, AsignarLote);
        }

        private void EncontrarPosicionCampos()
        {
            for (var i = 1; i < UltimaColumna; i++)
            {
                var nombreColumna = Worksheet.Cells[FilaCabecera, i].Text.ToLower().Trim();
                var columnaEncontrada = false;

                //Si se encuentra este nombre en la columna se pediran obligatorios este valor y el de auuid
                if (nombreColumna == "atiporelacion")
                {
                    CamposDocumento.Add("atiporelacion", 0);
                    CamposDocumento.Add("auuid", 0);
                }

                //Busca las llaves(nombres de las columnas) de todos los diccionarios
                //y les asigna el número de columna en que se encuentra
                EncontrarPosicionCampo(CamposDocumento, nombreColumna, i, true, ref columnaEncontrada);
                EncontrarPosicionCampo(CamposOpcionales, nombreColumna, i, true, ref columnaEncontrada);
                EncontrarPosicionCampo(CamposFolio, nombreColumna, i, AsignarFolio, ref columnaEncontrada);
                EncontrarPosicionCampo(CamposLote, nombreColumna, i, AsignarLote, ref columnaEncontrada);

                //Si no encuentra el nombre de la columna en los diccionarios, lo agrega como campo adicional
                if (!columnaEncontrada)
                {
                    AgregarCampoAdicional(nombreColumna, i);
                }
            }
        }

        private void EncontrarPosicionCampo(Dictionary<string, int> campos, string tituloCelda, int columna,
            bool buscar, ref bool tituloEncontrado)
        {
            if (!buscar || !campos.ContainsKey(tituloCelda)) return;

            campos[tituloCelda] = columna;
            tituloEncontrado = true;
            CamposEnLayout[tituloCelda] = columna;
        }

        private void AgregarCampoAdicional(string tituloCelda, int columna)
        {
            var campoAdicional = new CampoAdicional
            {
                Nombre = tituloCelda,
                Columna = columna,
                TipoCampo = Worksheet.Cells[FilaCabecera - 1, columna].Text.ToLower()
            };

            CamposAdicionales.Add(campoAdicional);
            CamposEnLayout[tituloCelda] = columna;
        }

        private void ObtenerCamposQueFaltan(Dictionary<string, int> campos, bool sonNecesarios = true)
        {
            if (!sonNecesarios) return;

            foreach (var campo in campos.Where(e => e.Value == 0))
            {
                CamposQueFaltan.Add(campo.Key);
            }
        }

        public List<string> ObtenerCamposFechasIncorrectos(int fila)
        {
            return (from campo in CamposFecha
                    where CamposEnLayout.ContainsKey(campo)
                    let valorCelda = Worksheet.Cells[fila, CamposEnLayout[campo]].Text
                    let fecha = ObtenerFechaContpaq(valorCelda)
                    where fecha == ""
                    select campo.ToUpper()
                ).ToList();
        }

        /// <summary>
        /// Obtiene el valor de la fila de todos los campos que no estan mapeados en los diccionarios de campo
        /// </summary>
        /// <param name="fila"></param>
        public void ObtenerValorCamposAdicionales(int fila)
        {
            foreach (var campo in CamposAdicionales)
            {
                var valorCelda = Worksheet.Cells[fila, campo.Columna].Text;
                campo.Valor = CamposFecha.Contains(campo.Nombre) ? ObtenerFechaContpaq(valorCelda) : valorCelda;
            }
        }

        public string ObtenerFechaContpaq(int fila, int columna)
        {
            var fechaString = Worksheet.Cells[fila, columna].Text;
            var fechaValida = DateTime.TryParseExact(fechaString, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var fecha);

            return fechaValida ? fecha.ToString("MM/dd/yyyy") : "";
        }

        public string ObtenerFechaContpaq(string fechaString)
        {
            var fechaValida = DateTime.TryParseExact(fechaString, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var fecha);

            return fechaValida ? fecha.ToString("MM/dd/yyyy") : "";
        }
    }
}