using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RJ.Areas.Cobranza.Models;

namespace RJ.Areas.Cobranza.Controllers
{
    public class ClienteController : Controller
    {
        #region Read

            public JsonResult Listar()
            {
                DataTable dt = Cliente.Instancia.ListarClientes();
                string fields = "[{\"name\":\"Cliente\",\"type\":\"string\"},{\"name\":\"DCliente\",\"type\":\"string\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Cliente = m["Cliente"].ToString(),
                                 DCliente = m["DCliente"].ToString()
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }
        
        #endregion
        
    }
}
