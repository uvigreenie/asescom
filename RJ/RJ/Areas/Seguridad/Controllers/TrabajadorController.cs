﻿using System;
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

                string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Trabajador\",\"dataIndex\":\"Trabajador\",\"hidden\":true,\"hideable\":false},";
                columns += "{\"text\":\"TipoDocumento\",\"dataIndex\":\"TipoDocumento\",\"hidden\":true,\"hideable\":false},{\"text\":\"Tipo Documento\",\"dataIndex\":\"DTipoDocumento\",\"width\":60},";
                columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":90},{\"text\":\"Apellido Paterno\",\"dataIndex\":\"ApellidoPaterno\",\"width\":180},";
                columns += "{\"text\":\"Apellido Materno\",\"dataIndex\":\"ApellidoMaterno\",\"width\":180},{\"text\":\"Nombres\",\"dataIndex\":\"Nombre\",\"width\":180},";
                columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fecha Nacimiento\",\"dataIndex\":\"FechaNacimiento\",\"width\":100,\"format\":\"d/m/Y\"},{\"text\":\"Sexo\",\"dataIndex\":\"Sexo\",\"width\":40},";
                columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"TelefonoFijo\",\"width\":90},{\"text\":\"Celular\",\"dataIndex\":\"Celular\",\"width\":90},";
                columns += "{\"text\":\"Correo\",\"dataIndex\":\"Correo\",\"width\":120},{\"text\":\"Correo Alterno\",\"dataIndex\":\"CorreoAlternativo\",\"width\":120},";
                columns += "{\"text\":\"Dirección\",\"dataIndex\":\"Direccion\",\"width\":120},{\"text\":\"Dirección Alterna\",\"dataIndex\":\"DireccionAlterna\",\"width\":120},";
                columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Activo\",\"dataIndex\":\"Activo\",\"processEvent\":'function () { return false; }',\"width\":100}]";

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
                                 FechaNacimiento = Convert.ToDateTime(m["FechaNacimiento"]).ToString("yyyy/MM/dd"),
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
                var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
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

            /// <summary>
            /// Lista trabajadores por usuario.
            /// </summary>
            /// <param name="usuario">id usuario.</param>
            /// <returns>Lista</returns>
            public JsonResult ListarTrabajadoresxUsuario(short usuario)
            {
                List<object> lista = Trabajador.Instancia.Listar(usuario);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarTrabajadoresxUsuarioLog()
            {
                short usuario = Convert.ToInt16(Session["Usuario"]);
                List<object> lista = Trabajador.Instancia.Listar(usuario);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            /// <summary>
            /// Lista trabajadores asignados a un usuario.
            /// </summary>
            /// <param name="usuario">id usuario.</param>
            /// <returns>Lista</returns>
            public JsonResult ListarTrabajadoresxAsignar(short usuario)
            {
                List<object> lista = Trabajador.Instancia.ListarTrabajadoresxAsignar(usuario);
                return Json(lista, JsonRequestBehavior.AllowGet);
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

            public JsonResult InsUpdUsuarioTrabajador(short usuario, object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());
                    string xml = "<root>";

                    for (int i = 0; i < lista.Count; i++)
                    {
                        xml += "<usuariotrabajador Trabajador = '" + lista[i]["Trabajador"].ToString() + "' />";
                    }
                    xml += "</root>";
                    Trabajador.Instancia.InsUpdUsuarioTrabajador(usuario, xml, Session["Login"].ToString());
                    return Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
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
