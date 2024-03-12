using System;
using System.IO;
using System.Linq;
using Crea_CaSdkComercial.Code.Servicios;
using SdkCreaComercial.Code.Business;
using SdkCreaComercial.Code.Data;
using SdkCreaComercial.Sdk;

namespace Crea_CaSdkComercial
{
    class Program
    {
        private static bool _isRunning;
        private static readonly SdkDbContext AcContext = new SdkDbContext();
        private static readonly Sdk Sdk = new Sdk();
        private static StreamWriter _sw;

        static void Main()
        {
            if (_isRunning) return;

            _isRunning = true;

            var logError = new LogError();
            var path = CrearPathLog();

            try
            {
                using (var sw = File.CreateText(path))
                {
                    _sw = sw;
                    var infoExcel = new InformacionExcel(_sw, Sdk);
                    var error = IniciarSesionSdk();

                    if (error != "") return;

                    error = AbrirEmpresa(infoExcel.Empresa);

                    if (error != "" || infoExcel.Error) return;

                    CrearDocumentos(infoExcel);
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message + ", " + ex.StackTrace;
                logError.SaveLog(AcContext, 0, error, "ConectarSdk");
                LogError.Guardar(_sw, error, TipoLog.Error);
            }
            finally
            {
                Sdk.fCierraEmpresa();
                Sdk.fTerminaSDK();

                AcContext.SaveChanges();
                _isRunning = false;
            }
        }

        private static string CrearPathLog()
        {
            var path = Settings.Default.LogEjecucion + DateTime.Today.ToString("yyyyMM") + "\\";
            var nombreArchivo = DateTime.Now.ToString("dd_HHmm") + ".txt";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += nombreArchivo;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return path;
        }

        private static string IniciarSesionSdk()
        {
            var mensaje = "Fecha de Ejecución: " + DateTime.Now.ToString("yy/MM/yyyy HH:mm");
            LogError.Guardar(_sw, mensaje, TipoLog.Informacion);

            var rutaSistema = Settings.Default.RutaSistema;
            var nombreSistema = Settings.Default.NombreSistema;

            var error = Sdk.IniciarSesion(rutaSistema, nombreSistema);

            if (error == "") return "";

            var logError = new LogError();
            logError.SaveLog(AcContext, 0, error, "IniciarSesionSdk");
            LogError.Guardar(_sw, "Error inicio de sesión SDK: " + error, TipoLog.Informacion);

            return error;
        }

        private static string AbrirEmpresa(string empresa)
        {
            var mensaje = "Abriendo Empresa: " + DateTime.Now.ToString("yy/MM/yyyy HH:mm");
            LogError.Guardar(_sw, mensaje, TipoLog.Informacion);
            var rutaEmpresa = Settings.Default.RutaEmpresas + empresa;
            var error = Sdk.AbrirEmpresa(rutaEmpresa);

            if (error == "") return "";

            var logError = new LogError();
            logError.SaveLog(AcContext, 0, error, "IniciarSesionSdk");
            LogError.Guardar(_sw, "Error al abir empresa: " + error, TipoLog.Informacion);

            return error;
        }

        private static void CrearDocumentos(InformacionExcel datos)
        {
            var documentos = datos.ObtenerDocumentos();

            if(documentos.Any(e=> e.Error != "")) return;

            foreach (var documento in documentos)
            {
                var error = Sdk.AltaDocumento(documento);
                if (error != "")
                {
                    LogError.Guardar(_sw, error, TipoLog.Error);
                    Console.ReadLine();
                }

                Utilities.CrearCorreo(documento, datos.Empresa, datos.AsuntoCorreo, datos.PlantillaCorreo, _sw);
            }
        }
    }
}