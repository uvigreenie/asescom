using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RJ.Areas.Seguridad.Models;

namespace RJ.Areas.Seguridad.Controllers
{
    public class GrupoController : Controller
    {
        #region Read

            public JsonResult Obtener()
            {
                List<object> lista = Grupo.Instancia.ObtenerGrupos(Convert.ToInt16(Session["Empresa"]));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

        #endregion
    }
}
