using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RJ.Areas.Seguridad.Models;
using System.Web.Script.Serialization;


namespace RJ.Areas.Seguridad.Controllers
{
    public class MenuController : Controller
    {
        #region Read

            public JsonResult Obtener(byte grupo)
            {
                if (grupo >= 0)
                {
                    List<object> lista = Menu.Instancia.ObtenerMenus(Convert.ToByte(Session["Empresa"]), Convert.ToByte(grupo));
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);
                }
            }

        #endregion

        #region Insert

            public JsonResult InsUpdMenus(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    short menu = Convert.ToInt16(lista[0]["Menu"]);
                    byte grupo = Convert.ToByte(lista[0]["Grupo"]);
                    short padre = Convert.ToInt16(lista[0]["Padre"]);
                    string nombre = lista[0]["Nombre"].ToString();
                    string tooltip = lista[0]["Tooltip"].ToString();
                    string clase = lista[0]["Clase"].ToString();
                    string icono = lista[0]["Icono"].ToString();
                    bool leaf = Convert.ToBoolean(lista[0]["Leaf"]);
                    short result = Menu.Instancia.InsUpdMenus(menu, grupo, padre, nombre, tooltip, clase, icono, leaf, Session["Login"].ToString());
                    return Json(new { success = "true", data = result.ToString() }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            public JsonResult GuardarMenuUsuario(object[] datos, short usuario)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());
                    string xml = "<root>";

                    for (int i = 0; i < lista.Count; i++)
                    {
                        xml += "<menuusuario Menu = '" + lista[i]["Menu"].ToString() + "' Acceso = '" + lista[i]["Acceso"].ToString() + "' />";
                    }
                    xml += "</root>";
                    if (lista.Count == 0)
                    {
                        return Json(new { success = "true", data = "false" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        Menu.Instancia.GuardarMenuUsuario(xml, usuario, Session["Login"].ToString());
                        return Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion

        #region Anular

            public JsonResult Anular(short menu)
            {
                try
                {
                    Menu.Instancia.AnularMenu(menu, Session["Login"].ToString());
                    return Json(new { success = "true", data = menu.ToString() }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion
    }
}
