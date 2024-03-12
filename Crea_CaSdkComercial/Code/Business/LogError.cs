using System;
using System.IO;using Crea_CaSdkComercial;
using SdkCreaComercial.Code.Data;

namespace SdkCreaComercial.Code.Business
{
    public class LogError
    {
        public int ErrorId { get; set; }
        public int AplicacionId { get; set; }
        public int DocumentoId { get; set; }
        public string Error { get; set; }
        public string Metodo { get; set; }
        public DateTime FechaError { get; set; }

        public void SaveLog(SdkDbContext context, int documentoId, string error, string metodo)
        {
            this.AplicacionId = Settings.Default.AplicacionId;
            this.DocumentoId = documentoId;
            this.Error = error;
            this.Metodo = metodo;
            this.FechaError = DateTime.Now;
            //context.LogErrors.Add(this);
        }

        public static void Guardar(StreamWriter sw, string mensaje, TipoLog tipoLog, int fila = 0)
        {
            mensaje = fila == 0 ? mensaje : $"Fila {fila}: {mensaje}";

            Console.ForegroundColor = tipoLog == TipoLog.Error ? ConsoleColor.Red : ConsoleColor.Yellow;

            Console.WriteLine(mensaje);
            sw.WriteLine(mensaje);
        }
    }

    public enum TipoLog
    {
        Informacion = 1,
        Error = 2
    }
}
