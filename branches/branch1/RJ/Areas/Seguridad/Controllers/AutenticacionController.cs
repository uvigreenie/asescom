using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RJ.Areas.Seguridad.Models;
using System.Web.Script.Serialization;

namespace RJ.Areas.Seguridad.Controllers
{
    public class AutenticacionController : Controller
    {
            public ActionResult Login()
            {
                Session["Usuario"] = null;
                Session["Login"] = null;
                Session["Nombres"] = null;
                Session["Empresa"] = null;
                return View();
            }

        #region Read

            [HttpPost]
            public JsonResult Verificar(string usuario, string password, UInt16 empresa)
            {
                try
                {
                    List<Autenticacion> lista = Autenticacion.Instancia.VerificarLogin(usuario, password, empresa);
                    if (lista.Count > 0 && ((Autenticacion)lista[0]).Activo)
                    {
                        Session["Usuario"] = ((Autenticacion)lista[0]).Usuario;
                        Session["Login"] = ((Autenticacion)lista[0]).Login;
                        Session["Nombres"] = ((Autenticacion)lista[0]).Nombres;
                        Session["Empresa"] = ((Autenticacion)lista[0]).Empresa;
                    }
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public JsonResult ListarUsuarios()
            {
                List<object> lista = Autenticacion.Instancia.ListarUsuarios(Convert.ToByte(Session["Empresa"]));
                return Json(lista, JsonRequestBehavior.AllowGet);
            }
        
            public JsonResult ObtenerDatosSession()
            {
                List<Autenticacion> lista = new List<Autenticacion>();
                try
                {
                    Autenticacion data = new Autenticacion();
                    data.Usuario = Convert.ToInt32(Session["Usuario"]);
                    data.Login = Session["Login"].ToString();
                    data.Nombres = Session["Nombres"].ToString();
                    data.Empresa = Convert.ToInt32(Session["Empresa"]);
                    lista.Add(data);

                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
            }

            public JsonResult ObtenerTreeMenu(byte grupo, short usuario)
            {
                if (grupo == 0)
                {
                    return Json(new List<TreeMenu>(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<TreeMenu> lista = TreeMenu.Instancia.ListarMenuxUsuario(grupo, usuario);
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
            }

            public JsonResult ObtenerAccesoTreeMenu(byte grupo, short usuario)
            {
                if (grupo == 0)
                {
                    return Json(new List<TreeMenu>(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    List<TreeMenu> lista = TreeMenu.Instancia.ListarAccesoMenuUsuario(grupo, usuario);
                    return Json(lista, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion

            #region Insertar

                public JsonResult InsUpdUsuarios(object[] datos)
                {
                    try
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                        byte empresa = Convert.ToByte(Session["Empresa"]);
                        short usuario = Convert.ToInt16(lista[0]["Usuario"]);
                        string login = lista[0]["Login"].ToString().ToLower();
                        string nombres = lista[0]["Nombres"].ToString();
                        string correo = lista[0]["Correo"].ToString();
                        bool activo = Convert.ToBoolean(lista[0]["Activo"]);
                        short result = Autenticacion.Instancia.InsUpdUsuario(empresa, usuario, login, nombres, correo, activo, Session["Login"].ToString());
                        return Json(new { success = "true", data = result.ToString() }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }

            #endregion

            #region Update

                public JsonResult ChangePassword(string passwordOld, string passwordNew)
                {
                    bool result = Autenticacion.Instancia.ChangePassword(Convert.ToInt32(Session["Usuario"]), passwordOld, passwordNew, Session["Login"].ToString());
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            #endregion

            #region Anular

                public JsonResult AnularUsuario(short usuario)
                {
                    try
                    {
                        Autenticacion.Instancia.AnularUsuario(usuario, Session["Login"].ToString());
                        return Json(new { success = "true", data = usuario.ToString() }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                }

            #endregion

    }
}
