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
using C1.C1Excel;

namespace RJ.Areas.Cobranza.Controllers
{
    public class CarteraController : Controller
    {
        #region read

            public JsonResult ListarFechaFinCartera(short gestionCliente)
            {
                DataTable dt = Cartera.Instancia.ListarFechaFinCartera(gestionCliente);

                string fields = "[{\"name\":\"FechaFin\",\"type\":\"string\"},{\"name\":\"DFechaFin\",\"type\":\"string\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 FechaFin = Convert.ToDateTime(m["FechaFin"]).ToString("yyyyMMdd"),
                                 DFechaFin = Convert.ToDateTime(m["FechaFin"]).ToString("dd/MM/yyyy")
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarFechaInicioCartera(short gestionCliente)
            {
                DataTable dt = Cartera.Instancia.ListarFechaInicioCartera(gestionCliente);

                string fields = "[{\"name\":\"FechaInicio\",\"type\":\"string\"},{\"name\":\"DFechaInicio\",\"type\":\"string\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 FechaInicio = Convert.ToDateTime(m["FechaInicio"]).ToString("yyyyMMdd"),
                                 DFechaInicio = Convert.ToDateTime(m["FechaInicio"]).ToString("dd/MM/yyyy")
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarTramo(int gestionCliente, string fechaFin)
            {
                DataTable dt = Cartera.Instancia.ListarTramo(gestionCliente, fechaFin);

                string fields = "[{\"name\":\"Tramo\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Tramo = c["Tramo"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarDepartamentoIBK(int gestionCliente, string fechaInicio)
            {
                DataTable dt = Cartera.Instancia.ListarDepartamentoIBK(gestionCliente, fechaInicio);

                string fields = "[{\"name\":\"Departamento\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Departamento = c["Departamento"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarZonal(int gestionCliente, string fechaFin)
            {
                DataTable dt = Cartera.Instancia.ListarZonal(gestionCliente, fechaFin);

                string fields = "[{\"name\":\"Zonal\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Zonal = c["Zonal"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarCluster(int gestionCliente, string fechaFin, string tramo)
            {
                DataTable dt = Cartera.Instancia.ListarCluster(gestionCliente, fechaFin, tramo);

                string fields = "[{\"name\":\"Cluster\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Cluster = c["Cluster"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarDepartamento(int gestionCliente, string fechaInicio,string fechaFin, string tramo, object[] clusters)
            {
                JsonResult respuestaJson = new JsonResult();
                if (gestionCliente == 1)
                {
                    string xmlDpto = "<root>";

                    for (int i = 0; i < clusters.Length; i++)
                    {
                        xmlDpto += "<cluster Cluster = '" + clusters[i].ToString() + "' />";
                    }
                    xmlDpto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarDepartamento(gestionCliente, fechaFin, tramo, xmlDpto);

                    string fields = "[{\"name\":\"Departamento\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Departamento = c["Departamento"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 2)
                {
                    DataTable dt = Cartera.Instancia.ListarDepartamentoIBK(gestionCliente, fechaInicio);

                    string fields = "[{\"name\":\"Departamento\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Departamento = c["Departamento"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarDepartamentoxZonal(int gestionCliente, string fechaFin, string fechaInicio, object[] zonales)
            {
                JsonResult respuestaJson = new JsonResult();
                if (gestionCliente == 1) {
                    string xmlZonal = "<root>";

                    if (zonales != null)
                    {
                        for (int i = 0; i < zonales.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonales[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    DataTable dt = Cartera.Instancia.ListarDepartamentoxZonal(gestionCliente, fechaFin, xmlZonal);

                    string fields = "[{\"name\":\"Departamento\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Departamento = c["Departamento"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }else if (gestionCliente==2)
                {
                    DataTable dt = Cartera.Instancia.ListarDepartamentoIBK(gestionCliente, fechaInicio);

                    string fields = "[{\"name\":\"Departamento\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Departamento = c["Departamento"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarTramoxDepartamento(int gestionCliente, string fechaFin, string fechaInicio, object[] zonales, object[] departamento)
            {
                JsonResult respuestaJson = new JsonResult();

                if (gestionCliente == 1) {
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    if (zonales != null)
                    {
                        for (int i = 0; i < zonales.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonales[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    DataTable dt = Cartera.Instancia.ListarTramoxDepartamento(gestionCliente, fechaFin, xmlZonal, xmlDpto);

                    string fields = "[{\"name\":\"Tramo\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Tramo = c["Tramo"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson =  Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 2) {
                    string xmlDpto = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarTramoxDptoIBK(gestionCliente, fechaInicio, xmlDpto);

                    string fields = "[{\"name\":\"Tramo\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Tramo = c["Tramo"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarProductoxDepartamento(int gestionCliente, string fechaInicio, object[] departamento)
            {
                JsonResult respuestaJson = new JsonResult();

                if (gestionCliente == 2)
                {
                    string xmlDpto = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarProductoxDptoIBK(gestionCliente, fechaInicio, xmlDpto);

                    string fields = "[{\"name\":\"Producto\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { Producto = c["Producto"].ToString() }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }
        public JsonResult ListarClusterxTramo(int gestionCliente, string fechaFin, object[] zonales, object[] departamento, object[] tramo)
        {
            string xmlZonal = "<root>";
            string xmlDpto = "<root>";
            string xmlTramo = "<root>";

            if (departamento != null)
            {
                for (int i = 0; i < departamento.Length; i++)
                {
                    xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                }
            }
            xmlDpto += "</root>";

            if (zonales != null)
            {
                for (int i = 0; i < zonales.Length; i++)
                {
                    xmlZonal += "<zonal Zonal = '" + zonales[i].ToString() + "' />";
                }
            }
            xmlZonal += "</root>";

            if (tramo != null)
            {
                for (int i = 0; i < tramo.Length; i++)
                {
                    xmlTramo += "<tramo Tramo = '" + tramo[i].ToString() + "' />";
                }
            }
            xmlTramo += "</root>";

            DataTable dt = Cartera.Instancia.ListarClusterxTramo(gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo);

            string fields = "[{\"name\":\"Cluster\",\"type\":\"string\"}]";

            var lista = (from c in dt.AsEnumerable()
                            select new { Cluster = c["Cluster"].ToString() }).ToList<object>();

            var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
            return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarProvinciaxDpto(int gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, string dpto)
        {
            JsonResult respuestaJson = new JsonResult();
            if (gestionCliente == 1) 
            {
                string xmlDpto = "<root>";
                if (departamento != null)
                {
                    for (int i = 0; i < departamento.Length; i++)
                    {
                        xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                    }
                }
                xmlDpto += "</root>";

                string xmlZonal = "<root>";
                if (zonal != null)
                {
                    for (int i = 0; i < zonal.Length; i++)
                    {
                        xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                    }
                }
                xmlZonal += "</root>";

                DataTable dt = Cartera.Instancia.ListarProvinciaxDpto(gestionCliente, fechaFin, xmlZonal, xmlDpto);

                string fields = "[{\"name\":\"Provincia\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                                select new { Provincia = c["Provincia"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                respuestaJson= Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }
            else if (gestionCliente == 2)
            {
                DataTable dt = Cartera.Instancia.ListarProvinciaxDptoIBK(gestionCliente, fechaInicio, dpto);

                string fields = "[{\"name\":\"Provincia\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Provincia = c["Provincia"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }
            return respuestaJson;
        }

        public JsonResult ListarDistritoxProv(int gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, string dpto, string provincia)
        {
            JsonResult respuestaJson = new JsonResult();
            if (gestionCliente == 1) {
                string xmlDpto = "<root>";
                if (departamento != null)
                {
                    for (int i = 0; i < departamento.Length; i++)
                    {
                        xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                    }
                }
                xmlDpto += "</root>";

                string xmlZonal = "<root>";
                if (zonal != null)
                {
                    for (int i = 0; i < zonal.Length; i++)
                    {
                        xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                    }
                }
                xmlZonal += "</root>";

                DataTable dt = Cartera.Instancia.ListarDistritoxProv(gestionCliente, fechaFin, xmlZonal, xmlDpto, provincia);

                string fields = "[{\"name\":\"Distrito\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Distrito = c["Distrito"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }
            else if (gestionCliente == 2) {
                DataTable dt = Cartera.Instancia.ListarDistritoxProvIBK(gestionCliente, fechaInicio, dpto, provincia);

                string fields = "[{\"name\":\"Distrito\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Distrito = c["Distrito"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }
            return respuestaJson;
        }

            public JsonResult ListarGestiones(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, DateTime fechaDesde, DateTime fechaHasta, bool mejorGestion)
            {
                JsonResult respuestaJson = new JsonResult();

                if (gestionCliente == 1)
                {
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    DataTable dt = new DataTable();
                    if (!mejorGestion)
                    {
                        dt = Cartera.Instancia.ListarGestionesGrid(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    }
                    else
                    {
                        dt = Cartera.Instancia.ListarMejorGestionGrid(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    }
                    //DataTable dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    string fields = "[{\"name\":\"codcartera\",\"type\":\"string\"},{\"name\":\"cliente\",\"type\":\"string\"},{\"name\":\"cuenta\",\"type\":\"string\"},";
                    fields += "{\"name\":\"servicio\",\"type\":\"string\"},{\"name\":\"razonsocial\",\"type\":\"string\"},{\"name\":\"FechaRegistro\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Codigo\",\"type\":\"string\"},{\"name\":\"Respuesta\",\"type\":\"string\"},{\"name\":\"Incidencia\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Observacion\",\"type\":\"string\"},{\"name\":\"fechapromesa\",\"type\":\"string\"},{\"name\":\"departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"provincia\",\"type\":\"string\"},{\"name\":\"distrito\",\"type\":\"string\"},{\"name\":\"zonal\",\"type\":\"string\"},";
                    fields += "{\"name\":\"usuarioregistro\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"codcartera\",\"width\":200},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"cliente\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"cuenta\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Servicio\",\"dataIndex\":\"servicio\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Nombre\",\"dataIndex\":\"razonsocial\",\"width\":250,\"filterable\":true},";
                    columns += "{\"text\":\"Fecha de Gestión\",\"dataIndex\":\"FechaRegistro\",\"width\":100},";
                    columns += "{\"text\":\"Código\",\"dataIndex\":\"Codigo\",\"width\":60},";
                    columns += "{\"text\":\"Respuesta\",\"dataIndex\":\"Respuesta\",\"width\":150},";
                    columns += "{\"text\":\"Incidencia\",\"dataIndex\":\"Incidencia\",\"width\":150},";
                    columns += "{\"text\":\"Observación\",\"dataIndex\":\"Observacion\",\"width\":300},";
                    columns += "{\"text\":\"Fecha de Promesa\",\"dataIndex\":\"fechapromesa\",\"width\":100},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"departamento\",\"width\":120,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"provincia\",\"width\":120},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"distrito\",\"width\":120},";
                    columns += "{\"text\":\"Zonal\",\"dataIndex\":\"zonal\",\"width\":60},";
                    columns += "{\"text\":\"Usuario\",\"dataIndex\":\"usuarioregistro\",\"width\":100}]";
                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     codcartera = m["codcartera"].ToString(),
                                     cliente = m["cliente"].ToString(),
                                     cuenta = m["cuenta"].ToString(),
                                     servicio = m["servicio"].ToString(),
                                     razonsocial = m["razonsocial"].ToString(),
                                     FechaRegistro = (m["FechaRegistro"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaRegistro"]).ToString("yyyy/MM/dd")),
                                     Codigo = m["Codigo"].ToString(),
                                     Respuesta = m["Respuesta"].ToString(),
                                     Incidencia = m["Incidencia"].ToString(),
                                     Observacion = m["Observacion"].ToString(),
                                     fechapromesa = (m["fechapromesa"] == DBNull.Value ? "" : Convert.ToDateTime(m["fechapromesa"]).ToString("yyyy/MM/dd")),
                                     departamento = m["departamento"].ToString(),
                                     provincia = m["provincia"].ToString(),
                                     distrito = m["distrito"].ToString(),
                                     zonal = m["zonal"].ToString(),
                                     usuarioregistro = m["usuarioregistro"].ToString()
                                 }).ToList<object>();
                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 2)
                {
                    string xmlDpto = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    DataTable dt = new DataTable();
                    if (!mejorGestion)
                    {
                        dt = Cartera.Instancia.ListarGestionesGridIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                    }
                    else
                    {
                        dt = Cartera.Instancia.ListarMejorGestionGridIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                    }
                    //DataTable dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    string fields = "[{\"name\":\"codcartera\",\"type\":\"string\"},{\"name\":\"CodigoCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"razonsocial\",\"type\":\"string\"},{\"name\":\"FechaRegistro\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Codigo\",\"type\":\"string\"},{\"name\":\"Respuesta\",\"type\":\"string\"},{\"name\":\"Incidencia\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Observacion\",\"type\":\"string\"},{\"name\":\"fechapromesa\",\"type\":\"string\"},{\"name\":\"departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"provincia\",\"type\":\"string\"},{\"name\":\"distrito\",\"type\":\"string\"},";
                    fields += "{\"name\":\"usuarioregistro\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"codcartera\",\"width\":100},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodigoCliente\",\"width\":110,\"filterable\":true},";
                    columns += "{\"text\":\"Nombre\",\"dataIndex\":\"razonsocial\",\"width\":250,\"filterable\":true},";
                    columns += "{\"text\":\"Fecha de Gestión\",\"dataIndex\":\"FechaRegistro\",\"width\":100},";
                    columns += "{\"text\":\"Código\",\"dataIndex\":\"Codigo\",\"width\":60},";
                    columns += "{\"text\":\"Respuesta\",\"dataIndex\":\"Respuesta\",\"width\":150},";
                    columns += "{\"text\":\"Incidencia\",\"dataIndex\":\"Incidencia\",\"width\":150},";
                    columns += "{\"text\":\"Observación\",\"dataIndex\":\"Observacion\",\"width\":300},";
                    columns += "{\"text\":\"Fecha de Promesa\",\"dataIndex\":\"fechapromesa\",\"width\":100},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"departamento\",\"width\":120,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"provincia\",\"width\":120},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"distrito\",\"width\":120},";
                    columns += "{\"text\":\"Usuario\",\"dataIndex\":\"usuarioregistro\",\"width\":100}]";
                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     codcartera = m["codcartera"].ToString(),
                                     CodigoCliente = m["CodigoCliente"].ToString(),
                                     razonsocial = m["razonsocial"].ToString(),
                                     FechaRegistro = (m["FechaRegistro"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaRegistro"]).ToString("yyyy/MM/dd")),
                                     Codigo = m["Codigo"].ToString(),
                                     Respuesta = m["Respuesta"].ToString(),
                                     Incidencia = m["Incidencia"].ToString(),
                                     Observacion = m["Observacion"].ToString(),
                                     fechapromesa = (m["fechapromesa"] == DBNull.Value ? "" : Convert.ToDateTime(m["fechapromesa"]).ToString("yyyy/MM/dd")),
                                     departamento = m["departamento"].ToString(),
                                     provincia = m["provincia"].ToString(),
                                     distrito = m["distrito"].ToString(),
                                     usuarioregistro = m["usuarioregistro"].ToString()
                                 }).ToList<object>();
                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarPagos(string cliente, short gestionCliente, string fechaFin, object[] zonal, object[] departamento, DateTime fechaDesde, DateTime fechaHasta, bool acumulado)
            {
                string xmlZonal = "<root>";
                string xmlDpto = "<root>";

                if (zonal != null)
                {
                    for (int i = 0; i < zonal.Length; i++)
                    {
                        xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                    }
                }
                xmlZonal += "</root>";

                if (departamento != null)
                {
                    for (int i = 0; i < departamento.Length; i++)
                    {
                        xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                    }
                }
                xmlDpto += "</root>";
                if (!acumulado)
                {
                    DataTable dt = Cartera.Instancia.ListarPagos(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    string fields = "[{\"name\":\"codcartera\",\"type\":\"string\"},{\"name\":\"cliente\",\"type\":\"string\"},{\"name\":\"cuenta\",\"type\":\"string\"},";
                    fields += "{\"name\":\"servicio\",\"type\":\"string\"},{\"name\":\"razonsocial\",\"type\":\"string\"},{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"TipoDocumento\",\"type\":\"string\"},{\"name\":\"MontoDeuda\",\"type\":\"float\"},{\"name\":\"FechaVencimiento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaEmision\",\"type\":\"string\"},{\"name\":\"FechaPago\",\"type\":\"string\"},{\"name\":\"MontoPagado\",\"type\":\"float\"},{\"name\":\"MontoDiferencia\",\"type\":\"float\"},{\"name\":\"departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"provincia\",\"type\":\"string\"},{\"name\":\"distrito\",\"type\":\"string\"},{\"name\":\"zonal\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaRegistro\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"codcartera\",\"width\":200},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"cliente\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"cuenta\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Servicio\",\"dataIndex\":\"servicio\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Nombre\",\"dataIndex\":\"razonsocial\",\"width\":250,\"filterable\":true},";
                    columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":60},";
                    columns += "{\"text\":\"TipoDocumento\",\"dataIndex\":\"TipoDocumento\",\"width\":60},";
                    columns += "{\"text\":\"Deuda\",\"dataIndex\":\"MontoDeuda\",\"width\":100},";
                    columns += "{\"text\":\"Vencimiento\",\"dataIndex\":\"FechaVencimiento\",\"width\":100},";
                    columns += "{\"text\":\"Emisión\",\"dataIndex\":\"FechaEmision\",\"width\":100},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Monto Pagado\",\"dataIndex\":\"MontoPagado\",\"format\":\"0.00\",\"width\":100},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"dataIndex\":\"MontoDiferencia\",\"format\":\"0.00\",\"width\":100},";
                    columns += "{\"text\":\"Fecha de Pago\",\"dataIndex\":\"FechaPago\",\"width\":100},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"departamento\",\"width\":120,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"provincia\",\"width\":120},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"distrito\",\"width\":120},";
                    columns += "{\"text\":\"Zonal\",\"dataIndex\":\"zonal\",\"width\":60},";
                    columns += "{\"text\":\"Fecha de Registro\",\"dataIndex\":\"FechaRegistro\",\"width\":100}]";
                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     codcartera = m["codcartera"].ToString(),
                                     cliente = m["cliente"].ToString(),
                                     cuenta = m["cuenta"].ToString(),
                                     servicio = m["servicio"].ToString(),
                                     razonsocial = m["razonsocial"].ToString(),
                                     Moneda = m["Moneda"].ToString(),
                                     TipoDocumento = m["TipoDocumento"].ToString(),
                                     FechaVencimiento = (m["FechaVencimiento"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaVencimiento"]).ToString("yyyy/MM/dd")),
                                     FechaEmision = (m["FechaEmision"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaEmision"]).ToString("yyyy/MM/dd")),
                                     MontoDeuda = Convert.ToDecimal(m["MontoDeuda"]),
                                     MontoPagado = Convert.ToDecimal(m["MontoPagado"]),
                                     MontoDiferencia = Convert.ToDecimal(m["MontoDiferencia"]),
                                     FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("yyyy/MM/dd")),
                                     departamento = m["departamento"].ToString(),
                                     provincia = m["provincia"].ToString(),
                                     distrito = m["distrito"].ToString(),
                                     zonal = m["zonal"].ToString(),
                                     FechaRegistro = (m["FechaRegistro"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaRegistro"]).ToString("yyyy/MM/dd"))
                                 }).ToList<object>();
                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    return Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DataTable dt = Cartera.Instancia.ListarPagosAcumulados(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    string fields = "[{\"name\":\"codcartera\",\"type\":\"string\"},{\"name\":\"cliente\",\"type\":\"string\"},{\"name\":\"cuenta\",\"type\":\"string\"},";
                    fields += "{\"name\":\"servicio\",\"type\":\"string\"},{\"name\":\"razonsocial\",\"type\":\"string\"},{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DeudaTotal\",\"type\":\"float\"},{\"name\":\"PagoTotal\",\"type\":\"float\"},{\"name\":\"Saldo\",\"type\":\"float\"},";
                    fields += "{\"name\":\"departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"provincia\",\"type\":\"string\"},{\"name\":\"distrito\",\"type\":\"string\"},{\"name\":\"zonal\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"codcartera\",\"width\":200},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"cliente\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"cuenta\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Servicio\",\"dataIndex\":\"servicio\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Nombre\",\"dataIndex\":\"razonsocial\",\"width\":250,\"filterable\":true},";
                    columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":60},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Total\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.##\",\"width\":100},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago Total\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.##\",\"width\":100},";
                    //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.##\",\"width\":100},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"departamento\",\"width\":120,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"provincia\",\"width\":120},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"distrito\",\"width\":120},";
                    columns += "{\"text\":\"Zonal\",\"dataIndex\":\"zonal\",\"width\":60}]";
                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     codcartera = m["codcartera"].ToString(),
                                     cliente = m["cliente"].ToString(),
                                     cuenta = m["cuenta"].ToString(),
                                     servicio = m["servicio"].ToString(),
                                     razonsocial = m["razonsocial"].ToString(),
                                     Moneda = m["Moneda"].ToString(),
                                     DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                     PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                     Saldo = Convert.ToDecimal(m["Saldo"]),
                                     departamento = m["departamento"].ToString(),
                                     provincia = m["provincia"].ToString(),
                                     distrito = m["distrito"].ToString(),
                                     zonal = m["zonal"].ToString()
                                 }).ToList<object>();
                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    return Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
            }

            public JsonResult ListarServicioV2(short gestionCliente,int detalleCartera, int moroso, int cartera)
            {
                JsonResult respuestaJson = new JsonResult();
                if (gestionCliente == 1) 
                {
                    DataTable dt = Cartera.Instancia.ListarServicioV2(detalleCartera);

                    string fields = "[{\"name\":\"IDServicio\",\"type\":\"int\"},";
                    fields += "{\"name\":\"TipoDocumento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaEmision\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaVencimiento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MontoDeuda\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoPagado\",\"type\":\"number\"},";
                    fields += "{\"name\":\"FechaPago\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},{\"text\":\"IDServicio\",\"dataIndex\":\"IDServicio\",\"hidden\":true,\"hideable\":false},";
                    columns += "{\"text\":\"T. Documento\",\"dataIndex\":\"TipoDocumento\",\"width\":100,\"hideable\":false},";
                    columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"hideable\":false},";
                    columns += "{\"text\":\"Emisión\",\"dataIndex\":\"FechaEmision\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Vencimiento\",\"dataIndex\":\"FechaVencimiento\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":80,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"MontoDeuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Monto Pagado\",\"align\":\"right\",\"dataIndex\":\"MontoPagado\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false,\"renderer\": \"fnColorearPago\"},";
                    columns += "{\"text\":\"Ult. Fecha Pago\",\"dataIndex\":\"FechaPago\",\"width\":90,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     IDServicio = Convert.ToInt32(m["IDServicio"]),
                                     TipoDocumento = m["TipoDocumento"].ToString(),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     FechaEmision = Convert.ToDateTime(m["FechaEmision"]).ToString("yyyy/MM/dd"),
                                     FechaVencimiento = Convert.ToDateTime(m["FechaVencimiento"]).ToString("yyyy/MM/dd"),
                                     Moneda = m["Moneda"].ToString(),
                                     MontoDeuda = Convert.ToDecimal(m["MontoDeuda"]),
                                     FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("dd/MM/yyyy")),
                                     MontoPagado = Convert.ToDecimal(m["MontoPagado"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 2) 
                {
                    DataTable dt = Cartera.Instancia.ListarProductos(cartera, moroso);
                    string fields = "[{\"name\":\"NroProducto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NroCJ\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NroTarjeta\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Producto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"SubProducto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MotivoBloqueo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Tramo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DiasMora\",\"type\":\"string\"},";
                    fields += "{\"name\":\"IniFimProd\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Deuda\",\"type\":\"number\"},";
                    fields += "{\"name\":\"Actualizado\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    
                    columns += "{\"text\":\"NroCJ\",\"dataIndex\":\"NroCJ\", \"width\": 100,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"Deuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"D. Actual\",\"align\":\"right\",\"dataIndex\":\"Actualizado\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"dataIndex\":\"MontoPago\",\"align\":\"right\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false, \"renderer\": \"fnColorearPago\"},";
                    columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":50,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"D. Mora\",\"align\":\"right\",\"format\":\"0,000.##\",\"dataIndex\":\"DiasMora\",\"width\":60,\"hideable\":false},";
                    columns += "{\"text\":\"F. Venc.\",\"dataIndex\":\"IniFimProd\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Producto\",\"dataIndex\":\"Producto\",\"width\":110,\"hideable\":false},";
                    columns += "{\"text\":\"SubProducto\",\"dataIndex\":\"SubProducto\",\"width\":130,\"hideable\":false},";
                    columns += "{\"text\":\"N° Tarjeta\",\"dataIndex\":\"NroTarjeta\",\"width\":140,\"hideable\":false},";
                    columns += "{\"text\":\"Motivo de Bloqueo\",\"dataIndex\":\"MotivoBloqueo\",\"width\":130,\"hideable\":false},";
                    columns += "{\"text\":\"NroProducto\",\"dataIndex\":\"NroProducto\", \"width\": 100,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";
                    
                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     NroProducto = m["NroProducto"].ToString(),
                                     NroCJ = (m["NroCJ"] == DBNull.Value ? "" : m["NroCJ"].ToString()),
                                     NroTarjeta = (m["NroTarjeta"] == DBNull.Value ? "" : m["NroTarjeta"].ToString()),
                                     Producto = m["Producto"].ToString(),
                                     SubProducto = m["SubProducto"].ToString(),
                                     MotivoBloqueo = (m["MotivoBloqueo"] == DBNull.Value ? "" : m["MotivoBloqueo"].ToString()),
                                     Tramo = m["Tramo"].ToString(),
                                     IniFimProd = (m["IniFimProd"] == DBNull.Value ? "" : Convert.ToDateTime(m["IniFimProd"]).ToString("dd/MM/yyyy")),
                                     DiasMora = Convert.ToDecimal(m["DiasMora"]),
                                     Deuda = Convert.ToDecimal(m["Deuda"]),
                                     Actualizado = Convert.ToDecimal(m["Actualizado"]),
                                     MontoPago = Convert.ToDecimal(m["MontoPago"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarMorososEnCarteraV2(string cliente, short gestionCliente, string fechaInicio,string fechaFin, object[] zonal, object[] departamento, object[] tramo, object[] cluster)
            {
                JsonResult respuestaJson= new JsonResult();
                if (gestionCliente == 1)
                {
                    string xmlCluster = "<root>";
                    string xmlDpto = "<root>";
                    string xmlZonal = "<root>";
                    string xmlTramo = "<root>";

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (cluster != null)
                    {
                        for (int i = 0; i < cluster.Length; i++)
                        {
                            xmlCluster += "<cluster Cluster = '" + cluster[i].ToString() + "' />";
                        }
                    }
                    xmlCluster += "</root>";

                    if (tramo != null)
                    {
                        for (int i = 0; i < tramo.Length; i++)
                        {
                            xmlTramo += "<tramo Tramo = '" + tramo[i].ToString() + "' />";
                        }
                    }
                    xmlTramo += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarMorososEnCarteraV2(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo, xmlCluster);

                    string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Cuenta\",\"type\":\"string\"},{\"name\":\"Servicio\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"TipoTecnologia\",\"type\":\"string\"},{\"name\":\"DetalleCartera\",\"type\":\"int\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},{\"name\":\"PagoTotal\",\"type\":\"number\"},{\"name\":\"Saldo\",\"type\":\"number\"},";
                    fields += "{\"name\":\"Gestionado\",\"type\":\"bool\"},{\"name\":\"Contactado\",\"type\":\"bool\"},{\"name\":\"PromesaPago\",\"type\":\"bool\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                    columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                    columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"Cuenta\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Servicio\",\"dataIndex\":\"Servicio\",\"width\":90,\"filterable\":true},"; columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestionado\",\"dataIndex\":\"Gestionado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contactado\",\"dataIndex\":\"Contactado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Con Promesa\",\"dataIndex\":\"PromesaPago\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                    columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                    columns += "{\"text\":\"Tipo Tecnología\",\"dataIndex\":\"TipoTecnologia\",\"width\":110},";
                    columns += "{\"text\":\"DetalleCartera\",\"dataIndex\":\"DetalleCartera\",\"hideable\":false,\"hidden\":true},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     Cartera = Convert.ToInt32(m["Cartera"]),
                                     DCliente = m["DCliente"].ToString(),
                                     CodCliente = m["CodCliente"].ToString(),
                                     Cuenta = m["Cuenta"].ToString(),
                                     Servicio = m["Servicio"].ToString(),
                                     DGestionCliente = m["DGestionCliente"].ToString(),
                                     CodCartera = m["CodCartera"].ToString(),
                                     Zonal = m["Zonal"].ToString(),
                                     Departamento = m["Departamento"].ToString(),
                                     Provincia = m["Provincia"].ToString(),
                                     Distrito = m["Distrito"].ToString(),
                                     Cluster = m["Cluster"].ToString(),
                                     Tramo = m["Tramo"].ToString(),
                                     TipoTecnologia = m["TipoTecnologia"].ToString(),
                                     DetalleCartera = Convert.ToInt32(m["DetalleCartera"]),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     Moroso = Convert.ToInt32(m["Moroso"]),
                                     DMoroso = m["DMoroso"].ToString(),
                                     DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                     PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                     Saldo = Convert.ToDecimal(m["Saldo"]),
                                     Gestionado = Convert.ToBoolean(m["Gestionado"]),
                                     Contactado = Convert.ToBoolean(m["Contactado"]),
                                     PromesaPago = Convert.ToBoolean(m["PromesaPago"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson= Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 2) 
                {
                    string xmlDpto = "<root>";
                    string xmlTramo = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    if (tramo != null)
                    {
                        for (int i = 0; i < tramo.Length; i++)
                        {
                            xmlTramo += "<tramo Tramo = '" + tramo[i].ToString() + "' />";
                        }
                    }
                    xmlTramo += "</root>";

                    DataTable dt = Cartera.Instancia.ListarMorososEnCarteraIBK(cliente,gestionCliente,fechaInicio,xmlDpto, xmlTramo);

                    string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},{\"name\":\"Actualizado\",\"type\":\"number\"},{\"name\":\"PagoTotal\",\"type\":\"number\"},{\"name\":\"Saldo\",\"type\":\"number\"},";
                    fields += "{\"name\":\"GestionadoCall\",\"type\":\"bool\"},{\"name\":\"ContactadoCall\",\"type\":\"bool\"},{\"name\":\"PromesaPago\", type:\"date\"},{\"name\":\"Tramo\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":50},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                    columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"width\":80,\"filterable\":true},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                    columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\", \"align\": \"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Actualizado\",\"align\": \"right\",\"dataIndex\":\"Actualizado\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\": \"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\": \"right\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestión Call\",\"dataIndex\":\"GestionadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contacto Call\",\"dataIndex\":\"ContactadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                    columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                    columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110 },";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":130},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":130},";
                    //columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     Cartera = Convert.ToInt32(m["Cartera"]),
                                     DCliente = m["DCliente"].ToString(),
                                     CodCliente = m["CodCliente"].ToString(),
                                     DGestionCliente = m["DGestionCliente"].ToString(),
                                     CodCartera = m["CodCartera"].ToString(),
                                     Departamento = m["Departamento"].ToString(),
                                     Provincia = m["Provincia"].ToString(),
                                     Distrito = m["Distrito"].ToString(),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     Tramo = m["Tramo"].ToString(),
                                     Moroso = Convert.ToInt32(m["Moroso"]),
                                     DMoroso = m["DMoroso"].ToString(),
                                     DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                     Actualizado = Convert.ToDecimal(m["Actualizado"]),
                                     PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                     Saldo = Convert.ToDecimal(m["Saldo"]),
                                     GestionadoCall = Convert.ToBoolean(m["GestionadoCall"]),
                                     ContactadoCall = Convert.ToBoolean(m["ContactadoCall"]),
                                     PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson= Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarMorososEnCartera(string cliente, short gestionCliente, string fechaFin, string tramo, object[] cluster, object[] departamento)
            {
                string xmlCluster = "<root>";
                string xmlDpto = "<root>";

                if (cluster != null)
                {
                    for (int i = 0; i < cluster.Length; i++)
                    {
                        xmlCluster += "<cluster Cluster = '" + cluster[i].ToString() + "' />";
                    }
                }
                xmlCluster += "</root>";

                if (departamento != null)
                {
                    for (int i = 0; i < departamento.Length; i++)
                    {
                        xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                    }
                }
                xmlDpto += "</root>";

                DataTable dt = Cartera.Instancia.ListarMorososEnCartera(cliente, gestionCliente, fechaFin, tramo, xmlCluster, xmlDpto);

                string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                fields += "{\"name\":\"Cuenta\",\"type\":\"string\"},{\"name\":\"Servicio\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"TipoTecnologia\",\"type\":\"string\"},{\"name\":\"DetalleCartera\",\"type\":\"int\"},";
                fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                fields += "{\"name\":\"DeudaTotal\",\"type\":\"float\"},{\"name\":\"PagoTotal\",\"type\":\"float\"},{\"name\":\"Saldo\",\"type\":\"float\"},";
                fields += "{\"name\":\"Gestionado\",\"type\":\"bool\"},{\"name\":\"Contactado\",\"type\":\"bool\"},{\"name\":\"PromesaPago\",\"type\":\"bool\"}]";

                string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"Cuenta\",\"width\":90,\"filterable\":true},";
                columns += "{\"text\":\"Servicio\",\"dataIndex\":\"Servicio\",\"width\":90,\"filterable\":true},"; columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestionado\",\"dataIndex\":\"Gestionado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contactado\",\"dataIndex\":\"Contactado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Con Promesa\",\"dataIndex\":\"PromesaPago\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":false,\"hidden\":true},";
                columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                columns += "{\"text\":\"Tipo Tecnología\",\"dataIndex\":\"TipoTecnologia\",\"width\":110},";
                columns += "{\"text\":\"DetalleCartera\",\"dataIndex\":\"DetalleCartera\",\"hideable\":false,\"hidden\":true},";
                columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Cartera = Convert.ToInt32(m["Cartera"]),
                                 DCliente = m["DCliente"].ToString(),
                                 CodCliente = m["CodCliente"].ToString(),
                                 Cuenta = m["Cuenta"].ToString(),
                                 Servicio = m["Servicio"].ToString(),
                                 DGestionCliente = m["DGestionCliente"].ToString(),
                                 CodCartera = m["CodCartera"].ToString(),
                                 Zonal = m["Zonal"].ToString(),
                                 Departamento = m["Departamento"].ToString(),
                                 Provincia = m["Provincia"].ToString(),
                                 Distrito = m["Distrito"].ToString(),
                                 Cluster = m["Cluster"].ToString(),
                                 Tramo = m["Tramo"].ToString(),
                                 TipoTecnologia = m["TipoTecnologia"].ToString(),
                                 DetalleCartera = Convert.ToInt32(m["DetalleCartera"]),
                                 NumeroDocumento = m["NumeroDocumento"].ToString(),
                                 Moroso = Convert.ToInt32(m["Moroso"]),
                                 DMoroso = m["DMoroso"].ToString(),
                                 DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                 PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                 Saldo = Convert.ToDecimal(m["Saldo"]),
                                 Gestionado = Convert.ToBoolean(m["Gestionado"]),
                                 Contactado = Convert.ToBoolean(m["Contactado"]),
                                 PromesaPago = Convert.ToBoolean(m["PromesaPago"])
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarMorosos(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento)
            {
                JsonResult respuestaJson = new JsonResult();
                if (gestionCliente == 1) {
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarMorososGrid(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto);

                    string fields = "[{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Cuenta\",\"type\":\"string\"},{\"name\":\"Servicio\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"TipoTecnologia\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DeudaTotal\",\"type\":\"float\"},{\"name\":\"PagoTotal\",\"type\":\"float\"},{\"name\":\"Saldo\",\"type\":\"float\"},";
                    fields += "{\"name\":\"Gestionado\",\"type\":\"bool\"},{\"name\":\"Contactado\",\"type\":\"bool\"},{\"name\":\"PromesaPago\",\"type\":\"bool\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},";
                    columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                    columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"Cuenta\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Servicio\",\"dataIndex\":\"Servicio\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestionado\",\"dataIndex\":\"Gestionado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contactado\",\"dataIndex\":\"Contactado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Con Promesa\",\"dataIndex\":\"PromesaPago\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                    columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                    columns += "{\"text\":\"Tipo Tecnología\",\"dataIndex\":\"TipoTecnologia\",\"width\":110},";
                    columns += "{\"text\":\"DetalleCartera\",\"dataIndex\":\"DetalleCartera\",\"hideable\":false,\"hidden\":true},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     DCliente = m["DCliente"].ToString(),
                                     CodCliente = m["CodCliente"].ToString(),
                                     Cuenta = m["Cuenta"].ToString(),
                                     Servicio = m["Servicio"].ToString(),
                                     DGestionCliente = m["DGestionCliente"].ToString(),
                                     CodCartera = m["CodCartera"].ToString(),
                                     Zonal = m["Zonal"].ToString(),
                                     Departamento = m["Departamento"].ToString(),
                                     Provincia = m["Provincia"].ToString(),
                                     Distrito = m["Distrito"].ToString(),
                                     Cluster = m["Cluster"].ToString(),
                                     Tramo = m["Tramo"].ToString(),
                                     TipoTecnologia = m["TipoTecnologia"].ToString(),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     DMoroso = m["DMoroso"].ToString(),
                                     DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                     PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                     Saldo = Convert.ToDecimal(m["Saldo"]),
                                     Gestionado = Convert.ToBoolean(m["Gestionado"]),
                                     Contactado = Convert.ToBoolean(m["Contactado"]),
                                     PromesaPago = Convert.ToBoolean(m["PromesaPago"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }else if (gestionCliente==2)
                {
                    string xmlDpto = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarMorososGridIBK(cliente, gestionCliente, fechaInicio, xmlDpto);

                    string fields = "[{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DeudaTotal\",\"type\":\"float\"},{\"name\":\"PagoTotal\",\"type\":\"float\"},{\"name\":\"Saldo\",\"type\":\"float\"},";
                    fields += "{\"name\":\"Contactado\",\"type\":\"bool\"},{\"name\":\"PromesaPago\",\"type\":\"bool\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},";
                    columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                    columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                    columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contactado\",\"dataIndex\":\"Contactado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Con Promesa\",\"dataIndex\":\"PromesaPago\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                    columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                    columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                    columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                    columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     DCliente = m["DCliente"].ToString(),
                                     CodCliente = m["CodCliente"].ToString(),
                                     DGestionCliente = m["DGestionCliente"].ToString(),
                                     CodCartera = m["CodCartera"].ToString(),
                                     Departamento = m["Departamento"].ToString(),
                                     Provincia = m["Provincia"].ToString(),
                                     Distrito = m["Distrito"].ToString(),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     DMoroso = m["DMoroso"].ToString(),
                                     DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                     PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                     Saldo = Convert.ToDecimal(m["Saldo"]),
                                     Contactado = Convert.ToBoolean(m["Contactado"]),
                                     PromesaPago = Convert.ToBoolean(m["PromesaPago"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarDirecciones(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, string dpto) 
            {
                JsonResult respuestaJson= new JsonResult();
                if (gestionCliente == 1) { 
                
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    List<object> lista = Cartera.Instancia.ListarDirecciones(cliente,gestionCliente,fechaFin,xmlZonal,xmlDpto);
                    respuestaJson = Json(lista, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 2)
                {
                    List<object> lista = Cartera.Instancia.ListarDireccionesIBK(cliente, gestionCliente, fechaInicio, dpto);
                    respuestaJson = Json(lista, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarServicio(int detalleCartera)
            {
                List<object> lista = Cartera.Instancia.ListarServicio(detalleCartera);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarDetalleMoroso(int moroso)
            {
                List<object> lista = Cartera.Instancia.ListarDetalleMoroso(moroso);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarGestionMoroso(int detalleMoroso)
            {
                List<object> lista = Cartera.Instancia.ListarGestionMoroso(detalleMoroso);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public void ExportarGestion(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, DateTime fechaDesde, DateTime fechaHasta, bool mejorGestion)
            {
                if (gestionCliente == 1) {
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    DataTable dt = new DataTable();
                    if (!mejorGestion)
                    {
                        dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    }
                    else
                    {
                        dt = Cartera.Instancia.ListarMejorGestion(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    }
                    //DataTable dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=GestCable-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestCABLE").ToArray());
                    Response.End();
                }
                else if (gestionCliente == 2) {
                    string xmlDpto = "<root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    DataTable dt = new DataTable();
                    if (!mejorGestion)
                    {
                        dt = Cartera.Instancia.ListarGestionesIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                    }
                    else
                    {
                        dt = Cartera.Instancia.ListarMejorGestionIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                    }
                    //DataTable dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=GestVINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestVINTE").ToArray());
                    Response.End();
                }
            }

            public void ExportarPagos(string cliente, short gestionCliente, string fechaFin, object[] zonal, object[] departamento, DateTime fechaDesde, DateTime fechaHasta, bool acumulado)
            {
                string xmlZonal = "<root>";
                string xmlDpto = "<root>";

                if (zonal != null)
                {
                    for (int i = 0; i < zonal.Length; i++)
                    {
                        xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                    }
                }
                xmlZonal += "</root>";

                if (departamento != null)
                {
                    for (int i = 0; i < departamento.Length; i++)
                    {
                        xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                    }
                }
                xmlDpto += "</root>";
                DataTable dt = new DataTable();
                if (!acumulado)
                {
                    dt = Cartera.Instancia.ListarPagos(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                }
                else
                {
                    dt = Cartera.Instancia.ListarPagosAcumulados(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                }
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=PagosCABLE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.csv";
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(ConvertirTablaToExcel(dt, "Pagos").ToArray());
                Response.End();
            }

            public void ExportarListaMorosos(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento)
            {
                if (gestionCliente == 1) { 
                
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";
                    DataView dv = new DataView();

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    DataTable dt = Cartera.Instancia.ListarMorosos(cliente, gestionCliente, fechaFin,  xmlZonal, xmlDpto);
                    dv = dt.DefaultView;
                    dv.Sort = "Servicio";
                    DataTable dtFinal = JuntarDetalle(dv.ToTable());
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=CABLE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    Response.BinaryWrite(ConvertirTablaToExcel(dtFinal, "Morosos").ToArray());
                    Response.End();
                }
                else if (gestionCliente == 2) {
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";
                    DataView dv = new DataView();

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (departamento != null)
                    {
                        for (int i = 0; i < departamento.Length; i++)
                        {
                            xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    DataTable dt = Cartera.Instancia.ListarMorososIBK(cliente, gestionCliente, fechaInicio, xmlDpto);
                    dv = dt.DefaultView;
                    //dv.Sort = "Servicio";
                    //DataTable dtFinal = JuntarDetalle(dv.ToTable());
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=VINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".csv");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    Response.BinaryWrite(ConvertirTablaToExcel(dt, "Morosos").ToArray());
                    Response.End();
                }
            }

            public void ExportarMorosos(string cliente, short gestionCliente, string fechaFin, string tramo, object[] cluster, object[] departamento)
            {
                string xmlCluster = "<root>";
                string xmlDpto = "<root>";

                if (cluster != null)
                {
                    for (int i = 0; i < cluster.Length; i++)
                    {
                        xmlCluster += "<cluster Cluster = '" + cluster[i].ToString() + "' />";
                    }
                }
                xmlCluster += "</root>";

                if (departamento != null)
                {
                    for (int i = 0; i < departamento.Length; i++)
                    {
                        xmlDpto += "<departamento Departamento = '" + departamento[i].ToString() + "' />";
                    }
                }
                xmlDpto += "</root>";
                DataTable dt = Cartera.Instancia.ListarMorososEnCartera(cliente, gestionCliente, fechaFin, tramo, xmlCluster, xmlDpto);
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=morosos.csv");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.csv";
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(ConvertirTablaToExcel(dt, "Morosos").ToArray());
                Response.End();
            }

        #endregion

        #region Insertar

            public JsonResult InsUpdGestionMoroso(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    int? gestionMoroso = lista[0]["GestionMoroso"] as int?;
                    int cartera = Convert.ToInt32(lista[0]["Cartera"]);
                    int detalleMoroso = Convert.ToInt32(lista[0]["DetalleMoroso"]);
                    byte tipoGestion = Convert.ToByte(lista[0]["TipoGestion"]);
                    byte claseGestion = Convert.ToByte(lista[0]["ClaseGestion"]);
                    short dclaseGestion = Convert.ToInt16(lista[0]["DClaseGestion"]);
                    short trabajador = Convert.ToInt16(lista[0]["Trabajador"]);
                    DateTime fechaGestion = Convert.ToDateTime(lista[0]["FechaGestion"]);
                    DateTime? fechaPromesa = ( lista[0]["FechaPromesa"] == null ? new DateTime?() : Convert.ToDateTime(lista[0]["FechaPromesa"]));
                    decimal monto = Convert.ToDecimal(lista[0]["Monto"]);
                    string observacion = lista[0]["Observacion"].ToString();
                    int result = Cartera.Instancia.InsUpdGestionMoroso(gestionMoroso,cartera,detalleMoroso,tipoGestion,claseGestion,dclaseGestion,fechaGestion,fechaPromesa,monto,observacion,trabajador,Session["Login"].ToString());
                    return Json(new { success = "true", data = result.ToString() }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }


            public JsonResult InsUpdDetalleMoroso(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    int detalleMoroso = Convert.ToInt32(lista[0]["DetalleMoroso"]);
                    int moroso = Convert.ToInt32(lista[0]["Moroso"]);
                    byte tipoDetalle = Convert.ToByte(lista[0]["TipoDetalle"]);
                    string descripcion = lista[0]["Descripcion"].ToString();
                    string descripcionEstado = lista[0]["DescripcionEstado"].ToString();
                    byte tipoEstado = Convert.ToByte(lista[0]["TipoEstado"]);
                    bool editable = Convert.ToBoolean(lista[0]["Editable"]);

                    Cartera.Instancia.InsUpdDetalleMoroso(detalleMoroso, moroso, tipoDetalle, descripcion, descripcionEstado, tipoEstado, editable, Session["Login"].ToString());
                    return Json(new { success = "true", data = "1" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

            public JsonResult InsUpdSector(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    int detalleMoroso = Convert.ToInt32(lista[0]["DetalleMoroso"]);
                    string sector = lista[0]["Sector"].ToString();
                    if (sector.Length == 1) sector = "0" + sector;
                    Cartera.Instancia.InsUpdSector(detalleMoroso, sector);
                    return Json(new { success = "true", data = "1" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        [ValidateInput(false)]
            public JsonResult InsUpdSectores(object[] datos)
            {
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    List<Dictionary<string, object>> lista = js.Deserialize<List<Dictionary<string, object>>>(datos[0].ToString());

                    //string fechaFin, fechaInicio, zonal = "";
                    string fechaFin = "";
                    string zonal = "";
                    string fechaInicio = "";
                    string cliente = lista[0]["Cliente"].ToString();
                    int gestionCliente = Convert.ToInt32(lista[0]["GestionCliente"]);
                    if (gestionCliente == 1)
                    {
                        zonal = lista[0]["Zonal"].ToString();
                        fechaFin = lista[0]["FechaFin"].ToString();
                    }
                    else if (gestionCliente == 2) 
                    {
                        fechaInicio = lista[0]["FechaInicio"].ToString();
                    }
                    string departamento = lista[0]["Departamento"].ToString();
                    string provincia = lista[0]["Provincia"].ToString();
                    string distrito = lista[0]["Distrito"].ToString();
                    string sector = lista[0]["Sector"].ToString();
                    if (sector.Length == 1) sector = "0" + sector;

                    if (gestionCliente == 1) 
                    {
                        Cartera.Instancia.InsUpdSectores(cliente, gestionCliente, fechaFin, zonal, departamento, provincia, distrito, sector);
                    }
                    else if (gestionCliente == 2)
                    {
                        Cartera.Instancia.InsUpdSectoresIBK(cliente, gestionCliente, fechaInicio, departamento, provincia, distrito, sector);
                    }
                    
                    return Json(new { success = "true", data = "1" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }

        #endregion

        [HttpPost]
        public JsonResult CargarCartera(int gestionCliente, int tipo, HttpPostedFileBase file)
            {
                try
                {
                    if (gestionCliente == 2)
                    {
                        C1XLBook book = new C1XLBook();
                        book.Load(file.InputStream);
                        XLSheet sheet = book.Sheets[0];

                        string xml = string.Empty;
                        string xmlDetalle = string.Empty;
                        string valor = string.Empty;
                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");

                        for (int i = 1; i < sheet.Rows.Count; i++)
                        {
                            xml = "<cartera ";
                            xmlDetalle = "";
                            string distrito = "", provincia = "", departamento = "", direccion = "";
                            for (int j = 0; j < sheet.Columns.Count; j++)
                            {
                                if (sheet[0, j].Value != null)
                                {
                                    valor = ( sheet[i,j].Value == null ? "" : devuelveCadena(sheet[i,j].Value.ToString().Trim()) );
                                    xml += (sheet[0, j].Value.Equals("FechaAsignacion") ? "CodCartera = 'VINTE" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("FechaAsignacion") ? "FechaInicio = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' FechaFin = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("CU") ? "CodigoCliente = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NroProd") ? "NroProducto = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NroTC") ? "NroTarjeta = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Prod") ? "Producto = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("SubProd") ? "SubProducto = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("DescripMotivoBloqueo") ? "MotivoBloqueo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("TramoClienteReal Inicial") ? "Tramo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("IniDMProd") ? "IniDMProducto = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("IniDMCliente") ? "IniDMCliente = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("MinimoMN") ? "MinimoMN = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("MinimoME") ? "MinimoME = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("VencidoMN") ? "VencidoMN = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("VencidoME") ? "VencidoME = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("TotalVencido") ? "ToralVencido = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NOMBRE_EMPRESA") ? "NombreEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("RUC") ? "RucEmpresa = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DIRECCION") ? "Direccion = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Distrito") ? "Distrito = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Provincia") ? "Provincia = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Departamento") ? "Departamento = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("DIRECCION_EMPRESA") ? "DireccionEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("DISTRITO_EMPRESA") ? "DistritoEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("PROVINCIA_EMPRESA") ? "ProvinciaEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("DEP_EMPRESA") ? "DepartamentoEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("SITUACION LABORAL") ? "SituacionLaboral = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("ESTATAL?") ? "Estatal = '" + (valor.Equals("Si") ? Convert.ToString(true) : Convert.ToString(false)) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("RANGO_SUELDO") ? "RangoSueldo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("MONEDA") ? "Moneda = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("FEC-DESEMB") && valor.Length > 0 ? "FechaPrestamo = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("FEC-1ER-VENC") && valor.Length > 0 ? "FechaCuota = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("CANT CUOTAS") ? "NroCuotas = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("DIRECC-CLIENTE") ? "DireccionCliente = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("IMP-CUO-ORD") ? "MontoCuota = '" + ( valor.Length == 0 ? "0" : valor.Replace(",","")) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("FEC-PROX-VENC") && valor.Length > 0 ? "FechaProxVencimiento = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("CUOTAS PAGADAS") ? "CuotasPagadas = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("CUOTAS PENDIENTES") ? "CuotasPendientes = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("CUOTAS VENCIDAS") ? "CuotasVencidas = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("IMP-CANCELACION") ? "ImporteCancelacion = '" + ( valor.Length == 0 ? "0" : valor.Replace(",","")) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("DNI") ? "NumeroDocumento = '" + valor + "' TipoDocumento = 'DN' " : "");
                                    xml += (sheet[0, j].Value.Equals("Nombre") ? "RazonSocial = '" + valor + "' " : "");
                                    distrito = (distrito.Length == 0 ? (sheet[0, j].Value.Equals("Distrito") ? valor : "") : distrito);
                                    provincia = (provincia.Length == 0 ? (sheet[0, j].Value.Equals("Provincia") ? valor : "") : provincia);
                                    departamento = (departamento.Length == 0 ? (sheet[0, j].Value.Equals("Departamento") ? valor : "") : departamento);
                                    direccion = (direccion.Length == 0 ? (sheet[0, j].Value.Equals("Direccion") ? valor : "") : direccion);

                                    //detallemoroso
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0,sheet[0, j].Value.ToString().Length-2).Equals("TlfCasa") && valor.Length > 0 ? "<detalle Descripcion = '"+ valor +"' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfCelular") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfTrabajo") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfReferencia") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfDesconocido") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.Equals("Email") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '4' />" : "");
                                }
                            }
                            xmlDetalle += "<detalle Descripcion = '" + direccion + "' TipoDetalle = '2' />";
                            cadena.Append(xml +" >");
                            cadena.Append(xmlDetalle);
                            cadena.Append("</cartera>");
                        }
                        cadena.Append("</root>");
                        Cartera.Instancia.GuardarCarteraIBK(gestionCliente, cadena, Session["Login"].ToString());
                        return Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        MemoryStream ms = new MemoryStream();
                        file.InputStream.CopyTo(ms);
                        StreamReader r = new StreamReader(ms);

                        string xml;
                        string linea = "";
                        string direccion = "";
                        string[] valores;
                        int contador = 10;

                        r.BaseStream.Position = 0;

                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");
                        if (tipo == 1)
                        {
                            while (!r.EndOfStream)
                            {
                                linea = r.ReadLine();
                                valores = linea.Split(new char[] { '|' });

                                if (valores.Length != 71)
                                {
                                    throw new Exception("La estructura del archivo no es correcta.");
                                }
                                xml = "<cartera Cliente = '" + valores[0].Trim() + "' ";
                                xml += "Cuenta = '" + valores[1].Trim() + "' ";
                                xml += "RazonSocial = '" + devuelveCadena((valores[4] + " " + valores[5] + ", " + valores[6]).Trim()).Trim() + "' ";
                                xml += "TipoDocumento = '" + valores[7].Trim() + "' ";
                                if (valores[8].ToString().Trim() == "\0" || valores[8].ToString().Trim() == "")
                                {
                                    xml += "NumeroDocumento = 'X" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + contador.ToString() + "' ";
                                    contador++;
                                }
                                else
                                {
                                    xml += "NumeroDocumento = '" + valores[8] + "' ";
                                }
                                xml += "Telefono1 = '" + valores[9].Trim() + "' ";
                                xml += "Telefono2 = '" + valores[10].Trim() + "' ";
                                xml += "Telefono3 = '" + valores[11].Trim() + "' ";

                                direccion = (
                                            (valores[19].ToString().Trim() == "\0" ? "" : valores[19].ToString().Trim()) + "  " +
                                            (valores[20].ToString().Trim() == "\0" || valores[20].ToString().Trim() == "0" || valores[20].ToString().Trim() == "" ? "" : valores[20].ToString().Trim() + " ") +
                                            (valores[21].ToString().Trim() == "\0" || valores[21].ToString().Trim() == "0" || valores[21].ToString().Trim() == "" ? "" : valores[21].ToString().Trim() + " ") +
                                            (valores[22].ToString().Trim() == "\0" || valores[22].ToString().Trim() == "0" || valores[22].ToString().Trim() == "" ? "" : "Piso " + valores[22].ToString().Trim() + " ") +
                                            (valores[23].ToString().Trim() == "\0" || valores[23].ToString().Trim() == "0" || valores[23].ToString().Trim() == "" ? "" : "Int. " + valores[23].ToString().Trim() + " ") +
                                            (valores[24].ToString().Trim() == "\0" || valores[24].ToString().Trim() == "0" || valores[24].ToString().Trim() == "" ? "" : "MZ. " + valores[24].ToString().Trim() + " ") +
                                            (valores[25].ToString().Trim() == "\0" || valores[25].ToString().Trim() == "0" || valores[25].ToString().Trim() == "" ? "" : "LT. " + valores[25].ToString().Trim() + " ") +
                                            (valores[26].ToString().Trim() == "\0" || valores[26].ToString().Trim() == "0" || valores[26].ToString().Trim() == "" ? "" : ". " + valores[26].ToString().Trim() + " ") +
                                            (valores[27].ToString().Trim() == "\0" || valores[27].ToString().Trim() == "0" || valores[27].ToString().Trim() == "" ? "" : valores[27].ToString().Trim() + " ") +
                                            (valores[28].ToString().Trim() == "\0" || valores[28].ToString().Trim() == "0" || valores[28].ToString().Trim() == "" ? "" : "SECTOR " + valores[28].ToString().Trim() + " ") +
                                            (valores[29].ToString().Trim() == "\0" || valores[29].ToString().Trim() == "0" || valores[29].ToString().Trim() == "" ? "" : valores[29].ToString().Trim() + " ETAPA")
                                            );

                                xml += "Direccion = '" + devuelveCadena(direccion).Trim() + "' ";
                                xml += "Referencia = '" + devuelveCadena(valores[30].Trim()) + "' ";
                                xml += "Servicio = '" + valores[35].Trim() + "' ";
                                xml += "Zonal = '" + valores[12].Trim() + "' ";
                                xml += "Departamento = '" + valores[14].Trim() + "' ";
                                xml += "Provincia = '" + valores[16].Trim() + "' ";
                                xml += "Distrito = '" + valores[18].Trim() + "' ";
                                xml += "Cluster = '" + valores[60].Trim() + "' ";
                                xml += "Tramo = '" + valores[62].Trim() + "' ";
                                xml += "TipoTecnologia = '" + valores[40].Trim() + "' ";
                                xml += "DeudaTotal = '" + (Convert.ToDecimal(valores[57]) + Convert.ToDecimal(valores[58])).ToString().Trim() + "' ";
                                xml += "CodCartera = '" + valores[65].Trim() + "' ";
                                xml += "FechaInicio = '" + valores[67].Trim() + "' ";
                                xml += "FechaFin = '" + valores[68].Trim() + "' />";
                                cadena.Append(xml);
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCartera(gestionCliente, cadena, Session["Login"].ToString());
                            return Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            while (!r.EndOfStream)
                            {
                                linea = r.ReadLine();
                                valores = linea.Split(new char[] { '|' });

                                if (valores.Length != 16)
                                {
                                    throw new Exception("La estructura del archivo no es correcta.");
                                }
                                xml = "<cartera CodCartera = '" + valores[1].Trim() + "' ";
                                xml += "Cliente = '" + valores[2].Trim() + "' ";
                                xml += "Cuenta = '" + valores[3].Trim() + "' ";
                                xml += "Servicio = '" + valores[4].Trim() + "' ";
                                xml += "TipoDocumento = '" + valores[6].Trim() + "' ";
                                xml += "NumeroDocumento = '" + valores[7].Trim() + "' ";
                                xml += "FechaEmision = '" + Convert.ToDateTime(valores[8]).ToString("yyyyMMdd") + "' ";
                                xml += "FechaVencimiento = '" + Convert.ToDateTime(valores[9]).ToString("yyyyMMdd") + "' ";
                                xml += "MontoDeuda = '" + valores[14].Trim() + "' ";
                                xml += "Moneda = '" + valores[13].Trim() + "' ";
                                xml += "EstadoDocumento = '" + valores[15].Trim() + "' />";
                                cadena.Append(xml);
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarDetalleCartera(gestionCliente, cadena, Session["Login"].ToString());
                            
                            return Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            
        }

        private string devuelveCadena(string prmstrCadena)
        {
            try
            {
                char[] chars = HttpUtility.HtmlEncode(prmstrCadena).ToCharArray();
                StringBuilder result = new StringBuilder(prmstrCadena.Length + (int)(prmstrCadena.Length * 0.1));

                foreach (char c in chars)
                {
                    int value = Convert.ToInt32(c);
                    if (value > 127)
                        result.AppendFormat("&#{0};", value);
                    else
                        result.Append(c);
                }

                return result.ToString();
                //prmstrCadena = prmstrCadena.Replace( "&", "&amp;");
                //prmstrCadena = prmstrCadena.Replace( ">", "&gt;");
                //prmstrCadena = prmstrCadena.Replace("–", "&#8211;");
                //prmstrCadena = prmstrCadena.Replace( "<", "&lt;");
                //prmstrCadena = prmstrCadena.Replace( "''", "&quot;");
                //prmstrCadena = prmstrCadena.Replace( "'", "&apos;");
                //prmstrCadena = prmstrCadena.Replace( "Á", "&#193;");
                //prmstrCadena = prmstrCadena.Replace( "á", "&#225;");
                //prmstrCadena = prmstrCadena.Replace( "É", "&#201;");
                //prmstrCadena = prmstrCadena.Replace( "é", "&#233;");
                //prmstrCadena = prmstrCadena.Replace( "Í", "&#205;");
                //prmstrCadena = prmstrCadena.Replace( "í", "&#237;");
                //prmstrCadena = prmstrCadena.Replace( "Ó", "&#211;");
                //prmstrCadena = prmstrCadena.Replace( "ó", "&#243;");
                //prmstrCadena = prmstrCadena.Replace( "Ú", "&#218;");
                //prmstrCadena = prmstrCadena.Replace( "ú", "&#250;");
                //prmstrCadena = prmstrCadena.Replace( "Ñ", "&#209;");
                //prmstrCadena = prmstrCadena.Replace( "ñ", "&#241;");
                //prmstrCadena = prmstrCadena.Replace( "`", "&#96;");
                //prmstrCadena = prmstrCadena.Replace( "´", "&#180;");
                //prmstrCadena = prmstrCadena.Replace( "¨", "&#168;");
                //prmstrCadena = prmstrCadena.Replace( "ä", "&#228;");
                //prmstrCadena = prmstrCadena.Replace( "ë", "&#235;");
                //prmstrCadena = prmstrCadena.Replace( "ö", "&#246;");
                //prmstrCadena = prmstrCadena.Replace( "ü", "&#252;");
                //prmstrCadena = prmstrCadena.Replace( "à", "&#224;");
                //prmstrCadena = prmstrCadena.Replace( "è", "&#232;");
                //prmstrCadena = prmstrCadena.Replace( "ì", "&#236;");
                //prmstrCadena = prmstrCadena.Replace( "ò", "&#242;");
                //prmstrCadena = prmstrCadena.Replace( "ù", "&#249;");
                //prmstrCadena = prmstrCadena.Replace( "ç", "&#231;");
                //prmstrCadena = prmstrCadena.Replace( "Ç", "&#199;");
                //prmstrCadena = prmstrCadena.Replace( "º", "&#186;");
                //prmstrCadena = prmstrCadena.Replace( "Ø", "&#216;");
                //prmstrCadena = prmstrCadena.Replace( "È", "&#200;");
                //prmstrCadena = prmstrCadena.Replace( "À", "&#192;");
                //prmstrCadena = prmstrCadena.Replace( "½", "&#189;");
                //prmstrCadena = prmstrCadena.Replace( "°", "&#176;");
                //prmstrCadena = prmstrCadena.Replace( "ª", "&#170;");
                //prmstrCadena = prmstrCadena.Replace( "¡", "&#161;");
                //prmstrCadena = prmstrCadena.Replace( "!", "&#33;");
                //prmstrCadena = prmstrCadena.Replace( "/", "&#47;");
                //prmstrCadena = prmstrCadena.Replace( "¿", "&#191;");
                //prmstrCadena = prmstrCadena.Replace( "?", "&#63;");
                //prmstrCadena = prmstrCadena.Replace( "=", "&#61;");
                //prmstrCadena = prmstrCadena.Replace( "[", "&#91;");
                //prmstrCadena = prmstrCadena.Replace( "]", "&#93;");
                //prmstrCadena = prmstrCadena.Replace( "\\", "&#92;");
                //prmstrCadena = prmstrCadena.Replace( "^", "&#94;");
                //prmstrCadena = prmstrCadena.Replace( "_", "&#95;");
                //prmstrCadena = prmstrCadena.Replace( "{", "&#123;");
                //prmstrCadena = prmstrCadena.Replace( "|", "&#124;");
                //prmstrCadena = prmstrCadena.Replace( "}", "&#125;");
                //prmstrCadena = prmstrCadena.Replace( "~", "&#126;");
                //prmstrCadena = prmstrCadena.Replace( "¬", "&#172;");
                //prmstrCadena = prmstrCadena.Replace( "¦", "&#161;");
                //prmstrCadena = prmstrCadena.Replace( "¯", "&#175;");
                //prmstrCadena = prmstrCadena.Replace( "·", "&#183;");
                //prmstrCadena = prmstrCadena.Replace( "$", "&#36;");
                //prmstrCadena = prmstrCadena.Replace( "%", "&#37;");
                //prmstrCadena = prmstrCadena.Replace( "€", "&euro;");
                //prmstrCadena = prmstrCadena.Replace( "—", "&#8212;");
                //prmstrCadena = prmstrCadena.Replace( "™", "&#8482;");
                //prmstrCadena = prmstrCadena.Replace( "…", "&#8230;");
                //prmstrCadena = prmstrCadena.Replace( "‰", "&#8240;");
                //prmstrCadena = prmstrCadena.Replace( "•", "&#8226;");
                //prmstrCadena = prmstrCadena.Replace( "†", "&#8224;");
                //prmstrCadena = prmstrCadena.Replace("¶", "&#20;");
                //prmstrCadena = prmstrCadena.Replace("╚", "&#200;");
                //prmstrCadena = prmstrCadena.Replace("╬", "&#206;");
                //return prmstrCadena;
            }
            catch (Exception)
            {
                return prmstrCadena;
            }
        }

        public MemoryStream ConvertirTablaToExcel(DataTable dt, string nombreHoja)
        {
            int col = 0;
            int row = 1;
            C1XLBook book = new C1XLBook();
            book.Sheets.Clear();
            XLSheet sheet = book.Sheets.Add(nombreHoja);

            int c = 0;
            foreach (DataColumn item in dt.Columns)
            {
                XLCell cell = sheet[0, col + c];
                cell.Value = item.ColumnName;
                int r = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    cell = sheet[row + r, col + c];
                    if (item.DataType == typeof(DateTime))
                        //cell.Value = Convert.ToDateTime(dr[item.Ordinal]).ToString("dd/MM/yyyy");
                        cell.Value=dr[item.Ordinal]==DBNull.Value? "":Convert.ToDateTime(dr[item.Ordinal]).ToString("dd/MM/yyyy");
                    else
                        cell.Value = dr[item.Ordinal];
                    r++;
                }
                c++;
            }

            MemoryStream ms = new MemoryStream();
            book.Save(ms, FileFormat.Csv);

            return ms;
        }

        private DataTable JuntarDetalle(DataTable dt)
        {
            DataTable dtFinal = new DataTable();
            CrearTablaDetalleDeuda(dtFinal);
            int nroFilas = dt.Rows.Count;
            int contador = -1;
            int contadorFilas = 0;
            int totalColumnas = dtFinal.Columns.Count -1;

            foreach (DataRow dr in dt.Rows)
            {
                contadorFilas++;
                DataRow dr2 = dtFinal.NewRow();
                if (dtFinal.Rows.Count == 0)
                {
                    for (int i = 0; i <= totalColumnas; i++)
                    {
                        dr2[i] = dr[i];
                    }
                    dr2["Sector"]= "\'" + dr2["Sector"];
                    dtFinal.Rows.Add(dr2);
                    contador += 1;
                }
                else 
                {
                    if (dr["Servicio"].ToString() == dtFinal.Rows[contador]["Servicio"].ToString())
                    {
                        dtFinal.Rows[contador]["DetalleDeuda"] += "\n" + dr["DetalleDeuda"].ToString();
                    }
                    else
                    {
                        dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                        for (int i = 0; i <= totalColumnas; i++)
                        {
                            dr2[i] = dr[i];
                        }
                        dr2["Sector"] = "\'" + dr2["Sector"];
                        dtFinal.Rows.Add(dr2);
                        contador += 1;
                    }
                    if (contadorFilas == nroFilas)
                    {
                        dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                    }
                }
            }
            return dtFinal;
        }

        private void CrearTablaDetalleDeuda(DataTable dt) 
        {
            dt.Columns.Add("DCliente");//0
            dt.Columns.Add("DGestionCliente");//1
            dt.Columns.Add("CodCartera");//2
            dt.Columns.Add("Zonal");//3
            dt.Columns.Add("Departamento");//4
            dt.Columns.Add("Provincia");//5
            dt.Columns.Add("Distrito");//6
            dt.Columns.Add("Cluster");//7
            dt.Columns.Add("Tramo");//8
            dt.Columns.Add("TipoTecnologia");//9
            dt.Columns.Add("Cuenta");//10
            dt.Columns.Add("Servicio");//11
            dt.Columns.Add("CodCliente");//12
            dt.Columns.Add("NumeroDocumento");//13
            dt.Columns.Add("DMoroso");//14
            dt.Columns.Add("Sector");//15
            dt.Columns.Add("DireccionBase");//16
            dt.Columns.Add("DireccionUbicada");//17
            dt.Columns.Add("DireccionPorVerificar");//18
            dt.Columns.Add("DeudaTotal");//19
            dt.Columns.Add("PagoTotal");//20
            dt.Columns.Add("Saldo");//21
            dt.Columns.Add("MejorIncidencia");//22
            dt.Columns.Add("GestionadoCall");//23
            dt.Columns.Add("GestionadoCampo");//24
            dt.Columns.Add("ContactadoCall");//25
            dt.Columns.Add("ContactadoCampo");//26
            dt.Columns.Add("PromesaPago");//27
            dt.Columns.Add("DetalleDeuda");//28
        }
    }
}
