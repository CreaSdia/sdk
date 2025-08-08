using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Crea_CaSdkComercial;
using Crea_CaSdkComercial.Code.Business.Sdk;
using OfficeOpenXml;
using SdkCreaComercial.Business.Sdk;
using SdkCreaComercial.Code.Business;

namespace SdkCreaComercial.Sdk
{
    public class Sdk
    {
        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string pPtrDirActual);

        [DllImport("MGWServicios.dll")]
        public static extern void fInicioSesionSDK(string usuario, string password);

        [DllImport("MGWServicios.DLL")]
        public static extern int fSetNombrePAQ(String aNombrePAQ);

        [DllImport("MGWServicios.dll")]
        public static extern int fAbreEmpresa(string Directorio);

        [DllImport("MGWServicios.dll")]
        public static extern void fTerminaSDK();

        [DllImport("MGWServicios.dll")]
        public static extern void fCierraEmpresa();

        [DllImport("MGWServicios.dll")]
        public static extern void fError(int NumeroError, StringBuilder Mensaje, int Longitud);

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaDocumento(ref int IdDocumento, ref tDocumento documento);

        [DllImport("MGWServicios.DLL")]
        public static extern int fAltaDocumentoCargoAbono(ref tDocumento documento);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscarIdDocumento(int idDocumento);

        [DllImport("MGWServicios.dll")]
        public static extern int fBorraDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern int fEditarDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaDocumento();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoDocumento(string campo, string valor);

        [DllImport("MGWServicios.dll")]
        public static extern int fLeeDatoDocumento(string campo, StringBuilder valor, int longitud);

        [DllImport("MGWServicios.DLL")]
        public static extern Int32 fSaldarDocumento(ref RegLlaveDoc astDocAPagar, ref RegLlaveDoc astDocPago,
            double importe, int moneda, string aFecha);

        public int EditaDocumento<T>()
        {
            return fEditarDocumento();
        }

        public int GuardaDocumento<T>()
        {
            return fGuardaDocumento();
        }

        public int SetDatoDocumento<T>(string campo, string valor)
        {
            return fSetDatoDocumento(campo, valor);
        }

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaMovimiento(int IdDocumento, ref int IdMovimiento, ref tMovimiento movimiento);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscarIdMovimiento(int idMovimiento);

        [DllImport("MGWServicios.dll")]
        public static extern int fEditarMovimiento();

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaMovimiento();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoMovimiento(string campo, string valor);

        public int EditaMovimiento<T>()
        {
            return fEditarMovimiento();
        }

        public int GuardaMovimiento<T>()
        {
            return fGuardaMovimiento();
        }

        public int SetDatoMovimiento<T>(string campo, string valor)
        {
            return fSetDatoMovimiento(campo, valor);
        }

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaMovimientoSeriesCapas(int IdMovimiento, ref tSeriesCapas seriesCapas);

        [DllImport("MGWServicios.dll")]
        public static extern int fCalculaMovtoSerieCapa(int IdMovimiento);

        [DllImport("MGWServicios.dll")]
        public static extern int fEmitirDocumento([MarshalAs(UnmanagedType.LPStr)] string concepto,
            [MarshalAs(UnmanagedType.LPStr)] string serie, double folio,
            [MarshalAs(UnmanagedType.LPStr)] string password,
            [MarshalAs(UnmanagedType.LPStr)] string archivoAdicional);

        [DllImport("MGWServicios.dll")]
        public static extern int fDocumentoUUID([MarshalAs(UnmanagedType.LPStr)] string concepto,
            [MarshalAs(UnmanagedType.LPStr)] string serie, double folio, StringBuilder uuId);

        [DllImport("MGWServicios.dll")]
        public static extern int fEntregEnDiscoXML([MarshalAs(UnmanagedType.LPStr)] string concepto,
            [MarshalAs(UnmanagedType.LPStr)] string serie, double folio, int formato, string pathPlantilla);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaIdCteProv(int idCteProv);

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaCteProv(ref int clienteProveedorId, ref tCteProv clienteProveedor);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaCteProv(string codigoCliente);

        [DllImport("MGWServicios.dll")]
        public static extern int fLeeDatoCteProv(string campo, StringBuilder valor, int longitud);

        [DllImport("MGWServicios.dll")]
        public static extern int fInsertaCteProv();

        [DllImport("MGWServicios.dll")]
        public static extern int fEditaCteProv();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoCteProv(string campo, string valor);

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaCteProv();

        [DllImport("MGWServicios.dll")]
        public static extern int fBorraCteProv();

        public int EditaCteProv<T>()
        {
            return fEditaCteProv();
        }

        public int GuardaCteProv<T>()
        {
            return fGuardaCteProv();
        }

        public int SetDatoCteProv<T>(string campo, string valor)
        {
            return fSetDatoCteProv(campo, valor);
        }

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaDireccionCteProv(string codigoCteProv, byte tipoDireccion);

        [DllImport("MGWServicios.dll")]
        public static extern int fInsertaDireccion();

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaDireccion(ref int direccionId, ref tDireccion direccion);

        [DllImport("MGWServicios.dll")]
        public static extern int fEditaDireccion();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoDireccion(string campo, string valor);

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaDireccion();

        public int EditaDireccion<T>()
        {
            return fEditaDireccion();
        }

        public int GuardaDireccion<T>()
        {
            return fGuardaDireccion();
        }

        public int SetDatoDireccion<T>(string campo, string valor)
        {
            return fSetDatoDireccion(campo, valor);
        }

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaProducto(string codigoProducto);

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaProducto(ref int productoId, ref tProducto producto);

        [DllImport("MGWServicios.dll")]
        public static extern int fInsertaProducto();

        [DllImport("MGWServicios.dll")]
        public static extern int fEditaProducto();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoProducto(string campo, string valor);

        [DllImport("MGWServicios.dll")]
        public static extern int fLeeDatoProducto(string campo, StringBuilder valor, int longitud);

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaProducto();

        public int EditaProducto<T>()
        {
            return fEditaProducto();
        }

        public int GuardaProducto<T>()
        {
            return fGuardaProducto();
        }

        public int SetDatoProducto<T>(string campo, string valor)
        {
            return fSetDatoProducto(campo, valor);
        }

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaUnidad(ref int unidadId, ref tUnidad unidad);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscaUnidad(string nombreUnidad);

        [DllImport("MGWServicios.dll")]
        public static extern int fBuscarIdUnidad(int idUnidad);

        [DllImport("MGWServicios.dll")]
        public static extern int fEditaUnidad();

        [DllImport("MGWServicios.dll")]
        public static extern int fGuardaUnidad();

        [DllImport("MGWServicios.dll")]
        public static extern int fSetDatoUnidad(string campo, string valor);

        [DllImport("MGWServicios.dll")]
        public static extern int fCuentaBancariaEmpresaDoctos(string cuenta);

        [DllImport("MGWServicios.dll")]
        public static extern int fAgregarRelacionCFDI2(string aCodConcepto, string aSerie, string aFolio,
            string aTipoRelacion, string aUUID);

        public int EditaUnidad<T>()
        {
            return fEditaUnidad();
        }

        public int GuardaUnidad<T>()
        {
            return fGuardaUnidad();
        }

        public int SetDatoUnidad<T>(string campo, string valor)
        {
            return fSetDatoUnidad(campo, valor);
        }

        private string _error = "";

        public string Empresa { get; set; }

        public Sdk()
        {
        }

        public Sdk(string rutaSistema, string nombreSistema, out string error)
        {
            var errorId = SetCurrentDirectory(rutaSistema);

            if (errorId == 0 || errorId == 1)
            {
                fInicioSesionSDK(Settings.Default.UsuarioContpaq, Settings.Default.PasswordContpaq);

                errorId = fSetNombrePAQ(nombreSistema);
            }

            error = GetError(errorId);
        }

        public string IniciarSesion(string rutaSistema, string nombreSistema)
        {
            var errorId = SetCurrentDirectory(rutaSistema);

            if (errorId == 0 || errorId == 1)
            {
                fInicioSesionSDK(Settings.Default.UsuarioContpaq, Settings.Default.PasswordContpaq);

                errorId = fSetNombrePAQ(nombreSistema);
            }

            return GetError(errorId);
        }

        public string AbrirEmpresa(string rutaEmpresa)
        {
            var errorId = fAbreEmpresa(rutaEmpresa);
            var result = GetError(errorId);
            return result;
        }

        public string BuscarDocumento(int documentoId)
        {
            var errorId = fBuscarIdDocumento(documentoId);
            var result = GetError(errorId);
            return result;
        }
        
        public string ObtenerDatoDocumento(string campo, int fila)
        {
            if (_error != "") return _error;

            var valor = new StringBuilder(512);
            var errorId = fLeeDatoDocumento(campo, valor, 350);
            _error = GetError(errorId, "fLeeDatoDocumento", fila);

            return valor.ToString();
        }
        
        public string ObtenerDatoDocumento(string campo, out int errorId)
        {
            errorId = 0;

            if (_error != "") return _error;

            var valor = new StringBuilder(512);
            errorId = fLeeDatoDocumento(campo, valor, 350);
            return valor.ToString();
        }

        public string CrearDocumentoCargoAbono(Documento documento, tDocumento sdkDocumento)
        {
            var errorId = fAltaDocumentoCargoAbono(ref sdkDocumento);
            var result = GetError(errorId, "fAltaDocumentoCargoAbono");

            if (result == "")
            {
                documento.Folio = Convert.ToDouble(ObtenerDatoDocumento("CFOLIO", out errorId));
                documento.Serie = ObtenerDatoDocumento("CSERIEDOCUMENTO", out errorId);
                documento.DocumentoId = Convert.ToInt32(ObtenerDatoDocumento("CIDDOCUMENTO", out errorId));
            }

            return result;
        }

        public string AltaDocumentoCargoAbono(Documento documento)
        {
            var sdkDocumento = new tDocumento
            {
                aCodConcepto = documento.Concepto,
                aSistemaOrigen = 5,
                aCodigoCteProv = documento.CodigoCliente,
                aFecha = documento.Fecha,
                aNumMoneda = documento.Moneda,
                aTipoCambio = documento.TipoCambio,
                aImporte = documento.Importe
            };

            if (documento.CuentaBancaria != "")
            {
                _error = SetCuentaBancaria(documento.CuentaBancaria);
            }

            if (_error != "") return _error;
            
            var errorId = fAltaDocumentoCargoAbono(ref sdkDocumento);
            _error = GetError(errorId, "fAltaDocumentoCargoAbono");

            ObtenerDatosCargoAbono(documento);

            return _error;
        }

        private void ObtenerDatosCargoAbono(Documento documento)
        {
            if(_error != "") return;

            documento.Folio = Convert.ToDouble(ObtenerDatoDocumento("CFOLIO", out _));
            documento.Serie = ObtenerDatoDocumento("CSERIEDOCUMENTO", out _);
            documento.DocumentoId = Convert.ToInt32(ObtenerDatoDocumento("CIDDOCUMENTO", out _));
        }

        private void AltaDocumentoSdk(Documento documento)
        {
            if (documento.CuentaBancaria != "")
            {
                _error = SetCuentaBancaria(documento.CuentaBancaria);
            }

            var errorId = 0;
            var metodoSdk = "";
            var sdkDocumento = documento.SdkDocumento;

            switch (documento.TipoDocumento)
            {
                case ETipoDocumento.Factura:
                    metodoSdk = "fAltaDocumento";
                    var documentoId = 0;
                    errorId = fAltaDocumento(ref documentoId, ref sdkDocumento);
                    documento.DocumentoId = errorId == 0? documentoId: 0;
                    break;
                case ETipoDocumento.Pago:
                    metodoSdk = "fAltaDocumentoCargoAbono";
                    errorId = fAltaDocumentoCargoAbono(ref sdkDocumento);

                    if (errorId == 0)
                    {
                        documento.DocumentoId = Convert.ToInt32(ObtenerDatoDocumento("CIDDOCUMENTO", out _));
                    }

                    break;
                case ETipoDocumento.Desconocido:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _error = GetError(errorId, metodoSdk, documento.FilaExcel);
        }

        private void AltaMovimientoSdk(Documento documento)
        {
            switch (documento.TipoDocumento)
            {
                case ETipoDocumento.Factura:
                    foreach (var movimiento in documento.Movimientos)
                    {
                        AltaMovimiento(documento.DocumentoId, movimiento);
                    }
                    break;
                case ETipoDocumento.Pago:
                    foreach (var movimiento in documento.MovimientosPago)
                    {
                        SaldarDocumento(movimiento, documento);
                    }
                    break;
                case ETipoDocumento.Desconocido:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string AltaDocumento(Documento documento)
        {
            var error = ActualizarUsoCfdi(documento);

            if (error != "") return error;

            AltaDocumentoSdk(documento);

            if (_error != "") return _error;
            
            BuscarIdDocumento(documento.DocumentoId);
            LlamarEditarCampo("Documento", documento.CamposAdicionales);

            ObtenerFolioSerie(documento);
            AltaMovimientoSdk(documento);

            RelacionarCfdi2(documento);
            BorrarDocumento(documento.DocumentoId);

            if (!documento.EsTimbrar) return _error;
            TimbrarDocumento(documento);

            BorrarDocumento(documento.DocumentoId);



            error = GenerarArchivos(documento);

            return error;
        }

        private void BorrarDocumento(int documentoId)
        {
            if (_error != "" && documentoId > 0)
            {
                BuscarIdDocumento(documentoId);
                BorraDocumento();
            }
        }

        public void ObtenerFolioSerie(Documento documento)
        {
            if(_error != "") return;

            BuscarIdDocumento(documento.DocumentoId);
            var folio = documento.Folio;
            var serie = documento.Serie;
            var fila = documento.FilaExcel;

            documento.Folio = folio == 0 ? Convert.ToDouble(ObtenerDatoDocumento("CFOLIO", fila)) : folio;
            documento.Serie = serie == "" ? ObtenerDatoDocumento("CSERIEDOCUMENTO", fila) : serie;
        }

        public void RelacionarCfdi2(Documento documento)
        {
            if(documento.TipoRelacion == "" || documento.UuidsRelacionados == "") return;

            var folio = documento.Folio.ToString();
            var errorId = fAgregarRelacionCFDI2(documento.Concepto, documento.Serie, folio, documento.TipoRelacion,
                documento.UuidsRelacionados);
            _error = GetError(errorId, "fAgregarRelacionCFDI2", documento.FilaExcel);
        }

        public string AltaMovimiento(int documentoId, Movimiento movimiento)
        {
            if (_error != "") return _error;

            var sdkMovimiento = movimiento.SdkMovimiento;

            var movimientoId = 0;
            var errorId = fAltaMovimiento(documentoId, ref movimientoId, ref sdkMovimiento);
            _error = GetError(errorId, "fAltaMovimiento", movimiento.FilaExcel);

            if (_error != "") return _error;

            movimiento.MovimientoId = movimientoId;
            LlamarEditarCampo("Movimiento", movimiento.CamposAdicionales);

            if (movimiento.SerieCapa != null)
            {
                AltaMovimientoSerieCapa(movimientoId, movimiento.SerieCapa);

                if (_error == "")
                {
                    errorId = fCalculaMovtoSerieCapa(movimientoId);
                    _error = GetError(errorId, "fCalculaMovtoSerieCapa", movimiento.FilaExcel);
                }
            }

            var facturaAdenda = new FacturaAdenda(Empresa);
            facturaAdenda.GuardarAdenda(movimientoId, movimiento.DatosAdenda);

            if (!movimiento.AsignarLote) return "";

            foreach (var serieCapa in movimiento.SerieCapas)
            {
                AltaMovimientoSerieCapa(movimientoId, serieCapa);
            }

            return _error;
        }

        public string AltaMovimientoSerieCapa(int movimientoId, SerieCapa serieCapa)
        {
            if (_error != "") return _error;

            var sdkSerieCapa = serieCapa.SdkSeriesCapas;  

            var errorId = fAltaMovimientoSeriesCapas(movimientoId, ref sdkSerieCapa);
            _error = GetError(errorId, "fAltaMovimientoSeriesCapas", serieCapa.FilaExcel);

            if (!string.IsNullOrEmpty(serieCapa.ClaveSat) && _error == "")
            {
                var campoEditar = string.IsNullOrEmpty(serieCapa.SdkSeriesCapas.aSeries) ?
                    "cClaveSat¬Pedimento" : "cClaveSat¬Serie";
                errorId = fSetDatoMovimiento(campoEditar, serieCapa.ClaveSat);
                _error = GetError(errorId, "fSetDatoMovimiento", serieCapa.FilaExcel);
            }

            return _error;
        }

        public string ActualizarUsoCfdi(Documento documento)
        {
            if (string.IsNullOrEmpty(documento.CodigoCliente) || string.IsNullOrEmpty(documento.UsoCfdi)) return "";

            var error = BuscarCteProv(documento.CodigoCliente);

            if (error != "") return error;

            error = LlamarEditarCampo("CteProv", "CUSOCFDI", documento.UsoCfdi);

            return error;
        }

        public string AltaDocumento(ref int documentoId, tDocumento documento)
        {
            var errorId = fAltaDocumento(ref documentoId, ref documento);
            var result = GetError(errorId, "fAltaDocumento");
            return result;
        }

        public string TimbrarDocumento(Documento documento)
        {
            if (_error != "") return _error;

            var errorId = fEmitirDocumento(documento.Concepto, documento.Serie, documento.Folio,
                documento.PasswordCsd, "");

            _error = GetError(errorId, "fEmitirDocumento", documento.FilaExcel);

            return _error;
        }

        public string GenerarArchivos(Documento documento)
        {
            if (_error != "") return _error;

            var error = EntregaEnDisco(documento, 0);
            error += EntregaEnDisco(documento, 1);

            return error;
        }

        public string BuscarIdDocumento(int documentoId)
        {
            var errorId = fBuscarIdDocumento(documentoId);
            var result = GetError(errorId, "fBuscarIdDocumento");
            return result;
        }

        public void SaldarDocumento(MovimientoPago movimiento, Documento documento)
        {
            if(_error != "") return;

            var factura = new RegLlaveDoc
            {
                aCodConcepto = movimiento.Concepto,
                aSerie = movimiento.Serie,
                folio = movimiento.Folio
            };

            var documentoCargo = new RegLlaveDoc
            {
                aCodConcepto = documento.Concepto,
                aSerie = documento.Serie,
                folio = documento.Folio
            };

            var errorId = fSaldarDocumento(ref factura, ref documentoCargo, movimiento.Importe, documento.Moneda, documento.Fecha);
            _error = GetError(errorId, "fSaldarDocumento");
        }

        public string EmitirDocumento(tDocumento documento, string password, string archivoAdicional)
        {
            var errorId = fEmitirDocumento(documento.aCodConcepto, documento.aSerie, documento.aFolio, password,
                archivoAdicional);
            var result = GetError(errorId, "EmitirDocumento");
            return result;
        }

        public string EntregaEnDisco(Documento documento, int formato)
        {
            var errorId = fEntregEnDiscoXML(documento.Concepto, documento.Serie, documento.Folio, formato,
                documento.PlantillaDocumento);
            var result = GetError(errorId, "fEntregEnDiscoXML", documento.FilaExcel);
            return result;
        }

        public string EntregaEnDisco(tDocumento documento, int formato, string pathPlantilla)
        {
            var errorId = fEntregEnDiscoXML(documento.aCodConcepto, documento.aSerie, documento.aFolio, formato,
                pathPlantilla);
            var result = GetError(errorId, "fEntregEnDiscoXML");
            return result;
        }

        public string DocumentoUuid(ref tDocumento documento)
        {
            var uuid = new StringBuilder(512);
            var errorId = fDocumentoUUID(documento.aCodConcepto, documento.aSerie, documento.aFolio, uuid);
            documento.uuid = uuid.ToString();

            var result = GetError(errorId, "fDocumentoUUID");
            return result;
        }

        public string LlamarEditarCampo(string tabla, string campo, string valorCampo,
            Dictionary<string, string> campos = null)
        {
            string result;
            try
            {
                object[] args = {campo, valorCampo};
                var metodoSdk = "fEdita" + tabla;
                var metodoEditar = typeof(Sdk).GetMethod("Edita" + tabla);
                var metodoSetDato = typeof(Sdk).GetMethod("SetDato" + tabla);
                var metodoGuardar = typeof(Sdk).GetMethod("Guarda" + tabla);

                if (metodoEditar == null || metodoSetDato == null || metodoGuardar == null)
                    return "Algún método de edición no esta declarado";

                var genericMethod = metodoEditar.MakeGenericMethod(typeof(int));
                var response = genericMethod.Invoke(this, null);
                var errorId = (int) response;
                var camposConError = "";

                if (errorId == 0)
                {
                    metodoSdk = "fSetDato" + tabla;
                    genericMethod = metodoSetDato.MakeGenericMethod(typeof(int));

                    if (campos == null)
                    {
                        response = genericMethod.Invoke(this, args);
                        errorId = (int) response;
                        var error = GetError(errorId, "fAltaMovimiento");
                    }
                    else
                    {
                        foreach (var campoDiccionario in campos)
                        {
                            object[] argsDiccionario = {campoDiccionario.Key, campoDiccionario.Value};
                            response = genericMethod.Invoke(this, argsDiccionario);
                            errorId = (int) response;

                            if (errorId != 0)
                            {
                                camposConError += campoDiccionario.Key + "Error: " + GetError(errorId) + ", ";
                            }
                        }

                        metodoSdk = metodoSdk + ", campos con error: " + camposConError;
                    }
                }

                if (camposConError == "")
                {
                    metodoSdk = "fGuarda" + tabla;
                    genericMethod = metodoGuardar.MakeGenericMethod(typeof(int));
                    response = genericMethod.Invoke(this, null);
                    errorId = (int) response;
                }
                else
                {
                    camposConError = "LlamarEditarCampo: " + camposConError;
                }

                result = camposConError;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public string LlamarEditarCampo(string tabla, List<CampoAdicional> campos)
        {
            if (_error != "") return _error;

            try
            {
                var metodoEditar = typeof(Sdk).GetMethod("Edita" + tabla);
                var metodoSetDato = typeof(Sdk).GetMethod("SetDato" + tabla);
                var metodoGuardar = typeof(Sdk).GetMethod("Guarda" + tabla);

                if (metodoEditar == null || metodoSetDato == null || metodoGuardar == null)
                    return "Algún método de edición no esta declarado";

                var genericMethod = metodoEditar.MakeGenericMethod(typeof(int));
                var response = genericMethod.Invoke(this, null);
                var errorId = (int) response;
                var camposConError = "";

                if (errorId == 0)
                {
                    genericMethod = metodoSetDato.MakeGenericMethod(typeof(int));

                    foreach (var campo in campos)
                    {
                        object[] argsDiccionario = {campo.Nombre, campo.Valor};
                        response = genericMethod.Invoke(this, argsDiccionario);
                        errorId = (int) response;

                        if (errorId != 0)
                        {
                            camposConError += $"Error en campo {campo.Nombre}: {GetError(errorId)}";
                        }
                    }
                }

                if (camposConError == "")
                {
                    genericMethod = metodoGuardar.MakeGenericMethod(typeof(int));
                    response = genericMethod.Invoke(this, null);
                }
                else
                {
                    camposConError = "LlamarEditarCampo: " + camposConError;
                }

                _error = camposConError;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
            }

            return _error;
        }

        public string BorraDocumento()
        {
            var errorId = fBorraDocumento();
            var result = GetError(errorId, "fBorraDocumento");
            return result;
        }

        public string AltaMovimiento(int documentoId, ref int movimientoId, tMovimiento movimiento)
        {
            var errorId = fAltaMovimiento(documentoId, ref movimientoId, ref movimiento);
            var result = GetError(errorId, "fAltaMovimiento");
            return result;
        }

        public string BuscarIdMovimiento(int movimientoId)
        {
            var errorId = fBuscarIdMovimiento(movimientoId);
            var result = GetError(errorId, "fBuscarIdMovimiento");
            return result;
        }

        public string AltaMovimientoSerieCapa(int movimientoId, tSeriesCapas seriesCapas)
        {
            var errorId = fAltaMovimientoSeriesCapas(movimientoId, ref seriesCapas);
            var result = GetError(errorId, "fAltaMovimientoSeriesCapas");
            return result;
        }

        public string ObtenerCampoCliente(string campo, out int errorId)
        {
            var valor = new StringBuilder(512);
            errorId = fLeeDatoCteProv(campo, valor, 350);
            return errorId == 0 ? valor.ToString() : "";
        }

        public string BuscarCteProv(string codigoCliente)
        {
            var errorId = fBuscaCteProv(codigoCliente);
            var result = GetError(errorId, "fBuscaCteProv");
            return result;
        }

        public string BorraCteProv()
        {
            var errorId = fBorraCteProv();
            var result = GetError(errorId, "fBorraCteProv");
            return result;
        }


        public string AltaCteProv(ref int clienteProveedorId, ref tCteProv clienteProveedor)
        {
            var errorId = fAltaCteProv(ref clienteProveedorId, ref clienteProveedor);
            var result = GetError(errorId, "fAltaCteProv");
            return result;
        }

        public string AltaDireccion(ref int direccionId, ref tDireccion direccion)
        {
            var errorId = fAltaDireccion(ref direccionId, ref direccion);
            var result = GetError(errorId, "fAltaDireccion");
            return result;
        }

        public string BuscarProducto(string codigoProducto, int fila = 0)
        {
            var errorId = fBuscaProducto(codigoProducto);
            var result = GetError(errorId, "fBuscaProducto", fila);
            return result;
        }

        public string AltaProducto(ref int productoId, ref tProducto producto)
        {
            var errorId = fAltaProducto(ref productoId, ref producto);
            var result = GetError(errorId, "fAltaProducto");
            return result;
        }

        public string ObtenerCampoProducto(string campo)
        {
            var valor = new StringBuilder(512);
            var errorId = fLeeDatoProducto(campo, valor, 350);
            return errorId == 0 ? valor.ToString() : "";
        }

        public string BuscarUnidad(string nombreUnidad)
        {
            var errorId = fBuscaUnidad(nombreUnidad);
            var result = GetError(errorId, "fBuscaUnidad");
            return result;
        }

        public string AltaUnidad(ref int unidadId, ref tUnidad unidad)
        {
            var errorId = fAltaUnidad(ref unidadId, ref unidad);
            var result = GetError(errorId, "fAltaUnidad");
            return result;
        }

        public string SetCuentaBancaria(string cuenta)
        {
            var errorId = fCuentaBancariaEmpresaDoctos(cuenta);
            var result = GetError(errorId, "fCuentaBancariaEmpresaDoctos");
            return result;
        }

        public string GetError(int errorId, string metodoSdk = "", int fila = 0)
        {
            var result = "";

            if (errorId == 0) return result;

            var myStringBuilder = new StringBuilder(512);
            fError(errorId, myStringBuilder, 350);
            result = myStringBuilder + (metodoSdk == "" ? "" : ", " + metodoSdk);

            return fila > 0? $"Error en fila {fila}: {result}": result;
        }
    }
}