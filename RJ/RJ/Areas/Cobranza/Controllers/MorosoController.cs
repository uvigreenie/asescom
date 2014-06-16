using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using RJ.Areas.Cobranza.Entity;
using RJ.Areas.Cobranza.Models;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;
//Revision 12
namespace RJ.Areas.Cobranza.Controllers
{
    public class MorosoController : Controller
    {
        #region Read

            public JsonResult ListarRubroEmpleo()
            {
                List<object> lista = Moroso.Instancia.ListarRubroEmpleo();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ObtenerDatosMoroso(int moroso)
            {
                List<object> lista = Moroso.Instancia.ObtenerDatosMoroso(moroso);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

        #endregion

        #region Insert

            public JsonResult UpdMoroso(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    int moroso = Convert.ToInt32(lista[0]["Moroso"]);
                    bool empleado = Convert.ToBoolean(lista[0]["Empleado"]);
                    int? rubroEmpleo = (Convert.ToInt32(lista[0]["RubroEmpleo"])== 0 ? new int?() : Convert.ToInt32(lista[0]["RubroEmpleo"]));
                    DateTime? horaContacto = ( lista[0]["HoraContacto"].ToString().Length == 0 ? new DateTime?() : Convert.ToDateTime(lista[0]["HoraContacto"]));
                    string observacion = lista[0]["Observacion"].ToString();
                    int result = Moroso.Instancia.UpdMoroso(moroso, empleado, rubroEmpleo, horaContacto, observacion, Session["Login"].ToString());
                    return Json(new { success = "true", data = result.ToString() }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion
    }
}
    