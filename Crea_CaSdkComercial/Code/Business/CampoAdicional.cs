using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SdkCreaComercial.Code.Business
{
    public class CampoAdicional
    {
        public string Nombre { get; set; }
        public string Valor { get; set; } = "";
        public int Columna { get; set; }
        public string TipoCampo { 
            get; 
            set;
        }

        public CampoAdicional(){}

        public CampoAdicional(CampoAdicional campoAdicional)
        {
            Nombre = campoAdicional.Nombre;
            Valor = campoAdicional.Valor;
            Columna = campoAdicional.Columna;
            TipoCampo = campoAdicional.TipoCampo;
        }
    }
}
