using Crea_CaSdkComercial.Code.Data;

namespace Crea_CaSdkComercial.Code.Business.Sdk
{
    public class FacturaAdenda
    {
        private readonly string _baseDatos;

        public FacturaAdenda(string baseDatos)
        {
            _baseDatos = baseDatos;
        }

        public void GuardarAdenda()
        {
            var sdkContext = new ContpaqiDbContext(_baseDatos);
        }
    }
}
