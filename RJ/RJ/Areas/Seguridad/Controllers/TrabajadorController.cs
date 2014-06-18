using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RJ.Areas.Seguridad.Models;
using System.Collections.Generic;

namespace RJ.Areas.Seguridad.Controllers
{
    public class TrabajadorController : Controller
    {
        #region Read

            public JsonResult Listar()
            {
                DataTable dt = Trabajador.Instancia.Listar();

                string fields = "[{\"name\":\"Trabajador\",\"type\":\"int\"},{\"name\":\"TipoDocumento\",\"type\":\"int\"},{\"name\":\"DTipoDocumento\",\"type\":\"string\"},";
                fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"ApellidoPaterno\",\"type\":\"string\"},{\"name\":\"ApellidoMaterno\",\"type\":\"string\"},";
                fields += "{\"name\":\"Nombre\",\"type\":\"string\"},{\"name\":\"FechaNacimiento\",\"type\":\"date\"},{\"name\":\"Sexo\",\"type\":\"string\"},";
                fields += "{\"name\":\"TelefonoFijo\",\"type\":\"string\"},{\"name\":\"Celular\",\"type\":\"string\"},{\"name\":\"Correo\",\"type\":\"string\"},";
                fields += "{\"name\":\"CorreoAlternativo\",\"type\":\"string\"},{\"name\":\"Direccion\",\"type\":\"string\"},{\"name\":\"DireccionAlterna\",\"type\":\"string\"},";
                fields += "{\"name\":\"Activo\",\"type\":\"bool\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Trabajador = Convert.ToInt16(m["Trabajador"]),
                                 TipoDocumento = Convert.ToByte(m["TipoDocumento"]),
                                 DTipoDocumento = m["DTipoDocumento"].ToString(),
                                 NumeroDocumento = m["NumeroDocumento"].ToString(),
                                 ApellidoPaterno = m["ApellidoPaterno"].ToString(),
                                 ApellidoMaterno = m["ApellidoMaterno"].ToString(),
                                 Nombre = m["Nombre"].ToString(),
                                 FechaNacimiento = Convert.ToDateTime(m["FechaNacimiento"]),
                                 Sexo = m["Sexo"].ToString(),
                                 TelefonoFijo = m["TelefonoFijo"].ToString(),
                                 Celular = m["Celular"].ToString(),
                                 Correo = m["Correo"].ToString(),
                                 CorreoAlternativo = m["CorreoAlternativo"].ToString(),
                                 Direccion = m["Direccion"].ToString(),
                                 DireccionAlterna = m["DireccionAlterna"].ToString(),
                                 Activo = Convert.ToBoolean(m["Activo"])
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarTipoDocumento()
            {
                DataTable dt = Trabajador.Instancia.ListarTipoDocumento();

                string fields = "[{\"name\":\"TipoDocumento\",\"type\":\"int\"},{\"name\":\"DTipoDocumento\",\"type\":\"string\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 TipoDocumento = Convert.ToInt16(m["TipoDocumento"]),
                                 DTipoDocumento = m["DTipoDocumento"].ToString()
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

        #endregion

        #region Insert

            public JsonResult InsUpdTrabajador(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    short trabajador = Convert.ToInt16(lista[0]["Trabajador"]);
                    byte tipoDocumento = Convert.ToByte(lista[0]["TipoDocumento"]);
                    string numeroDocumento = lista[0]["NumeroDocumento"].ToString();
                    string apellidoPaterno = lista[0]["ApellidoPaterno"].ToString();
                    string apellidoMaterno = lista[0]["ApellidoMaterno"].ToString();
                    string nombre = lista[0]["Nombre"].ToString();
                    DateTime fecha = Convert.ToDateTime(lista[0]["FechaNacimiento"]);
                    string sexo = lista[0]["Sexo"].ToString();
                    string telefonoFijo = lista[0]["TelefonoFijo"].ToString();
                    string celular = lista[0]["Celular"].ToString();
                    string correo = lista[0]["Correo"].ToString();
                    string correoAlternativo = lista[0]["CorreoAlternativo"].ToString();
                    string direccion = lista[0]["Direccion"].ToString();
                    string direccionAlterna = lista[0]["DireccionAlterna"].ToString();
                    bool activo = Convert.ToBoolean(lista[0]["Activo"]);
                    short result = Trabajador.Instancia.InsUpdTrabajador(trabajador,tipoDocumento,numeroDocumento,apellidoPaterno,apellidoMaterno,nombre,fecha,sexo,telefonoFijo,celular,correo,correoAlternativo,direccion,direccionAlterna, activo, Session["Login"].ToString());
                    return Json(new { success = "true", data = result.ToString() }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion

        #region Anular

            public JsonResult Anular(short trabajador)
            {
                try
                {
                    Trabajador.Instancia.Anular(trabajador, Session["Login"].ToString());
                    return Json(new { success = "true", data = trabajador.ToString() }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion
    }
}
