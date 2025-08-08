using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crea_CaSdkComercial.Code.Business;
using Crea_CaSdkComercial.Code.Business.Sdk;
using Crea_CaSdkComercial.Code.Data;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SdkCreaComercial.Business.Sdk;
using SdkCreaComercial.Code.Business;
using SdkCreaComercial.Sdk;

namespace Crea_CaSdkComercial.Code.Servicios
{
    class InformacionExcel
    {
        private StreamWriter _sw;
        private ExcelLayout _datos;
        private Sdk _sdk;
        private int _cuentaBancaria;
        public bool Error { get; set; }
        public string Empresa { get; set; }
        public string AsuntoCorreo { get; set; }
        public string PlantillaCorreo { get; set; }

        public InformacionExcel(StreamWriter sw, Sdk sdk)
        {
            Console.SetWindowSize(90, 25);
            Console.CursorVisible = false;
            Console.Title = "Generador Pagos";
            _sw = sw;
            LogError.Guardar(sw, "Cargando layout", TipoLog.Informacion);

            var fileInfo = new FileInfo(Settings.Default.PathLayoutRecurrente);
            var package = new ExcelPackage(fileInfo);

            LogError.Guardar(sw, "Procesando layout", TipoLog.Informacion);
            _datos = new ExcelLayout(package.Workbook.Worksheets[1]);

            if (_datos.CamposQueFaltan.Any())
            {
                var mensaje = "Las siguientes columnas no se encuentran en el template y son obligatorias: ";
                LogError.Guardar(_sw, mensaje + string.Join(",", _datos.CamposQueFaltan), TipoLog.Error);
                Error = true;
            }

            _sdk = sdk;
            Empresa = _datos.Empresa;
            _sdk.Empresa = _datos.Empresa;
            AsuntoCorreo = _datos.AsuntoCorreo;
            PlantillaCorreo = _datos.PlantillaCorreo;
        }

        public List<Documento> ObtenerDocumentos()
        {
            var documento = new Documento();
            var documentos = new List<Documento>();
            var fila = _datos.FilaCabecera + 1;
            int folio;
            var currentFolio = 0;
            string valorPrimerCelda;

            do
            {
                valorPrimerCelda = _datos.Worksheet.Cells[fila, 1].Text;

                if (valorPrimerCelda == "") continue;

                LogError.Guardar(_sw, "Obteniendo Información", TipoLog.Informacion, fila);
                _datos.ObtenerValorCamposAdicionales(fila);
                
                if (ValidarInformacion(fila, out folio))
                {
                    if (currentFolio != folio)
                    {
                        currentFolio = folio;
                        documento = ObtenerDocumento(folio, fila);
                        documentos.Add(documento);
                    }

                    AgregarMovimiento(documento, fila);
                }
                //Se guardan las filas con error como si fueran documentos, asi al momento de guardar todos los documentos
                //si alguna fila tuvo error no se guarda nada
                else
                {
                    documento = new Documento() { Error = "Error genérico" };
                    documentos.Add(documento);
                }

                fila++;
            } while (valorPrimerCelda != "");

            return documentos;
        }

        private Documento ObtenerDocumento(double folio, int fila)
        {
            var moneda = Convert.ToInt32(ObtenerValorCampoAdicional("cidmoneda", "1"));
            var tipoCambio = Convert.ToDouble(ObtenerValorCampoAdicional("ctipocambio", "1.00"));
            var concepto = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["ccodigoconcepto"]].Text;
            var fecha = _datos.ObtenerFechaContpaq(fila, _datos.CamposDocumento["cfecha"]);
            var codigoCliente = "";

            if (_datos.CamposDocumento.ContainsKey("ccodigocliente"))
            {
                codigoCliente = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["ccodigocliente"]].Text;
            }

            var documento = new Documento(_datos.TipoDocumento, concepto, codigoCliente, fecha, _datos.TimbrarDocumento, _datos.PasswordCsd,
                _datos.PlantillaDocumento, _datos.CamposAdicionales, fila);

            if (_datos.CamposDocumento.ContainsKey("atiporelacion"))
            {
                documento.TipoRelacion = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["atiporelacion"]].Text;
                documento.UuidsRelacionados = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["auuid"]].Text;
            }

            switch (_datos.TipoDocumento)
            {
                case ETipoDocumento.Factura:
                    CompletarFactura(documento, folio, fila);
                    break;
                case ETipoDocumento.Pago:
                    CompletarDocumentoPago(documento, fila);
                    break;
                case ETipoDocumento.Desconocido:
                    documento.Folio = folio;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            documento.CrearDocumntoSdk(moneda, tipoCambio, codigoCliente);

            return documento;
        }

        private void AgregarMovimiento(Documento documento, int fila)
        {
            switch (_datos.TipoDocumento)
            {
                case ETipoDocumento.Factura:
                    documento.Movimientos.Add(ObtenerMovimiento(fila));
                    break;
                case ETipoDocumento.Pago:
                    documento.MovimientosPago.Add(ObtenerMovimientoPago(fila));
                    break;
                case ETipoDocumento.Desconocido:
                    documento.Movimientos.Add(ObtenerMovimientoDesconocido(fila));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CompletarFactura(Documento documento, double folio, int fila)
        {
            if (!_datos.AsignarFolio) return;

            
            documento.Folio = folio;
            documento.Serie = _datos.Worksheet.Cells[fila, _datos.CamposFolio["cseriedocumento"]].Text;
            documento.UsoCfdi = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["cusocfdi"]].Text;
        }

        private void CompletarDocumentoPago(Documento documento, int fila)
        {
            var ws = _datos.Worksheet;
            var numeroCuenta = ObtenerValorCampoOpcional(fila, "nocuenta");

            documento.CuentaBancaria = numeroCuenta;
            documento.Importe = Convert.ToDouble(ws.Cells[fila, _datos.CamposDocumento["ctotalpago"]].Text);
            documento.UsoCfdi = ws.Cells[fila, _datos.CamposDocumento["cusocfdi"]].Text;
            documento.CamposAdicionales.Add(new CampoAdicional
            {
                Nombre = "CMETODOPAG",
                Valor = ws.Cells[fila, _datos.CamposDocumento["cmetodopag"]].Text
            });

            if (_cuentaBancaria > 0)
            {
                documento.CamposAdicionales.Add(new CampoAdicional
                {
                    Nombre = "CIDMONEDCA",
                    Valor = _cuentaBancaria.ToString()
                });
            }
        }

        private Movimiento ObtenerMovimiento(int fila, bool esFactura = true)
        {
            var ws = _datos.Worksheet;
            var codigoProducto = ws.Cells[fila, _datos.CamposDocumento["ccodigoproducto"]].Text;
            var campoPrecio = esFactura ? "cprecio" : "ccosto";
            var precio = Convert.ToDouble(ws.Cells[fila, _datos.CamposDocumento[campoPrecio]].Text);
            var codigoAlmacen = ObtenerValorCampoAdicional("cidalmacen", "1");
            var unidades = _datos.CamposDocumento.ContainsKey("cunidades")
                ? Convert.ToDouble(ws.Cells[fila, _datos.CamposDocumento["cunidades"]].Text)
                : 1;

            var movimiento = new Movimiento(_datos.AsignarLote, _datos.CamposAdicionales, fila);
            movimiento.Unidades = unidades;
            movimiento.CrearMovimintoSdk(codigoProducto, unidades, precio, codigoAlmacen, esFactura);

            if (!_datos.AsignarLote) return movimiento;

            var series = ObtenerValorCampoOpcional(fila, "aSeries");
            movimiento.SerieCapa = ObtenerSerieCapa(unidades, series, fila, true);

            return movimiento;
        }

        private SerieCapa ObtenerSerieCapa(double unidades, string series, int fila, bool esFactura)
        {
            var ws = _datos.Worksheet;

            var fechaPedimento = _datos.ObtenerFechaContpaq(fila, _datos.CamposOpcionales["afechapedimento"]);
            var tipoCambio = Convert.ToDouble(ObtenerValorCampoOpcional(fila, "atipocambio", "0"));
            var pedimento = ObtenerValorCampoOpcional(fila, "apedimento");
            var agencia = ObtenerValorCampoOpcional(fila, "aagencia");

            var serieCapa = new SerieCapa(fila);

            if (esFactura)
            {
                var fechaFabricacion = _datos.ObtenerFechaContpaq(fila, _datos.CamposDocumento["cfechafabricacion"]);
                var numeroLote = ws.Cells[fila, _datos.CamposDocumento["cnumerolote"]].Text;
                var fechaCaducidad = _datos.ObtenerFechaContpaq(fila, _datos.CamposDocumento["cfechacaducidad"]);

                serieCapa.CrearSerieCapaSdk(unidades, numeroLote, fechaCaducidad, fechaFabricacion, tipoCambio, series,
                    pedimento, agencia, fechaPedimento);
            }
            else
            {
                serieCapa.CrearSerieCapaSdk(unidades, tipoCambio, series, pedimento, agencia,
                    fechaPedimento);
            }

            return serieCapa;
        }

        private MovimientoPago ObtenerMovimientoPago(int fila)
        {
            var ws = _datos.Worksheet;

            var movimiento = new MovimientoPago
            {
                Concepto = ws.Cells[fila, _datos.CamposDocumento["cconceptofactura"]].Text,
                Serie = ws.Cells[fila, _datos.CamposDocumento["cseriefactura"]].Text,
                Folio = Convert.ToDouble(ws.Cells[fila, _datos.CamposDocumento["cfoliofactura"]].Text),
                Importe = Convert.ToDouble(ws.Cells[fila, _datos.CamposDocumento["abono"]].Text),
                FilaExcel = fila
            };

            return movimiento;
        }

        private Movimiento ObtenerMovimientoDesconocido(int fila)
        {
            var movimiento = ObtenerMovimiento(fila, false);
            _sdk.BuscarProducto(movimiento.CodigoProducto);

            var controlExistenciaString = _sdk.ObtenerCampoProducto("CCONTROLEXISTENCIA");
            if (controlExistenciaString == "") return new Movimiento();

            var controlExistencia = (ETipoControlExistencia)Convert.ToInt32(controlExistenciaString);

            var esPedimento = controlExistencia == ETipoControlExistencia.Pedimento
                              || controlExistencia == ETipoControlExistencia.SeriePedimento;
            var esSerie = controlExistencia == ETipoControlExistencia.Serie
                          || controlExistencia == ETipoControlExistencia.SeriePedimento;

            var series = "";
            if (esSerie)
            {
                series = esSerie ? ObtenerValorCampoOpcional(fila, "aseries") : "";

                if (!esPedimento)
                {
                    var serieCapa = new SerieCapa(fila);
                    serieCapa.CrearSerieCapaSdk(movimiento.Unidades, series);
                    movimiento.SerieCapa = serieCapa;
                }
            }

            if (!esPedimento) return movimiento;

            movimiento.SerieCapa = ObtenerSerieCapa(movimiento.Unidades, series, fila, false);
            movimiento.SerieCapa.ClaveSat = ObtenerValorCampoOpcional(fila, "cclavesat");

            return movimiento;
        }

        private bool ValidarInformacion(int fila, out int folio)
        {
            var errorCliente = "";
            var errorCuentaBancaria = ValidarCuentaBancaria(fila);
            var fechasConError = string.Join(",", _datos.ObtenerCamposFechasIncorrectos(fila));
            var folioString = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["cfolio"]].Text;
            int.TryParse(folioString, out folio);

            var mensaje = folio == 0 ? "La columna CFOLIO debe ser numérica. " : "";
            mensaje += errorCliente != "" ? errorCliente : "";
            mensaje += errorCuentaBancaria != "" ? errorCuentaBancaria : "";
            mensaje += fechasConError != ""
                ? "Las siguientes columnas tienen un formato de fecha incorrecto: " + fechasConError + ". "
                : "";

            if (mensaje == "") return true;

            LogError.Guardar(_sw, mensaje, TipoLog.Error, fila);

            return false;
        }

        private string ObtenerValorCampoOpcional(int fila, string nombreCampo, string valorDefault = "")
        {
            return _datos.CamposOpcionales.ContainsKey(nombreCampo)
                ? _datos.Worksheet.Cells[fila, _datos.CamposOpcionales[nombreCampo]].Text
                : valorDefault;
        }

        private string ObtenerValorCampoAdicional(string nombreCampo, string valorDefault = "")
        {
            var campoAdicional = _datos.CamposAdicionales.FirstOrDefault(e => e.Nombre == nombreCampo);
            return campoAdicional?.Valor ?? valorDefault;
        }

        private T ObtenerValorCampo<T>(List<CampoAdicional> campos, string nombreCampo, string valorDefault = "")
        {
            var campo = campos.FirstOrDefault(e => e.Nombre == nombreCampo);
            var valor = campo?.Valor ?? valorDefault;
            ;

            return (T)Convert.ChangeType(valor, typeof(T));
        }

        private string ValidarCliente(int fila, string usoCfdi)
        {
            if (_datos.TipoDocumento == ETipoDocumento.Desconocido) return "";

            var codigoCliente = _datos.Worksheet.Cells[fila, _datos.CamposDocumento["ccodigocliente"]].Text;

            if (codigoCliente == "" || usoCfdi == "") return "";

            return _sdk.BuscarCteProv(codigoCliente);
        }

        private string ValidarCuentaBancaria(int fila)
        {
            if (_datos.TipoDocumento != ETipoDocumento.Pago) return "";

            var cuentaCliente = ObtenerValorCampoOpcional(fila, "clientenocuenta");
            var sdkContext = new ContpaqiDbContext(_datos.Empresa);
            var cuentaBancaria = sdkContext.CuentasBancarias.FirstOrDefault(e => e.CNUMEROCUENTA == cuentaCliente);

            if (cuentaBancaria != null)
            {
                _cuentaBancaria = cuentaBancaria.CIDCUENTA;
            }

            return cuentaCliente != "" && cuentaBancaria == null ? "La cuenta del cliente no existe." : "";
        }
    }
}