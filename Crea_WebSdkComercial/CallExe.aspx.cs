using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Crea_WebSdkComercial
{
    public partial class CallExe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCallSdk_Click(object sender, EventArgs e)
        {           
            Process.Start(@"C:\XN - Crea 4 0\Moises\Proyectos\Crea_CaSdkComercial\Crea_CaSdkComercial\bin\Debug\Crea_CaSdkComercial.exe");
        }
    }
}