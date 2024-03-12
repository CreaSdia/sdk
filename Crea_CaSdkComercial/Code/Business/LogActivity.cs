using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crea_CaSdkComercial;
using SdkCreaComercial.Code.Data;

namespace SdkCreaComercial.Code.Business
{
    public class LogActivity
    {
        public int ActivityId { get; set; }
        public int AplicacionId { get; set; }
        public int DocumentoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public void SaveLog(SdkDbContext context, int documentoId, string descripcion)
        {
            this.AplicacionId = Settings.Default.AplicacionId;
            this.DocumentoId = documentoId;
            this.Descripcion = descripcion;
            this.FechaCreacion = DateTime.Now;
            context.LogActivities.Add(this);
        }
    }
}
