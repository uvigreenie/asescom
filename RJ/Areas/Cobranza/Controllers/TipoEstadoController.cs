using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RJ.Areas.Cobranza.Models;

namespace RJ.Areas.Cobranza.Controllers
{
    public class TipoEstadoController : Controller
    {
        #region Read

            public JsonResult Listar()
            {
                DataTable dt = TipoEstado.Instancia.Listar();

                string fields = "[{\"name\":\"TipoEstado\",\"type\":\"int\"},{\"name\":\"DTipoEstado\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { TipoEstado = Convert.ToInt16(c["TipoEstado"]), DTipoEstado = c["DTipoEstado"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

        #endregion
    }
}
