using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RJ.Areas.Cobranza.Models;

namespace RJ.Areas.Cobranza.Controllers
{
    public class TipoDetalleController : Controller
    {
        #region Read

            public JsonResult Listar()
            {
                DataTable dt = TipoDetalle.Instancia.Listar();

                string fields = "[{\"name\":\"TipoDetalle\",\"type\":\"int\"},{\"name\":\"DTipoDetalle\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { TipoDetalle = Convert.ToInt16(c["TipoDetalle"]), DTipoDetalle = c["DTipoDetalle"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

        #endregion
    }
}
