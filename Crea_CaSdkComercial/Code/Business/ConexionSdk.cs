using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crea_CaSdkComercial;
using SdkCreaComercial.Business.Sdk;
using SdkCreaComercial.Code.Data;
using SdkCreaComercial.Sdk;

namespace SdkCreaComercial.Code.Business
{
    class ConexionSdk
    {
        private static bool _isRunning;
        private static readonly SdkDbContext AcContext = new SdkDbContext();

        public static void ConectarSdk(object sender, EventArgs e)
        {
            if (_isRunning) return;
          
            _isRunning = true;
            var logActivity = new LogActivity();
            var logError = new LogError();

            //var lastActivity = AcContext.LogActivities.Where(x => x.FechaCreacion.Day == DateTime.Today.Day).ToList();
            logActivity.SaveLog(AcContext, 0, "Inicia el servicio");

            try
            {
                string error;
                var errorId = 0;

                var rutaSistema = Settings.Default.RutaSistema;
                var nombreSistema = Settings.Default.NombreSistema;
                var rutaEmpresas = Settings.Default.RutaEmpresas;
                var empresas = Settings.Default.Empresas.Split(',');
         
                var sdk = new Sdk.Sdk(rutaSistema, nombreSistema, out error);

                if (error == "")
                {
                    foreach (var rutaEmpresa in empresas.Select(empresa => rutaEmpresas + empresa))
                    {
                        error = sdk.AbrirEmpresa(rutaEmpresa);

                        if (error == "")
                        {
                            //Utilities.TimbrarDocumentosRecurrentes(sdk, AcContext);

                            var codigoCliente = "ZPE081205174";
                            //errorId = Sdk.Sdk.fBuscaCteProv(codigoCliente);

                            if (errorId == 0)
                            {
                                //var documentoId = 0;
                                //var documento = new tDocumento
                                //{
                                //    aCodConcepto = 5.ToString(),
                                //    aSistemaOrigen = 5,
                                //    aFecha = DateTime.Today.ToString("MM/dd/yyyy"),
                                //    aNumMoneda = 1,
                                //    aTipoCambio = 1,
                                //    aCodigoCteProv = codigoCliente
                                //};

                                //var movimientos = new List<tMovimiento>();

                                //movimientos.Add(new tMovimiento
                                //{
                                //    aConsecutivo = 1,
                                //    aCodProdSer = "PROD01",
                                //    aUnidades = 4,
                                //    aPrecio = 50.96,
                                //    aCodAlmacen = 1.ToString()
                                //});

                                //movimientos.Add(new tMovimiento
                                //{
                                //    aConsecutivo = 1,
                                //    aCodProdSer = "PROD02",
                                //    aUnidades = 3,
                                //    aPrecio = 276.43,
                                //    aCodAlmacen = 1.ToString()
                                //});

                                //error = sdk.CrearDocumento(ref documento, movimientos, true);
                            }
                            else if (errorId == 3)
                            {
                                var clienteId = 0;
                                var cliente = new tCteProv
                                {
                                    cCodigoCliente = codigoCliente,
                                    cRazonSocial = "SDIA Consultoria 98",
                                    cRFC = "SDI0907H30",
                                    cTipoCliente = 1,
                                    cNombreMoneda = "Peso Mexicano",
                                    cListaPreciosCliente = 1,
                                    cFechaAlta = DateTime.Today.ToString("MM/dd/yyyy")
                                };

                                //var cliente = new Dictionary<string, string>
                                //{
                                //    { "cCodigoCliente", codigoCliente },
                                //    { "cRazonSocial", "SDIA Consultoria 97" },
                                //    { "cRFC", "SDI0907H30" },
                                //    { "cTipoCliente", "1" },
                                //    { "CIDMONEDA", "1" },
                                //    { "cListaPrecioCliente", "1" },
                                //    { "cFechaAlta", DateTime.Today.ToString("MM/dd/yyyy") }
                                //};

                                var direccion = new tDireccion
                                {
                                    cCodCteProv = codigoCliente,
                                    cTipoCatalogo = 1,
                                    cTipoDireccion = 0,
                                    cNombreCalle = "Caxpa",
                                    cNumeroExterior = "10",
                                    cNumeroInterior = "",
                                    cColonia = "Barrio de San Lucas",
                                    cCodigoPostal = "04030",
                                    cPais = "México",
                                    cEstado = "Distrito Federal"
                                };

                                //error = sdk.CrearClienteProveedor(ref clienteId, ref cliente, direccion);

                                if (error == "")
                                {
                                    var direccionId = 0;


                                    //var direccion = new Dictionary<string, string>
                                    //        {
                                    //            { "CIDCATALOGO", idCliente },
                                    //            { "CTIPOCATALOGO", "1" },
                                    //            { "CTIPODIRECCION", "0" },
                                    //            { "CNOMBRECALLE", "Caxpa" },
                                    //            { "CNUMEROEXTERIOR", "10" },
                                    //            { "CNUMEROINTERIOR", "" },
                                    //            { "CCOLONIA", "Barrio de San Lucas" },
                                    //            { "CCODIGOPOSTAL", "04030" },
                                    //            { "CPAIS", "México" },
                                    //            { "CESTADO", "Distrito Federal" },
                                    //            { "CMUNICIPIO", "Coyoacán" }
                                    //        };                                    
                                }
                            }
                            else
                            {
                                error = sdk.GetError(errorId);
                            }

                            Sdk.Sdk.fCierraEmpresa();
                        }
                    }

                    Sdk.Sdk.fTerminaSDK();
                }

                if (error != "")
                {
                    logError = new LogError();
                    logError.SaveLog(AcContext, 0, error, "ConectarSdk");
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message + ", " + ex.StackTrace;
                logError.SaveLog(AcContext, 0, error, "ConectarSdk");
            }
            finally
            {
                AcContext.SaveChanges();
                _isRunning = false;
            }
        }

        public void ValidarCrearCliente(ref string error)
        {
            var codigoCliente = "P30";
            var errorId = Sdk.Sdk.fBuscaCteProv(codigoCliente);


        }
    }
}
