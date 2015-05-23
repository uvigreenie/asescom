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
using RJ.Areas.Common.Models;
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
                if (gestionCliente == 1 || gestionCliente == 4 || gestionCliente == 6) {
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
                }
                else if (gestionCliente == 2 || gestionCliente == 5)
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

                if (gestionCliente == 1 || gestionCliente == 4 || gestionCliente ==6) {
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

            public JsonResult ListarProductoxZonalBBVA(int gestionCliente, string fechaInicio, object[] zonales)
            {
                JsonResult respuestaJson = new JsonResult();

                if (gestionCliente == 7 || gestionCliente == 8)
                {
                    string xmlZonal = "<root>";

                    if (zonales != null)
                    {
                        for (int i = 0; i < zonales.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonales[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    DataTable dt = Cartera.Instancia.ListarProductoxZonalBBVA(gestionCliente, fechaInicio, xmlZonal);

                    string fields = "[{\"name\":\"Codigo\",\"type\":\"string\"}, {\"name\":\"Producto\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new { 
                                     Codigo = c["Codigo"].ToString(),
                                     Producto = c["Producto"].ToString() 
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarTramoxProductoBBVA(int gestionCliente, string fechaInicio, object[] zonales, object[] productos)
            {
                JsonResult respuestaJson = new JsonResult();

                if (gestionCliente == 7 || gestionCliente == 8)
                {
                    string xmlZonal = "<root>";
                    string xmlProducto = "<root>";

                    if (zonales != null)
                    {
                        for (int i = 0; i < zonales.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonales[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";

                    if (productos != null)
                    {
                        for (int i = 0; i < productos.Length; i++)
                        {
                            xmlProducto += "<producto Producto ='"+ productos[i].ToString()+"' />";
                        }
                    }
                    xmlProducto += "</root>";

                    DataTable dt = Cartera.Instancia.ListarTramoxProductoBBVA(gestionCliente, fechaInicio, xmlZonal, xmlProducto);

                    string fields = "[{\"name\":\"Tramo\",\"type\":\"string\"}]";

                    var lista = (from c in dt.AsEnumerable()
                                 select new
                                 {
                                     Tramo = c["Tramo"].ToString()
                                 }).ToList<object>();

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
            if (gestionCliente == 1 || gestionCliente == 4) 
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
            else if (gestionCliente == 2 || gestionCliente == 5)
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
            if (gestionCliente == 1 || gestionCliente == 4) {
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
            else if (gestionCliente == 2 || gestionCliente == 5) {
                DataTable dt = Cartera.Instancia.ListarDistritoxProvIBK(gestionCliente, fechaInicio, dpto, provincia);

                string fields = "[{\"name\":\"Distrito\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Distrito = c["Distrito"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }
            return respuestaJson;
        }

            public JsonResult ListarGestiones(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, DateTime fechaDesde, DateTime fechaHasta, bool mejorGestion, bool promesa)
            {
                JsonResult respuestaJson = new JsonResult();

                if (gestionCliente == 1 || gestionCliente == 4 || gestionCliente == 6)
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
                else if (gestionCliente == 2 || gestionCliente == 5)
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
                        if (!promesa)
                        {
                            dt = Cartera.Instancia.ListarGestionesGridIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                        }
                        else
                        {
                            dt = Cartera.Instancia.ListarPromesasGridIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                        }

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

            public JsonResult ListarControlGestionXTrabajador()
            {
                JsonResult respuestaJson = new JsonResult();
                DataTable dt = Cartera.Instancia.ListarControlGestionXTrabajador(Session["Login"].ToString());

                string fields = "[{\"name\":\"TipoGestion\",\"type\":\"string\"},";
                //fields += "{\"name\":\"Tasa\",\"type\":\"string\"},";
                //fields += "{\"name\":\"Descuento\",\"type\":\"number\"},";
                fields += "{\"name\":\"NroGestiones\",\"type\":\"number\"}]";

                string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                //columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\", \"text\":\"Capital Inicial\",\"dataIndex\":\"CapitalProductoInicial\",\"width\":100,\"format\":\"0,000.00\"},";
                columns += "{\"align\":\"left\", \"text\":\"Tipo\",\"dataIndex\":\"TipoGestion\",\"width\":100,\"hideable\":false},";
                //columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\",\"text\":\"Descuento\",\"dataIndex\":\"Descuento\",\"width\":100,\"hideable\":false,\"format\":\"0,000.00\"},";
                columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\",\"text\":\"Clientes\",\"dataIndex\":\"NroGestiones\",\"width\":100,\"hideable\":false}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 //CapitalProductoInicial = Convert.ToDecimal(m["CapitalProductoInicial"]),
                                 TipoGestion = m["TipoGestion"].ToString(),
                                 //Descuento = Convert.ToDecimal(m["Descuento"]),
                                 NroGestiones = Convert.ToDecimal(m["NroGestiones"])
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);

                return respuestaJson;
            }

            public JsonResult ListarCampañaXProducto(int detalleCarteraBanco) 
            {
                JsonResult respuestaJson = new JsonResult();
                DataTable dt = Cartera.Instancia.ListarCampañaXProducto(detalleCarteraBanco);
                
                string fields = "[{\"name\":\"CapitalProductoInicial\",\"type\":\"number\"},";
                fields += "{\"name\":\"Tasa\",\"type\":\"string\"},";
                fields += "{\"name\":\"Descuento\",\"type\":\"number\"},";
                fields += "{\"name\":\"Monto\",\"type\":\"number\"}]";

                string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\", \"text\":\"Capital Inicial\",\"dataIndex\":\"CapitalProductoInicial\",\"width\":100,\"format\":\"0,000.00\"},";
                columns += "{\"align\":\"right\", \"text\":\"Tasa\",\"dataIndex\":\"Tasa\",\"width\":100,\"hideable\":false},";
                columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\",\"text\":\"Descuento\",\"dataIndex\":\"Descuento\",\"width\":100,\"hideable\":false,\"format\":\"0,000.00\"},";
                columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\",\"text\":\"Monto a pagar\",\"dataIndex\":\"Monto\",\"width\":100,\"hideable\":false,\"format\":\"0,000.00\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 CapitalProductoInicial = Convert.ToDecimal(m["CapitalProductoInicial"]),
                                 Tasa = m["Tasa"].ToString(),
                                 Descuento = Convert.ToDecimal(m["Descuento"]),
                                 Monto = Convert.ToDecimal(m["Monto"])
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);

                return respuestaJson;
            }

            public JsonResult ListarPagoXProducto(short gestionCliente, int detalleCarteraBanco, int idservicio)
            {
                JsonResult respuestaJson = new JsonResult();
                if (gestionCliente == 1) {
                    DataTable dt = Cartera.Instancia.ListarPagoXServicio(idservicio);

                    string fields = "[{\"name\":\"IDServicio\",\"type\":\"int\"},";
                    fields += "{\"name\":\"FechaPago\",\"type\":\"date\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"},";
                    fields += "{\"name\":\"FechaRegistro\",\"type\":\"date\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    columns += "{\"text\":\"IDServicio\",\"dataIndex\":\"IDServicio\",\"width\":100,\"hideable\":false, hidden: true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\", \"text\":\"Monto Pagado\",\"dataIndex\":\"MontoPago\",\"width\":100,\"hideable\":false,\"format\":\"0,000.00\"},";
                    columns += "{\"xtype\":\"datecolumn\",\"align\":\"right\",\"text\":\"Fecha de Pago\",\"dataIndex\":\"FechaPago\",\"width\":110,\"hideable\":false, \"format\":\"d/m/Y\"},";
                    columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fecha de Registro\",\"dataIndex\":\"FechaRegistro\",\"width\":110,hidden: true, \"hideable\":false, \"format\":\"d/m/Y\"}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     IDServicio = Convert.ToInt32(m["IDServicio"]),
                                     MontoPago = Convert.ToDecimal(m["MontoPagado"]),
                                     FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("yyyy/MM/dd")),
                                     FechaRegistro = (m["FechaRegistro"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaRegistro"]).ToString("yyyy/MM/dd"))
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);

                }
                else if (gestionCliente == 2) {
                    DataTable dt = Cartera.Instancia.ListarPagoXProducto(detalleCarteraBanco);

                    string fields = "[{\"name\":\"DetalleCarteraBanco\",\"type\":\"int\"},";
                    fields += "{\"name\":\"FechaPago\",\"type\":\"date\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"},";
                    fields += "{\"name\":\"FechaRegistro\",\"type\":\"date\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    columns += "{\"text\":\"DCB\",\"dataIndex\":\"DetalleCarteraBanco\",\"width\":100,\"hideable\":false, hidden: true},";
                    columns += "{\"xtype\":\"numbercolumn\",\"align\":\"right\", \"text\":\"Monto Pagado\",\"dataIndex\":\"MontoPago\",\"width\":100,\"hideable\":false,\"format\":\"0,000.00\"},";
                    columns += "{\"xtype\":\"datecolumn\",\"align\":\"right\",\"text\":\"Fecha de Pago\",\"dataIndex\":\"FechaPago\",\"width\":110,\"hideable\":false, \"format\":\"d/m/Y\"},";
                    columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fecha de Registro\",\"dataIndex\":\"FechaRegistro\",\"width\":110,hidden: true, \"hideable\":false, \"format\":\"d/m/Y\"}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     DetalleCarteraBanco = Convert.ToInt32(m["DetalleCarteraBanco"]),
                                     MontoPago = Convert.ToDecimal(m["MontoPago"]),
                                     FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("yyyy/MM/dd")),
                                     FechaRegistro = (m["FechaRegistro"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaRegistro"]).ToString("yyyy/MM/dd"))
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarRazonNoPago(short gestionCliente) 
            {
                JsonResult respuestaJson = new JsonResult();

                DataTable dt = Cartera.Instancia.ListarRazonNoPago(gestionCliente);

                string fields = "[{\"name\":\"Razon\",\"type\":\"int\"},{\"name\":\"DRazon\",\"type\":\"string\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Razon = Convert.ToInt16(m["RazonNoPago"]),
                                 DRazon = m["DRazonNoPago"].ToString()
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);

                return respuestaJson;
            }

            public JsonResult ListarSustento(short gestionCliente)
            {
                JsonResult respuestaJson = new JsonResult();
                return respuestaJson;
            }

            public JsonResult ListarServicioV2(short gestionCliente,int detalleCartera, int detalleCarteraFija, int detalleCarteraMovil, int moroso, int cartera)
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
                    fields += "{\"name\":\"Campaña\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Producto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"SubProducto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MotivoBloqueo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Tramo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DiasMora\",\"type\":\"string\"},";
                    fields += "{\"name\":\"IniFimProd\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Deuda\",\"type\":\"number\"},";
                    fields += "{\"name\":\"Actualizado\",\"type\":\"number\"},";
                    fields += "{\"name\":\"DetalleCarteraBanco\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    columns += "{\"text\":\"DCB\",\"dataIndex\":\"DetalleCarteraBanco\", hidden: true},";
                    columns += "{\"text\":\"NroCJ\",\"dataIndex\":\"NroCJ\", \"width\": 100,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"Deuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"D. Actual\",\"align\":\"right\",\"dataIndex\":\"Actualizado\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"Campaña\",\"dataIndex\":\"Campaña\", \"width\": 90,\"hideable\":false,\"align\":\"right\",  \"renderer\": \"fnColorearPorcentajeTexto\"},";
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
                                     MontoPago = Convert.ToDecimal(m["MontoPago"]),
                                     Campaña = (m["Campaña"] == DBNull.Value ? "" : m["Campaña"].ToString()),
                                     DetalleCarteraBanco = Convert.ToInt32(m["DetalleCarteraBanco"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }else if(gestionCliente == 4)
                {
                    DataTable dt = Cartera.Instancia.ListarServicioFija(detalleCarteraFija);

                    string fields = "[{\"name\":\"IDServicioFija\",\"type\":\"int\"},";
                    fields += "{\"name\":\"TipoDocumento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaEmision\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaVencimiento\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MontoDeuda\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoExigible\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"},";
                    fields += "{\"name\":\"FechaPago\",\"type\":\"string\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},{\"text\":\"IDServicioFija\",\"dataIndex\":\"IDServicioFija\",\"hidden\":true,\"hideable\":false},";
                    
                    columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":120,\"hideable\":false},";
                    columns += "{\"text\":\"Emisión\",\"dataIndex\":\"FechaEmision\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Vencimiento\",\"dataIndex\":\"FechaVencimiento\",\"width\":90,\"hideable\":false},";
                    //columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":80,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"D. Total\",\"align\":\"right\",\"dataIndex\":\"MontoDeuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"align\":\"right\",\"dataIndex\":\"MontoExigible\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false,\"renderer\": \"fnColorearPago\"},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"MontoPago\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"Ult. Fecha Pago\",\"dataIndex\":\"FechaPago\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"T. Documento\",\"dataIndex\":\"TipoDocumento\",\"width\":120,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     IDServicioFija = Convert.ToInt32(m["IDServicioFija"]),
                                     TipoDocumento = m["TipoDocumento"].ToString(),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     FechaEmision = Convert.ToDateTime(m["FechaEmision"]).ToString("yyyy/MM/dd"),
                                     FechaVencimiento = Convert.ToDateTime(m["FechaVencimiento"]).ToString("yyyy/MM/dd"),
                                     //Moneda = m["Moneda"].ToString(),
                                     MontoDeuda = Convert.ToDecimal(m["MontoDeuda"]),
                                     MontoPago = Convert.ToDecimal(m["montopagado"]),
                                     FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("dd/MM/yyyy")),
                                     MontoExigible = Convert.ToDecimal(m["MontoExigible"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 5)
                {
                    DataTable dt = Cartera.Instancia.ListarProductosRecovery(cartera, moroso);
                    string fields = "[{\"name\":\"NroProdOrig\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NroCJ\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"NroTarjeta\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"Campaña\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Producto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"SubProducto\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"MotivoBloqueo\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"Tramo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DiasMora\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"IniFimProd\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                    fields += "{\"name\":\"SaldoCapital\",\"type\":\"number\"},";

                    fields += "{\"name\":\"SaldoTransar\",\"type\":\"number\"},";
                    fields += "{\"name\":\"ValorCuota\",\"type\":\"number\"},";
                    fields += "{\"name\":\"TotalCuotas\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CuotaBalloon\",\"type\":\"number\"},";
                    fields += "{\"name\":\"AñoCastigo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Campaña\",\"type\":\"string\"},";

                    fields += "{\"name\":\"ImporteCancelacion\",\"type\":\"number\"},";
                    fields += "{\"name\":\"DetalleCarteraBanco\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    columns += "{\"text\":\"DCB\",\"dataIndex\":\"DetalleCarteraBanco\", hidden: true},";
                    columns += "{\"text\":\"NroCJ\",\"dataIndex\":\"NroCJ\", \"width\": 100,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"SaldoTransar\",\"align\":\"right\",\"dataIndex\":\"SaldoTransar\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"ValorCuota\",\"align\":\"right\",\"dataIndex\":\"ValorCuota\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"Cuotas\",\"align\":\"right\",\"dataIndex\":\"TotalCuotas\", \"width\": 70,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"C. Balloon\",\"align\":\"right\",\"dataIndex\":\"CuotaBalloon\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"AñoCastigo\",\"align\":\"right\",\"dataIndex\":\"AñoCastigo\", \"width\": 100,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"SaldoCapital\",\"align\":\"right\",\"dataIndex\":\"SaldoCapital\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"DeudaTotal\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Imp. Cancelacion\",\"align\":\"right\",\"dataIndex\":\"ImporteCancelacion\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"Campaña\",\"dataIndex\":\"Campaña\", \"width\": 90,\"hideable\":false,\"align\":\"right\",  \"renderer\": \"fnColorearPorcentajeTexto\"},";
                    //columns += "{\"text\":\"Campaña\",\"dataIndex\":\"Campaña\", \"width\": 90,\"hideable\":false,\"align\":\"right\",  \"renderer\": \"fnColorearPorcentajeTexto\"},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"dataIndex\":\"MontoPago\",\"align\":\"right\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false, \"renderer\": \"fnColorearPago\"},";
                    //columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":50,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"D. Mora\",\"align\":\"right\",\"format\":\"0,000.##\",\"dataIndex\":\"DiasMora\",\"width\":60,\"hideable\":false},";
                    //columns += "{\"text\":\"F. Venc.\",\"dataIndex\":\"IniFimProd\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Producto\",\"dataIndex\":\"Producto\",\"width\":110,\"hideable\":false},";
                    columns += "{\"text\":\"SubProducto\",\"dataIndex\":\"SubProducto\",\"width\":130,\"hideable\":false},";
                    //columns += "{\"text\":\"N° Tarjeta\",\"dataIndex\":\"NroTarjeta\",\"width\":140,\"hideable\":false},";
                    //columns += "{\"text\":\"Motivo de Bloqueo\",\"dataIndex\":\"MotivoBloqueo\",\"width\":130,\"hideable\":false},";
                    columns += "{\"text\":\"NroProdOrig\",\"dataIndex\":\"NroProdOrig\", \"width\": 100,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     NroProdOrig = m["NroProdOrig"].ToString(),
                                     NroCJ = (m["NroCJ"] == DBNull.Value ? "" : m["NroCJ"].ToString()),
                                     //NroTarjeta = (m["NroTarjeta"] == DBNull.Value ? "" : m["NroTarjeta"].ToString()),
                                     Producto = m["Producto"].ToString(),
                                     SubProducto = m["SubProducto"].ToString(),
                                     //MotivoBloqueo = (m["MotivoBloqueo"] == DBNull.Value ? "" : m["MotivoBloqueo"].ToString()),
                                     //Tramo = m["Tramo"].ToString(),
                                     //IniFimProd = (m["IniFimProd"] == DBNull.Value ? "" : Convert.ToDateTime(m["IniFimProd"]).ToString("dd/MM/yyyy")),
                                     DiasMora = Convert.ToDecimal(m["DiasMora"]),
                                     DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                     SaldoCapital = Convert.ToDecimal(m["SaldoCapital"]),
                                     SaldoTransar = Convert.ToDecimal(m["SaldoTransar"]),
                                     ValorCuota = Convert.ToDecimal(m["ValorCuota"]),
                                     TotalCuotas = Convert.ToDecimal(m["TotalCuotas"]),
                                     CuotaBalloon = Convert.ToDecimal(m["CuotaBalloon"]),
                                     AñoCastigo = Convert.ToDecimal(m["AñoCastigo"]),
                                     ImporteCancelacion = Convert.ToDecimal(m["ImporteCancelacion"]),
                                     MontoPago = Convert.ToDecimal(m["MontoPago"]),
                                     Campaña = (m["Campaña"] == DBNull.Value ? "" : m["Campaña"].ToString()),
                                     //Campaña = (m["Campaña"] == DBNull.Value ? "" : m["Campaña"].ToString()),
                                     DetalleCarteraBanco = Convert.ToInt32(m["DetalleCarteraBanco"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 6)
                {
                    DataTable dt = Cartera.Instancia.ListarServicioMovil(detalleCarteraMovil);

                    string fields = "[{\"name\":\"IDServicioMovil\",\"type\":\"int\"},";
                    fields += "{\"name\":\"TipoDocumento\",\"type\":\"string\"},";
                    fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"FechaEmision\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaVencimiento\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MontoDeuda\",\"type\":\"number\"},";
                    //fields += "{\"name\":\"MontoExigible\",\"type\":\"number\"},";
                    //fields += "{\"name\":\"MontoPago\",\"type\":\"number\"},";
                    fields += "{\"name\":\"MontoPago\",\"type\":\"number\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},{\"text\":\"IDServicioMovil\",\"dataIndex\":\"IDServicioMovil\",\"hidden\":true,\"hideable\":false},";

                    columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":120,\"hideable\":false},";
                    //columns += "{\"text\":\"Emisión\",\"dataIndex\":\"FechaEmision\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Vencimiento\",\"dataIndex\":\"FechaVencimiento\",\"width\":90,\"hideable\":false},";
                    //columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":80,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"MontoDeuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"align\":\"right\",\"dataIndex\":\"MontoExigible\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false,\"renderer\": \"fnColorearPago\"},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"MontoPago\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false, \"renderer\": \"fnColorearPago\"},";
                    //columns += "{\"text\":\"Ult. Fecha Pago\",\"dataIndex\":\"FechaPago\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"T. Documento\",\"dataIndex\":\"TipoDocumento\",\"width\":120,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     IDServicioFija = Convert.ToInt32(m["IDServicioMovil"]),
                                     TipoDocumento = m["TipoDocumento"].ToString(),
                                     NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     //FechaEmision = Convert.ToDateTime(m["FechaEmision"]).ToString("yyyy/MM/dd"),
                                     FechaVencimiento = Convert.ToDateTime(m["FechaVencimiento"]).ToString("yyyy/MM/dd"),
                                     //Moneda = m["Moneda"].ToString(),
                                     MontoDeuda = Convert.ToDecimal(m["MontoDeuda"]),
                                     MontoPago = Convert.ToDecimal(m["Pago"])
                                     //FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("dd/MM/yyyy")),
                                     //MontoExigible = Convert.ToDecimal(m["MontoExigible"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 7)
                {
                    DataTable dt = Cartera.Instancia.ListarProductosBBVA(gestionCliente, cartera, moroso);

                    string fields = "[{\"name\":\"DetalleCarteraBBVA\",\"type\":\"int\"},";
                    fields += "{\"name\":\"NroProducto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Producto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"SubProducto\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"FechaEmision\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaMora\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DiasMora\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Tramo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Oficina\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodigoOficina\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MontoDeuda\",\"type\":\"number\"}]";
                    //fields += "{\"name\":\"MontoExigible\",\"type\":\"number\"},";
                    //fields += "{\"name\":\"MontoPago\",\"type\":\"number\"},";
                    //fields += "{\"name\":\"MontoPago\",\"type\":\"number\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    columns += "{\"text\":\"DetalleCarteraBBVA\",\"dataIndex\":\"DetalleCarteraBBVA\",\"hidden\":true,\"hideable\":false},";
                    columns += "{\"text\":\"NroProducto\",\"dataIndex\":\"NroProducto\",\"width\":150,\"hideable\":false},";
                    columns += "{\"text\":\"Producto\",\"dataIndex\":\"Producto\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"SubProducto\",\"dataIndex\":\"SubProducto\",\"width\":140,\"hideable\":false},";
                    //columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":120,\"hideable\":false},";
                    //columns += "{\"text\":\"Emisión\",\"dataIndex\":\"FechaEmision\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":80,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"MontoDeuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"DiasMora\",\"dataIndex\":\"DiasMora\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Vencimiento\",\"dataIndex\":\"FechaMora\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Oficina\",\"dataIndex\":\"Oficina\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Cod. Ofic.\",\"dataIndex\":\"CodigoOficina\",\"width\":90,\"hideable\":false},";
                    //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"align\":\"right\",\"dataIndex\":\"MontoExigible\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false,\"renderer\": \"fnColorearPago\"},";
                    //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"MontoPago\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false, \"renderer\": \"fnColorearPago\"},";
                    //columns += "{\"text\":\"Ult. Fecha Pago\",\"dataIndex\":\"FechaPago\",\"width\":90,\"hideable\":false},";
                    //columns += "{\"text\":\"T. Documento\",\"dataIndex\":\"TipoDocumento\",\"width\":120,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     DetalleCarteraBBVA = Convert.ToInt32(m["DetalleCarteraBBVA"]),
                                     //TipoDocumento = m["TipoDocumento"].ToString(),
                                     //NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     //FechaEmision = Convert.ToDateTime(m["FechaEmision"]).ToString("yyyy/MM/dd"),
                                     NroProducto = m["NroProducto"].ToString(),
                                     Producto = m["Producto"].ToString(),
                                     SubProducto = m["SubProducto"].ToString(),
                                     DiasMora = m["DiasMora"].ToString(),
                                     Tramo = m["Tramo"].ToString(),
                                     Oficina = m["Oficina"].ToString(),
                                     CodigoOficina = m["CodigoOficina"].ToString(),
                                     FechaMora = Convert.ToDateTime(m["FechaMora"]).ToString("yyyy/MM/dd"),
                                     Moneda = m["Moneda"].ToString(),
                                     MontoDeuda = Convert.ToDecimal(m["MontoDeuda"])
                                     //MontoPago = Convert.ToDecimal(m["Pago"])
                                     //FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("dd/MM/yyyy")),
                                     //MontoExigible = Convert.ToDecimal(m["MontoExigible"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                else if ( gestionCliente == 8)
                {
                    DataTable dt = Cartera.Instancia.ListarProductosBBVA(gestionCliente, cartera, moroso);

                    string fields = "[{\"name\":\"DetalleCarteraBBVA\",\"type\":\"int\"},";
                    fields += "{\"name\":\"NroProducto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Producto\",\"type\":\"string\"},";
                    fields += "{\"name\":\"SubProducto\",\"type\":\"string\"},";
                    //fields += "{\"name\":\"FechaEmision\",\"type\":\"string\"},";
                    fields += "{\"name\":\"FechaMora\",\"type\":\"string\"},";
                    fields += "{\"name\":\"DiasMora\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Tramo\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                    fields += "{\"name\":\"Oficina\",\"type\":\"string\"},";
                    fields += "{\"name\":\"CodigoOficina\",\"type\":\"string\"},";
                    fields += "{\"name\":\"MontoDeuda\",\"type\":\"number\"}]";
                    //fields += "{\"name\":\"MontoExigible\",\"type\":\"number\"},";
                    //fields += "{\"name\":\"MontoPago\",\"type\":\"number\"},";
                    //fields += "{\"name\":\"MontoPago\",\"type\":\"number\"}]";

                    string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":45, hidden: true},";
                    columns += "{\"text\":\"DetalleCarteraBBVA\",\"dataIndex\":\"DetalleCarteraBBVA\",\"hidden\":true,\"hideable\":false},";
                    columns += "{\"text\":\"NroProducto\",\"dataIndex\":\"NroProducto\",\"width\":150,\"hideable\":false},";
                    columns += "{\"text\":\"Producto\",\"dataIndex\":\"Producto\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"SubProducto\",\"dataIndex\":\"SubProducto\",\"width\":140,\"hideable\":false},";
                    //columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":120,\"hideable\":false},";
                    //columns += "{\"text\":\"Emisión\",\"dataIndex\":\"FechaEmision\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":80,\"hideable\":false},";
                    columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"MontoDeuda\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false},";
                    columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"DiasMora\",\"dataIndex\":\"DiasMora\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Vencimiento\",\"dataIndex\":\"FechaMora\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Oficina\",\"dataIndex\":\"Oficina\",\"width\":90,\"hideable\":false},";
                    columns += "{\"text\":\"Cod. Ofic.\",\"dataIndex\":\"CodigoOficina\",\"width\":90,\"hideable\":false},";
                    //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"align\":\"right\",\"dataIndex\":\"MontoExigible\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false,\"renderer\": \"fnColorearPago\"},";
                    //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"MontoPago\",\"width\":80,\"format\":\"0,000.00\",\"hideable\":false, \"renderer\": \"fnColorearPago\"},";
                    //columns += "{\"text\":\"Ult. Fecha Pago\",\"dataIndex\":\"FechaPago\",\"width\":90,\"hideable\":false},";
                    //columns += "{\"text\":\"T. Documento\",\"dataIndex\":\"TipoDocumento\",\"width\":120,\"hideable\":false},";
                    columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                    var lista = (from m in dt.AsEnumerable()
                                 select new
                                 {
                                     DetalleCarteraBBVA = Convert.ToInt32(m["DetalleCarteraBBVA"]),
                                     //TipoDocumento = m["TipoDocumento"].ToString(),
                                     //NumeroDocumento = m["NumeroDocumento"].ToString(),
                                     //FechaEmision = Convert.ToDateTime(m["FechaEmision"]).ToString("yyyy/MM/dd"),
                                     NroProducto = m["NroProducto"].ToString(),
                                     Producto = m["Producto"].ToString(),
                                     SubProducto = m["SubProducto"].ToString(),
                                     DiasMora = m["DiasMora"].ToString(),
                                     Tramo = m["Tramo"].ToString(),
                                     Oficina = m["Oficina"].ToString(),
                                     CodigoOficina = m["CodigoOficina"].ToString(),
                                     FechaMora = Convert.ToDateTime(m["FechaMora"]).ToString("yyyy/MM/dd"),
                                     Moneda = m["Moneda"].ToString(),
                                     MontoDeuda = Convert.ToDecimal(m["MontoDeuda"])
                                     //MontoPago = Convert.ToDecimal(m["Pago"])
                                     //FechaPago = (m["FechaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaPago"]).ToString("dd/MM/yyyy")),
                                     //MontoExigible = Convert.ToDecimal(m["MontoExigible"])
                                 }).ToList<object>();

                    var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                    var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                    respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                }
                return respuestaJson;
            }

            public JsonResult ListarMorososEnCarteraV2(string cliente, short gestionCliente, string fechaInicio,string fechaFin, object[] zonal, object[] departamento, object[] tramo, object[] cluster, object[] producto, bool otrosFiltros, int idParametro, string parametro, double valor1, double valor2)
            {
                 JsonResult respuestaJson= new JsonResult();
                 if (!otrosFiltros)
                 {
                     if (gestionCliente == 1 || gestionCliente == 4 || gestionCliente == 6)
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

                         if (gestionCliente == 1)
                         {
                             DataTable dt = Cartera.Instancia.ListarMorososEnCarteraV2(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo, xmlCluster);
                             //{\"name\":\"CodCartera\",\"type\":\"string\"},
                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Servicio\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Cuenta\",\"type\":\"string\"},{\"name\":\"DetalleCartera\",\"type\":\"int\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                             fields += "{\"name\":\"Pago\",\"type\":\"number\"},";
                             fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                             fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                             columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                             columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"Cuenta\",\"width\":110},";
                             columns += "{\"text\":\"Servicio\",\"dataIndex\":\"Servicio\",\"width\":90,\"filterable\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Total\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"Pago\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                             columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":true},";
                             columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                             columns += "{\"text\":\"DetalleCartera\",\"dataIndex\":\"DetalleCartera\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                             var lista = (from m in dt.AsEnumerable()
                                          select new
                                          {
                                              Cartera = Convert.ToInt32(m["Cartera"]),
                                              DCliente = m["DCliente"].ToString(),
                                              CodCliente = m["CodCliente"].ToString(),
                                              Servicio = m["Servicio"].ToString(),
                                              Sector = m["Sector"].ToString(),
                                              DGestionCliente = m["DGestionCliente"].ToString(),
                                              //CodCartera = m["CodCartera"].ToString(),
                                              Zonal = m["Zonal"].ToString(),
                                              Departamento = m["Departamento"].ToString(),
                                              Provincia = m["Provincia"].ToString(),
                                              Distrito = m["Distrito"].ToString(),
                                              Cluster = m["Cluster"].ToString(),
                                              Tramo = m["Tramo"].ToString(),
                                              Cuenta = m["Cuenta"].ToString(),
                                              DetalleCartera = Convert.ToInt32(m["DetalleCartera"]),
                                              NumeroDocumento = m["NumeroDocumento"].ToString(),
                                              Moroso = Convert.ToInt32(m["Moroso"]),
                                              DMoroso = m["DMoroso"].ToString(),
                                              DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                              Pago = Convert.ToDecimal(m["Pago"]),
                                              Saldo = Convert.ToDecimal(m["Saldo"]),
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                              EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                         else if (gestionCliente == 4)
                         {
                             DataTable dt = Cartera.Instancia.ListarMorososEnCarteraV2(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo, xmlCluster);
                             //{\"name\":\"CodCartera\",\"type\":\"string\"},
                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Inscripcion\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Telefono\",\"type\":\"string\"},{\"name\":\"DetalleCarteraFija\",\"type\":\"int\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Exigible\",\"type\":\"number\"},";
                             fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                             fields += "{\"name\":\"Pago\",\"type\":\"number\"},";
                             fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                             fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                             columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"Telefono\",\"width\":110},";

                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                             columns += "{\"text\":\"Inscripcion\",\"dataIndex\":\"Inscripcion\",\"width\":100,\"filterable\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Total\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Exigible\",\"align\":\"right\",\"dataIndex\":\"Exigible\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"Pago\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                             columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":true},";
                             columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                             columns += "{\"text\":\"DetalleCarteraFija\",\"dataIndex\":\"DetalleCarteraFija\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                             var lista = (from m in dt.AsEnumerable()
                                          select new
                                          {
                                              Cartera = Convert.ToInt32(m["Cartera"]),
                                              DCliente = m["DCliente"].ToString(),
                                              CodCliente = m["CodCliente"].ToString(),
                                              Inscripcion = m["Inscripcion"].ToString(),
                                              Sector = m["Sector"].ToString(),
                                              DGestionCliente = m["DGestionCliente"].ToString(),
                                              //CodCartera = m["CodCartera"].ToString(),
                                              Zonal = m["Zonal"].ToString(),
                                              Departamento = m["Departamento"].ToString(),
                                              Provincia = m["Provincia"].ToString(),
                                              Distrito = m["Distrito"].ToString(),
                                              Cluster = m["Cluster"].ToString(),
                                              Tramo = m["Tramo"].ToString(),
                                              Telefono = m["Telefono"].ToString(),
                                              DetalleCarteraFija = Convert.ToInt32(m["DetalleCarteraFija"]),
                                              NumeroDocumento = m["NumeroDocumento"].ToString(),
                                              Moroso = Convert.ToInt32(m["Moroso"]),
                                              DMoroso = m["DMoroso"].ToString(),
                                              //DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                              Exigible = Convert.ToDecimal(m["Exigible"]),
                                              DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                              Pago = Convert.ToDecimal(m["Pago"]),
                                              Saldo = Convert.ToDecimal(m["Saldo"]),
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                              EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                         else if (gestionCliente == 6)
                         {
                             DataTable dt = Cartera.Instancia.ListarMorososEnCarteraV2(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo, xmlCluster);
                             //{\"name\":\"CodCartera\",\"type\":\"string\"},
                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Anexo\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Telefono\",\"type\":\"string\"},{\"name\":\"DetalleCarteraMovil\",\"type\":\"int\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"Vencido\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"PorVencer\",\"type\":\"number\"},";
                             fields += "{\"name\":\"OfrecerDescuento\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"PagoTotal\",\"type\":\"number\"},";
                             fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Reclamo\",\"type\":\"number\"},";
                             fields += "{\"name\":\"NotaCredito\",\"type\":\"number\"},";
                             fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                             fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"Suspendido\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                             columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"Telefono\",\"width\":110},";

                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                             columns += "{\"text\":\"Anexo\",\"dataIndex\":\"Anexo\",\"width\":90,\"filterable\":true},";
                             //columns += "{\"text\":\"Inscripcion\",\"dataIndex\":\"Inscripcion\",\"width\":90,\"filterable\":true},"; 
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Vencido\",\"align\":\"right\",\"dataIndex\":\"Vencido\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Por Vencer\",\"align\":\"right\",\"dataIndex\":\"PorVencer\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"align\":\"right\",\"dataIndex\":\"Exigible\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Reclamo\",\"align\":\"right\",\"dataIndex\":\"Reclamo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Nota Crédito\",\"align\":\"right\",\"dataIndex\":\"NotaCredito\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestión Call\",\"dataIndex\":\"GestionadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contacto Call\",\"dataIndex\":\"ContactadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Descuento\",\"dataIndex\":\"OfrecerDescuento\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Suspendido\",\"dataIndex\":\"Suspendido\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                             columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                             //columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                             columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                             columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":true},";
                             columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                             //columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                             columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                             columns += "{\"text\":\"DetalleCarteraMovil\",\"dataIndex\":\"DetalleCarteraMovil\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                             var lista = (from m in dt.AsEnumerable()
                                          select new
                                          {
                                              Cartera = Convert.ToInt32(m["Cartera"]),
                                              DCliente = m["DCliente"].ToString(),
                                              CodCliente = m["CodCliente"].ToString(),
                                              Anexo = m["Anexo"].ToString(),
                                              Sector = m["Sector"].ToString(),
                                              DGestionCliente = m["DGestionCliente"].ToString(),
                                              //CodCartera = m["CodCartera"].ToString(),
                                              Zonal = m["Zonal"].ToString(),
                                              Departamento = m["Departamento"].ToString(),
                                              Provincia = m["Provincia"].ToString(),
                                              Distrito = m["Distrito"].ToString(),
                                              Cluster = m["Cluster"].ToString(),
                                              Tramo = m["Tramo"].ToString(),
                                              Telefono = m["Telefono"].ToString(),
                                              DetalleCarteraMovil = Convert.ToInt32(m["DetalleCarteraMovil"]),
                                              NumeroDocumento = m["NumeroDocumento"].ToString(),
                                              Moroso = Convert.ToInt32(m["Moroso"]),
                                              DMoroso = m["DMoroso"].ToString(),
                                              DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                              Reclamo = Convert.ToDecimal(m["Reclamo"]),
                                              NotaCredito = Convert.ToDecimal(m["NotaCredito"]),
                                              //Exigible = Convert.ToDecimal(m["MTO_Exigible"]),
                                              PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                              Saldo = Convert.ToDecimal(m["Saldo"]),
                                              Suspendido = Convert.ToBoolean(m["Suspendido"]),
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                              EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                              OfrecerDescuento = Convert.ToBoolean(m["OfrecerDescuento"]),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                     }
                     else if (gestionCliente == 2 || gestionCliente == 5)
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
                         if (gestionCliente == 2)
                         {
                             DataTable dt = Cartera.Instancia.ListarMorososEnCarteraIBK(cliente, gestionCliente, fechaInicio, xmlDpto, xmlTramo);

                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},{\"name\":\"Actualizado\",\"type\":\"number\"},{\"name\":\"PagoTotal\",\"type\":\"number\"},{\"name\":\"Saldo\",\"type\":\"number\"},";
                             fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"},";
                             fields += "{\"name\":\"DobleSalto\",\"type\":\"bool\"},{\"name\":\"Tramo\",\"type\":\"string\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":50},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                             columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\", \"align\": \"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Actualizado\",\"align\": \"right\",\"dataIndex\":\"Actualizado\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\": \"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\": \"right\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                             //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Doble Salto\",\"dataIndex\":\"DobleSalto\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
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
                                              Sector = m["Sector"].ToString(),
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
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                              //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                              UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                         else if (gestionCliente == 5)
                         {
                             DataTable dt = Cartera.Instancia.ListarMorososEnCarteraIBK(cliente, gestionCliente, fechaInicio, xmlDpto, xmlTramo);

                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Flag\",\"type\":\"string\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},{\"name\":\"ImporteCancelacion\",\"type\":\"number\"},{\"name\":\"PagoTotal\",\"type\":\"number\"},{\"name\":\"SaldoCapital\",\"type\":\"number\"},";
                             fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"},";
                             fields += "{\"name\":\"Tramo\",\"type\":\"string\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":50},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                             columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"DeudaTotal\", \"align\": \"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\": \"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             columns += "{\"text\":\"Flag\",\"dataIndex\":\"Flag\",\"width\":130},";
                             columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110 },";
                             columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":130},";
                             columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":130},";
                             columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                             var lista = (from m in dt.AsEnumerable()
                                          select new
                                          {
                                              Cartera = Convert.ToInt32(m["Cartera"]),
                                              DCliente = m["DCliente"].ToString(),
                                              CodCliente = m["CodCliente"].ToString(),
                                              Sector = m["Sector"].ToString(),
                                              DGestionCliente = m["DGestionCliente"].ToString(),
                                              CodCartera = m["CodCartera"].ToString(),
                                              Departamento = m["Departamento"].ToString(),
                                              Provincia = m["Provincia"].ToString(),
                                              Distrito = m["Distrito"].ToString(),
                                              NumeroDocumento = m["NumeroDocumento"].ToString(),
                                              Tramo = m["Tramo"].ToString(),
                                              Flag = m["Flag"].ToString(),
                                              Moroso = Convert.ToInt32(m["Moroso"]),
                                              DMoroso = m["DMoroso"].ToString(),
                                              DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                              ImporteCancelacion = Convert.ToDecimal(m["ImporteCancelacion"]),
                                              PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                              SaldoCapital = Convert.ToDecimal(m["SaldoCapital"]),
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                              //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                              UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));

                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                     }else if(gestionCliente == 7 || gestionCliente ==8){
                         string xmlZonal = "<root>";
                         string xmlTramo = "<root>";
                         string xmlProducto = "<root>";

                         if (zonal != null)
                         {
                             for (int i = 0; i < zonal.Length; i++)
                             {
                                 xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
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

                         if (producto != null)
                         {
                             for (int i = 0; i < producto.Length; i++)
                             {
                                 xmlProducto += "<producto Producto = '" + producto[i].ToString() + "' />";
                             }
                         }
                         xmlProducto += "</root>";

                         DataTable dt = new DataTable();
                         if (fechaFin == "Asignados")
                         {
                             dt = Cartera.Instancia.ListarAsignadosBBVA(Session["Login"].ToString());
                         }
                         else
                         {
                             dt = Cartera.Instancia.ListarMorososEnCarteraBBVA(cliente, gestionCliente, fechaInicio, xmlZonal, xmlTramo, xmlProducto);
                         }

                         if (gestionCliente == 7)
                         {
                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},";
                             fields += "{\"name\":\"DCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Distrito\",\"type\":\"string\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Moroso\",\"type\":\"int\"},";
                             fields += "{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Marca\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"CodigoOficina\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"Oficina\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DeudaVencida\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"ImporteCancelacion\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"PagoTotal\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"SaldoCapital\",\"type\":\"number\"},";
                             fields += "{\"name\":\"GestionadoCall\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"ContactadoCall\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"TlfContactado\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"Nuevo\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"PromesaPago\", type:\"date\"}]";
                             //fields += "{\"name\":\"Tramo\",\"type\":\"string\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";

                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                             //columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":60,\"filterable\":false},";

                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"SaldoCapital\",\"align\": \"right\",\"dataIndex\":\"SaldoCapital\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"DeudaVencida\", \"align\": \"right\",\"dataIndex\":\"DeudaVencida\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"ImporteCancelacion\",\"align\": \"right\",\"dataIndex\":\"ImporteCancelacion\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";

                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestión Call\",\"dataIndex\":\"GestionadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";

                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contacto Call\",\"dataIndex\":\"ContactadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Doble Salto\",\"dataIndex\":\"DobleSalto\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Nuevo\",\"dataIndex\":\"Nuevo\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             //columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":90,\"filterable\":true},";
                             columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":150},";
                             columns += "{\"text\":\"Marca\",\"dataIndex\":\"Marca\",\"width\":150},";
                             //columns += "{\"text\":\"Cod. Oficina\",\"dataIndex\":\"CodigoOficina\",\"width\":80,\"filterable\":false},";
                             //columns += "{\"text\":\"Oficina\",\"dataIndex\":\"Oficina\",\"width\":150,\"filterable\":false},";
                             //columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                             columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":120},";
                             columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                             var lista = (from m in dt.AsEnumerable()
                                          select new
                                          {
                                              Cartera = Convert.ToInt32(m["Cartera"]),
                                              DCliente = m["DCliente"].ToString(),
                                              CodCliente = m["CodCliente"].ToString(),
                                              //Sector = m["Sector"].ToString(),
                                              DGestionCliente = m["DGestionCliente"].ToString(),
                                              CodCartera = m["CodCartera"].ToString(),
                                              //Departamento = m["Departamento"].ToString(),
                                              //Provincia = m["Provincia"].ToString(),
                                              Distrito = m["Distrito"].ToString(),
                                              NumeroDocumento = m["NumeroDocumento"].ToString(),
                                              //Tramo = m["Tramo"].ToString(),
                                              Moroso = Convert.ToInt32(m["Moroso"]),
                                              Moneda = m["Moneda"].ToString(),
                                              DMoroso = m["DMoroso"].ToString(),
                                              //Oficina = m["Oficina"].ToString(),
                                              //CodigoOficina = m["CodigoOficina"].ToString(),
                                              DeudaVencida = Convert.ToDecimal(m["DeudaVencida"]),
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              GestionadoCall = Convert.ToBoolean(m["GestionadoCall"]),
                                              ContactadoCall = Convert.ToBoolean(m["ContactadoCall"]),
                                              Nuevo = Convert.ToBoolean(m["Nuevo"]),
                                              Marca = m["StatusBanco"].ToString(),
                                              //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));

                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                         else if (gestionCliente == 8)
                         {
                             string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},";
                             fields += "{\"name\":\"DCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},";
                             fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Distrito\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Departamento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Moroso\",\"type\":\"int\"},";
                             fields += "{\"name\":\"DMoroso\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"Marca\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"CodigoOficina\",\"type\":\"string\"},";
                             //fields += "{\"name\":\"Oficina\",\"type\":\"string\"},";
                             fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                             fields += "{\"name\":\"AlCambio\",\"type\":\"number\"},";
                             fields += "{\"name\":\"LiqTotal\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"ImporteCancelacion\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"PagoTotal\",\"type\":\"number\"},";
                             //fields += "{\"name\":\"SaldoCapital\",\"type\":\"number\"},";
                             fields += "{\"name\":\"GestionadoCall\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"ContactadoCall\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"TlfContactado\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"Nuevo\",\"type\":\"bool\"},";
                             fields += "{\"name\":\"PromesaPago\", type:\"date\"}]";
                             //fields += "{\"name\":\"Tramo\",\"type\":\"string\"}]";

                             string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";

                             columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                             //columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                             columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                             columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                             columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":60,\"filterable\":false},";

                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"SaldoCapital\",\"align\": \"right\",\"dataIndex\":\"SaldoCapital\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\", \"align\": \"right\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Al Cambio\", \"align\": \"right\",\"dataIndex\":\"AlCambio\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Liq. Total\", \"align\": \"right\",\"dataIndex\":\"LiqTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                             //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"ImporteCancelacion\",\"align\": \"right\",\"dataIndex\":\"ImporteCancelacion\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";

                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestión Call\",\"dataIndex\":\"GestionadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";

                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contacto Call\",\"dataIndex\":\"ContactadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Doble Salto\",\"dataIndex\":\"DobleSalto\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Nuevo\",\"dataIndex\":\"Nuevo\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                             columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                             //columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":90,\"filterable\":true},";
                             columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":150},";
                             columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":150},";
                             
                             //columns += "{\"text\":\"Marca\",\"dataIndex\":\"Marca\",\"width\":150},";
                             //columns += "{\"text\":\"Cod. Oficina\",\"dataIndex\":\"CodigoOficina\",\"width\":80,\"filterable\":false},";
                             //columns += "{\"text\":\"Oficina\",\"dataIndex\":\"Oficina\",\"width\":150,\"filterable\":false},";
                             //columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                             columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":120},";
                             columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                             var lista = (from m in dt.AsEnumerable()
                                          select new
                                          {
                                              Cartera = Convert.ToInt32(m["Cartera"]),
                                              DCliente = m["DCliente"].ToString(),
                                              CodCliente = m["CodCliente"].ToString(),
                                              //Sector = m["Sector"].ToString(),
                                              DGestionCliente = m["DGestionCliente"].ToString(),
                                              CodCartera = m["CodCartera"].ToString(),
                                              Departamento = m["Departamento"].ToString(),
                                              //Provincia = m["Provincia"].ToString(),
                                              Distrito = m["Distrito"].ToString(),
                                              NumeroDocumento = m["NumeroDocumento"].ToString(),
                                              //Tramo = m["Tramo"].ToString(),
                                              Moroso = Convert.ToInt32(m["Moroso"]),
                                              Moneda = m["Moneda"].ToString(),
                                              DMoroso = m["DMoroso"].ToString(),
                                              //Oficina = m["Oficina"].ToString(),
                                              //CodigoOficina = m["CodigoOficina"].ToString(),
                                              Saldo = Convert.ToDecimal(m["Saldo"]),
                                              LiqTotal = Convert.ToDecimal(m["LiqTotal"]),
                                              AlCambio = Convert.ToDecimal(m["AlCambio"]),
                                              TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                              GestionadoCall = Convert.ToBoolean(m["GestionadoCall"]),
                                              ContactadoCall = Convert.ToBoolean(m["ContactadoCall"]),
                                              Nuevo = Convert.ToBoolean(m["Nuevo"]),
                                              //Marca = m["StatusBanco"].ToString(),
                                              //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                              PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                          }).ToList<object>();

                             var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));

                             var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                             respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                         }
                     }
                 }
                 else {
                     if (gestionCliente == 5) 
                     {
                         DataTable dt = Cartera.Instancia.ListarMorososEnCarteraIBKOtrosFiltros(cliente, gestionCliente, idParametro, parametro.Trim());

                         string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},{\"name\":\"Sector\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Flag\",\"type\":\"string\"},";
                         fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                         fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},{\"name\":\"ImporteCancelacion\",\"type\":\"number\"},{\"name\":\"PagoTotal\",\"type\":\"number\"},{\"name\":\"SaldoCapital\",\"type\":\"number\"},";
                         fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"},";
                         fields += "{\"name\":\"Tramo\",\"type\":\"string\"}]";

                         string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":50},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                         columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                         columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":150},";
                         columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"DeudaTotal\", \"align\": \"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\": \"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Flag\",\"dataIndex\":\"Flag\",\"width\":130},";
                         columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110 },";
                         columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":130},";
                         columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":130},";
                         columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                         var lista = (from m in dt.AsEnumerable()
                                      select new
                                      {
                                          Cartera = Convert.ToInt32(m["Cartera"]),
                                          DCliente = m["DCliente"].ToString(),
                                          CodCliente = m["CodCliente"].ToString(),
                                          Sector = m["Sector"].ToString(),
                                          DGestionCliente = m["DGestionCliente"].ToString(),
                                          CodCartera = m["CodCartera"].ToString(),
                                          Departamento = m["Departamento"].ToString(),
                                          Provincia = m["Provincia"].ToString(),
                                          Distrito = m["Distrito"].ToString(),
                                          NumeroDocumento = m["NumeroDocumento"].ToString(),
                                          Tramo = m["Tramo"].ToString(),
                                          Flag = m["Flag"].ToString(),
                                          Moroso = Convert.ToInt32(m["Moroso"]),
                                          DMoroso = m["DMoroso"].ToString(),
                                          DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                          ImporteCancelacion = Convert.ToDecimal(m["ImporteCancelacion"]),
                                          PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                          SaldoCapital = Convert.ToDecimal(m["SaldoCapital"]),
                                          TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                          EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                          //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                          UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                          PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                      }).ToList<object>();

                         var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));

                         var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                         respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                     }
                     else if (gestionCliente == 2)
                     {
                         DataTable dt = Cartera.Instancia.ListarMorososEnCarteraIBKOtrosFiltros(cliente, gestionCliente, idParametro, parametro.Trim());

                         string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},{\"name\":\"Sector\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},";
                         fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                         fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},{\"name\":\"Actualizado\",\"type\":\"number\"},{\"name\":\"PagoTotal\",\"type\":\"number\"},{\"name\":\"Saldo\",\"type\":\"number\"},";
                         fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"},";
                         fields += "{\"name\":\"DobleSalto\",\"type\":\"bool\"},{\"name\":\"Tramo\",\"type\":\"string\"}]";

                         string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":50},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                         columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                         columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":150},";
                         columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\", \"align\": \"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Actualizado\",\"align\": \"right\",\"dataIndex\":\"Actualizado\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\": \"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\": \"right\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                         //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Doble Salto\",\"dataIndex\":\"DobleSalto\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
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
                                          Sector = m["Sector"].ToString(),
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
                                          TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                          EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                          //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                          UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                          PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                      }).ToList<object>();

                         var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                         var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                         respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                     }
                     else if (gestionCliente == 7)
                     {
                         DataTable dt = Cartera.Instancia.ListarMorososEnCarteraBBVAOtrosFiltros(cliente, gestionCliente, idParametro, parametro.Trim());

                         string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},";
                         fields += "{\"name\":\"DCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodCliente\",\"type\":\"string\"},";
                         //fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Distrito\",\"type\":\"string\"},";
                         fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Moroso\",\"type\":\"int\"},";
                         fields += "{\"name\":\"DMoroso\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Moneda\",\"type\":\"string\"},";
                         fields += "{\"name\":\"CodigoOficina\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Oficina\",\"type\":\"string\"},";
                         fields += "{\"name\":\"DeudaVencida\",\"type\":\"number\"},";
                         //fields += "{\"name\":\"ImporteCancelacion\",\"type\":\"number\"},";
                         //fields += "{\"name\":\"PagoTotal\",\"type\":\"number\"},";
                         //fields += "{\"name\":\"SaldoCapital\",\"type\":\"number\"},";
                         fields += "{\"name\":\"GestionadoCall\",\"type\":\"bool\"},";
                         fields += "{\"name\":\"ContactadoCall\",\"type\":\"bool\"},";
                         fields += "{\"name\":\"TlfContactado\",\"type\":\"bool\"},";
                         fields += "{\"name\":\"PromesaPago\", type:\"date\"}]";
                         //fields += "{\"name\":\"Tramo\",\"type\":\"string\"}]";

                         string columns = "[{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                         columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":120},";
                         columns += "{\"text\":\"DNI\",\"dataIndex\":\"NumeroDocumento\",\"align\": \"right\",\"width\":90,\"filterable\":true,\"hidden\":true},";
                         //columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"align\": \"right\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":95,\"filterable\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                         columns += "{\"text\":\"Moneda\",\"dataIndex\":\"Moneda\",\"width\":60,\"filterable\":false},";
                         //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"SaldoCapital\",\"align\": \"right\",\"dataIndex\":\"SaldoCapital\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"DeudaVencida\", \"align\": \"right\",\"dataIndex\":\"DeudaVencida\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"ImporteCancelacion\",\"align\": \"right\",\"dataIndex\":\"ImporteCancelacion\",\"format\":\"0,000.00\",\"width\":90,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestión Call\",\"dataIndex\":\"GestionadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contacto Call\",\"dataIndex\":\"ContactadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Doble Salto\",\"dataIndex\":\"DobleSalto\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         //columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":90,\"filterable\":true},";
                         columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":150},";
                         columns += "{\"text\":\"Cod. Oficina\",\"dataIndex\":\"CodigoOficina\",\"width\":80,\"filterable\":false},";
                         columns += "{\"text\":\"Oficina\",\"dataIndex\":\"Oficina\",\"width\":150,\"filterable\":false},";
                         //columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                         columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                         var lista = (from m in dt.AsEnumerable()
                                      select new
                                      {
                                          Cartera = Convert.ToInt32(m["Cartera"]),
                                          DCliente = m["DCliente"].ToString(),
                                          CodCliente = m["CodCliente"].ToString(),
                                          //Sector = m["Sector"].ToString(),
                                          DGestionCliente = m["DGestionCliente"].ToString(),
                                          CodCartera = m["CodCartera"].ToString(),
                                          //Departamento = m["Departamento"].ToString(),
                                          //Provincia = m["Provincia"].ToString(),
                                          Distrito = m["Distrito"].ToString(),
                                          NumeroDocumento = m["NumeroDocumento"].ToString(),
                                          //Tramo = m["Tramo"].ToString(),
                                          Moroso = Convert.ToInt32(m["Moroso"]),
                                          Moneda = m["Moneda"].ToString(),
                                          DMoroso = m["DMoroso"].ToString(),
                                          Oficina = m["Oficina"].ToString(),
                                          CodigoOficina = m["CodigoOficina"].ToString(),
                                          DeudaVencida = Convert.ToDecimal(m["DeudaVencida"]),
                                          TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                          GestionadoCall = Convert.ToBoolean(m["GestionadoCall"]),
                                          ContactadoCall = Convert.ToBoolean(m["ContactadoCall"]),
                                          //DobleSalto = Convert.ToBoolean(m["DobleSalto"]),
                                          PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                      }).ToList<object>();

                         var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));

                         var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                         respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                     }
                     else if (gestionCliente == 6)
                     {
                         DataTable dt = Cartera.Instancia.ListarMorososEnCarteraOtrosFiltros(cliente, gestionCliente, idParametro, parametro.Trim(), valor1, valor2);
                         //{\"name\":\"CodCartera\",\"type\":\"string\"},
                         string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Anexo\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Telefono\",\"type\":\"string\"},{\"name\":\"DetalleCarteraMovil\",\"type\":\"int\"},";
                         fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                         fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                         fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},";
                         //fields += "{\"name\":\"PorVencer\",\"type\":\"number\"},";
                         fields += "{\"name\":\"OfrecerDescuento\",\"type\":\"bool\"},";
                         fields += "{\"name\":\"PagoTotal\",\"type\":\"number\"},";
                         fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Reclamo\",\"type\":\"number\"},";
                         fields += "{\"name\":\"NotaCredito\",\"type\":\"number\"},";
                         fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                         fields += "{\"name\":\"FechaFin\",\"type\":\"date\"},";
                         fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"Suspendido\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"}]";

                         string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                         columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                         columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"Telefono\",\"width\":110},";

                         columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                         columns += "{\"text\":\"Anexo\",\"dataIndex\":\"Anexo\",\"width\":90,\"filterable\":true},";
                         //columns += "{\"text\":\"Inscripcion\",\"dataIndex\":\"Inscripcion\",\"width\":90,\"filterable\":true},"; 
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                         //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Vencido\",\"align\":\"right\",\"dataIndex\":\"Vencido\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Por Vencer\",\"align\":\"right\",\"dataIndex\":\"PorVencer\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"align\":\"right\",\"dataIndex\":\"Exigible\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"PagoTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Reclamo\",\"align\":\"right\",\"dataIndex\":\"Reclamo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Nota Crédito\",\"align\":\"right\",\"dataIndex\":\"NotaCredito\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestión Call\",\"dataIndex\":\"GestionadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contacto Call\",\"dataIndex\":\"ContactadoCall\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Descuento\",\"dataIndex\":\"OfrecerDescuento\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Suspendido\",\"dataIndex\":\"Suspendido\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                         columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                         //columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                         columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                         columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":true},";
                         columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fin Cartera\",\"dataIndex\":\"FechaFin\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         
                         columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                         columns += "{\"text\":\"DetalleCarteraMovil\",\"dataIndex\":\"DetalleCarteraMovil\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                         var lista = (from m in dt.AsEnumerable()
                                      select new
                                      {
                                          Cartera = Convert.ToInt32(m["Cartera"]),
                                          DCliente = m["DCliente"].ToString(),
                                          CodCliente = m["CodCliente"].ToString(),
                                          Anexo = m["Anexo"].ToString(),
                                          Sector = m["Sector"].ToString(),
                                          DGestionCliente = m["DGestionCliente"].ToString(),
                                          CodCartera = m["CodCartera"].ToString(),
                                          Zonal = m["Zonal"].ToString(),
                                          Departamento = m["Departamento"].ToString(),
                                          Provincia = m["Provincia"].ToString(),
                                          Distrito = m["Distrito"].ToString(),
                                          Cluster = m["Cluster"].ToString(),
                                          Tramo = m["Tramo"].ToString(),
                                          Telefono = m["Telefono"].ToString(),
                                          DetalleCarteraMovil = Convert.ToInt32(m["DetalleCarteraMovil"]),
                                          NumeroDocumento = m["NumeroDocumento"].ToString(),
                                          Moroso = Convert.ToInt32(m["Moroso"]),
                                          DMoroso = m["DMoroso"].ToString(),
                                          DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                          Reclamo = Convert.ToDecimal(m["Reclamo"]),
                                          NotaCredito = Convert.ToDecimal(m["NotaCredito"]),
                                          //Exigible = Convert.ToDecimal(m["MTO_Exigible"]),
                                          PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                          Saldo = Convert.ToDecimal(m["Saldo"]),
                                          Suspendido = Convert.ToBoolean(m["Suspendido"]),
                                          TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                          FechaFin = (m["FechaFin"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaFin"]).ToString("yyyy/MM/dd")),
                                          UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                          EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                          OfrecerDescuento = Convert.ToBoolean(m["OfrecerDescuento"]),
                                          PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                      }).ToList<object>();

                         var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                         var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                         respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                     }
                     else if (gestionCliente == 4)
                     {
                         DataTable dt = Cartera.Instancia.ListarMorososEnCarteraOtrosFiltros(cliente, gestionCliente, idParametro, parametro.Trim(), valor1, valor2);
                         //{\"name\":\"CodCartera\",\"type\":\"string\"},
                         string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Inscripcion\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Telefono\",\"type\":\"string\"},{\"name\":\"DetalleCarteraFija\",\"type\":\"int\"},";
                         fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Exigible\",\"type\":\"number\"},";
                         fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                         fields += "{\"name\":\"Pago\",\"type\":\"number\"},";
                         fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                         fields += "{\"name\":\"FechaFin\",\"type\":\"date\"},";
                         fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},";
                         fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"}]";

                         string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                         columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                         columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"Telefono\",\"width\":110},";

                         columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                         columns += "{\"text\":\"Inscripcion\",\"dataIndex\":\"Inscripcion\",\"width\":100,\"filterable\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Total\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Exigible\",\"align\":\"right\",\"dataIndex\":\"Exigible\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"Pago\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                         columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":true},";
                         columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fin Cartera\",\"dataIndex\":\"FechaFin\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                         columns += "{\"text\":\"DetalleCarteraFija\",\"dataIndex\":\"DetalleCarteraFija\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                         var lista = (from m in dt.AsEnumerable()
                                      select new
                                      {
                                          Cartera = Convert.ToInt32(m["Cartera"]),
                                          DCliente = m["DCliente"].ToString(),
                                          CodCliente = m["CodCliente"].ToString(),
                                          Inscripcion = m["Inscripcion"].ToString(),
                                          Sector = m["Sector"].ToString(),
                                          DGestionCliente = m["DGestionCliente"].ToString(),
                                          CodCartera = m["CodCartera"].ToString(),
                                          Zonal = m["Zonal"].ToString(),
                                          Departamento = m["Departamento"].ToString(),
                                          Provincia = m["Provincia"].ToString(),
                                          Distrito = m["Distrito"].ToString(),
                                          Cluster = m["Cluster"].ToString(),
                                          Tramo = m["Tramo"].ToString(),
                                          Telefono = m["Telefono"].ToString(),
                                          DetalleCarteraFija = Convert.ToInt32(m["DetalleCarteraFija"]),
                                          NumeroDocumento = m["NumeroDocumento"].ToString(),
                                          Moroso = Convert.ToInt32(m["Moroso"]),
                                          DMoroso = m["DMoroso"].ToString(),
                                          //DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                          Exigible = Convert.ToDecimal(m["Exigible"]),
                                          DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                          Pago = Convert.ToDecimal(m["Pago"]),
                                          Saldo = Convert.ToDecimal(m["Saldo"]),
                                          TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                          FechaFin = (m["FechaFin"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaFin"]).ToString("yyyy/MM/dd")),
                                          UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                          EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                          PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                      }).ToList<object>();

                         var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                         var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                         respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                     }
                     else if (gestionCliente == 1)
                     {
                         DataTable dt = Cartera.Instancia.ListarMorososEnCarteraOtrosFiltros(cliente, gestionCliente, idParametro, parametro.Trim(), valor1, valor2);
                         //{\"name\":\"CodCartera\",\"type\":\"string\"},
                         string fields = "[{\"name\":\"Cartera\",\"type\":\"int\"},{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Servicio\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Cuenta\",\"type\":\"string\"},{\"name\":\"DetalleCartera\",\"type\":\"int\"},";
                         fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"Moroso\",\"type\":\"int\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                         fields += "{\"name\":\"DeudaTotal\",\"type\":\"number\"},";
                         fields += "{\"name\":\"Pago\",\"type\":\"number\"},";
                         fields += "{\"name\":\"Sector\",\"type\":\"string\"},";
                         fields += "{\"name\":\"Saldo\",\"type\":\"number\"},";
                         fields += "{\"name\":\"FechaFin\",\"type\":\"date\"},";
                         fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},";
                         fields += "{\"name\":\"UltGestionCall\",\"type\":\"date\"},{\"name\":\"TlfContactado\",\"type\":\"bool\"},{\"name\":\"EstadoGestionCall\",\"type\":\"string\"},{\"name\":\"PromesaPago\",\"type\":\"date\"}]";

                         string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Cartera\",\"dataIndex\":\"Cartera\",\"hidden\":true,\"hideable\":false},";
                         columns += "{\"text\":\"Sector\",\"dataIndex\":\"Sector\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                         columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                         columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"Cuenta\",\"width\":110},";
                         columns += "{\"text\":\"Servicio\",\"dataIndex\":\"Servicio\",\"width\":90,\"filterable\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"Moroso\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda Total\",\"align\":\"right\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Pago\",\"align\":\"right\",\"dataIndex\":\"Pago\",\"format\":\"0.00\",\"width\":70,\"filterable\":true, \"renderer\": \"fnColorearPago\"},";
                         columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"align\":\"right\",\"dataIndex\":\"Saldo\",\"format\":\"0.00\",\"width\":70,\"filterable\":true},";
                         columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Tlf. Contactado\",\"dataIndex\":\"TlfContactado\",\"processEvent\":'function() { return false; }',\"width\":110,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Ult. Gestión\",\"dataIndex\":\"UltGestionCall\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Estado Gestion Call\",\"dataIndex\":\"EstadoGestionCall\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Promesa\",\"dataIndex\":\"PromesaPago\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                         columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                         columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                         columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":true},";
                         columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                         columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fin Cartera\",\"dataIndex\":\"FechaFin\",\"width\":110,\"filterable\":true, \"format\":\"d/m/Y\"},";
                         columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                         columns += "{\"text\":\"DetalleCartera\",\"dataIndex\":\"DetalleCartera\",\"hideable\":false,\"hidden\":true},";
                         columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                         var lista = (from m in dt.AsEnumerable()
                                      select new
                                      {
                                          Cartera = Convert.ToInt32(m["Cartera"]),
                                          DCliente = m["DCliente"].ToString(),
                                          CodCliente = m["CodCliente"].ToString(),
                                          Servicio = m["Servicio"].ToString(),
                                          Sector = m["Sector"].ToString(),
                                          DGestionCliente = m["DGestionCliente"].ToString(),
                                          CodCartera = m["CodCartera"].ToString(),
                                          Zonal = m["Zonal"].ToString(),
                                          Departamento = m["Departamento"].ToString(),
                                          Provincia = m["Provincia"].ToString(),
                                          Distrito = m["Distrito"].ToString(),
                                          Cluster = m["Cluster"].ToString(),
                                          Tramo = m["Tramo"].ToString(),
                                          Cuenta = m["Cuenta"].ToString(),
                                          DetalleCartera = Convert.ToInt32(m["DetalleCartera"]),
                                          NumeroDocumento = m["NumeroDocumento"].ToString(),
                                          Moroso = Convert.ToInt32(m["Moroso"]),
                                          DMoroso = m["DMoroso"].ToString(),
                                          DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                          Pago = Convert.ToDecimal(m["Pago"]),
                                          Saldo = Convert.ToDecimal(m["Saldo"]),
                                          TlfContactado = Convert.ToBoolean(m["TlfContactado"]),
                                          UltGestionCall = (m["UltGestionCall"] == DBNull.Value ? "" : Convert.ToDateTime(m["UltGestionCall"]).ToString("yyyy/MM/dd")),
                                          FechaFin = (m["FechaFin"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaFin"]).ToString("yyyy/MM/dd")),
                                          EstadoGestionCall = m["EstadoGestionCall"].ToString(),
                                          PromesaPago = (m["PromesaPago"] == DBNull.Value ? "" : Convert.ToDateTime(m["PromesaPago"]).ToString("yyyy/MM/dd"))
                                      }).ToList<object>();

                         var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                         var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                         respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista, total = lista.Count }, JsonRequestBehavior.AllowGet);
                     }
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

            public JsonResult ListarMorosos(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, object[] tramo)
            {
                JsonResult respuestaJson = new JsonResult();
                if (gestionCliente == 1 || gestionCliente == 4) {
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";
                    string xmlTramo = "<root>";

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

                    if (tramo != null)
                    {
                        for (int i = 0; i < tramo.Length; i++)
                        {
                            xmlTramo += "<tramo Tramo = '" + tramo[i].ToString() + "' />";
                        }
                    }
                    xmlTramo += "</root>";

                    if (gestionCliente == 1)
                    {
                        DataTable dt = Cartera.Instancia.ListarMorososGrid(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo);

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
                    }
                    else if (gestionCliente == 4)
                    {
                        DataTable dt = Cartera.Instancia.ListarMorososGrid(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo);
                        //,{\"name\":\"Saldo\",\"type\":\"float\"}
                        string fields = "[{\"name\":\"DCliente\",\"type\":\"string\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"},";
                        fields += "{\"name\":\"CodCartera\",\"type\":\"string\"},{\"name\":\"Zonal\",\"type\":\"string\"},{\"name\":\"Departamento\",\"type\":\"string\"},";
                        fields += "{\"name\":\"Cuenta\",\"type\":\"string\"},{\"name\":\"Inscripcion\",\"type\":\"string\"},{\"name\":\"CodCliente\",\"type\":\"string\"},";
                        fields += "{\"name\":\"Provincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"string\"},{\"name\":\"Cluster\",\"type\":\"string\"},";
                        fields += "{\"name\":\"Tramo\",\"type\":\"string\"},{\"name\":\"Telefono\",\"type\":\"string\"},";
                        fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"DMoroso\",\"type\":\"string\"},";
                        fields += "{\"name\":\"DeudaTotal\",\"type\":\"float\"},{\"name\":\"DeudaExigible\",\"type\":\"float\"}]";
                        //fields += "{\"name\":\"Gestionado\",\"type\":\"bool\"},{\"name\":\"Contactado\",\"type\":\"bool\"},{\"name\":\"PromesaPago\",\"type\":\"bool\"}]";

                        string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},";
                        columns += "{\"text\":\"Cluster\",\"dataIndex\":\"Cluster\",\"width\":60,\"filterable\":true},";
                        columns += "{\"text\":\"Cliente\",\"dataIndex\":\"DCliente\",\"width\":150,\"hideable\":false,\"hidden\":true},";
                        columns += "{\"text\":\"Gestión\",\"dataIndex\":\"DGestionCliente\",\"width\":100,\"hideable\":false,\"hidden\":true},";
                        columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":100,\"filterable\":true},";
                        columns += "{\"text\":\"CodCliente\",\"dataIndex\":\"CodCliente\",\"width\":90,\"filterable\":true},";
                        columns += "{\"text\":\"Cuenta\",\"dataIndex\":\"Cuenta\",\"width\":90,\"filterable\":true},";
                        columns += "{\"text\":\"Inscripcion\",\"dataIndex\":\"Inscripcion\",\"width\":90,\"filterable\":true},";
                        columns += "{\"text\":\"Moroso\",\"dataIndex\":\"DMoroso\",\"width\":280,\"filterable\":true},";
                        columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Deuda\",\"dataIndex\":\"DeudaTotal\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                        columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Exigible\",\"dataIndex\":\"DeudaExigible\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                        //columns += "{\"xtype\":\"numbercolumn\",\"text\":\"Saldo\",\"dataIndex\":\"Saldo\",\"format\":\"0,000.##\",\"width\":70,\"filterable\":true},";
                        //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Gestionado\",\"dataIndex\":\"Gestionado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                        //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Contactado\",\"dataIndex\":\"Contactado\",\"processEvent\":'function() { return false; }',\"width\":100,\"filterable\":true},";
                        //columns += "{\"xtype\":\"checkcolumn\",\"text\":\"Con Promesa\",\"dataIndex\":\"PromesaPago\",\"processEvent\":'function () { return false; }',\"width\":100,\"filterable\":true},";
                        columns += "{\"text\":\"Zonal\",\"dataIndex\":\"Zonal\",\"width\":60,\"filterable\":true},";
                        columns += "{\"text\":\"Provincia\",\"dataIndex\":\"Provincia\",\"width\":140,\"hideable\":false,\"hidden\":true},";
                        columns += "{\"text\":\"Departamento\",\"dataIndex\":\"Departamento\",\"width\":110,\"filterable\":true},";
                        columns += "{\"text\":\"Distrito\",\"dataIndex\":\"Distrito\",\"width\":140},";
                        columns += "{\"text\":\"Tramo\",\"dataIndex\":\"Tramo\",\"width\":80,\"hideable\":false,\"hidden\":true},";
                        columns += "{\"text\":\"Cartera\",\"dataIndex\":\"CodCartera\",\"width\":190},";
                        columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"Telefono\",\"width\":110},";
                        //columns += "{\"text\":\"DetalleCarteraFija\",\"dataIndex\":\"DetalleCarteraFija\",\"hideable\":false,\"hidden\":true},";
                        columns += "{\"flex\":1,\"menuDisabled\":true,\"hideable\":false}]";

                        var lista = (from m in dt.AsEnumerable()
                                     select new
                                     {
                                         DCliente = m["DCliente"].ToString(),
                                         CodCliente = m["CodCliente"].ToString(),
                                         Cuenta = m["Cuenta"].ToString(),
                                         Inscripcion = m["Inscripcion"].ToString(),
                                         DGestionCliente = m["DGestionCliente"].ToString(),
                                         CodCartera = m["CodCartera"].ToString(),
                                         Zonal = m["Zonal"].ToString(),
                                         Departamento = m["Departamento"].ToString(),
                                         Provincia = m["Provincia"].ToString(),
                                         Distrito = m["Distrito"].ToString(),
                                         Cluster = m["Cluster"].ToString(),
                                         Tramo = m["Tramo"].ToString(),
                                         Telefono = m["Telefono"].ToString(),
                                         NumeroDocumento = m["NumeroDocumento"].ToString(),
                                         DMoroso = m["DMoroso"].ToString(),
                                         DeudaTotal = Convert.ToDecimal(m["DeudaTotal"]),
                                         DeudaExigible = Convert.ToDecimal(m["DeudaExigible"])
                                         //PagoTotal = Convert.ToDecimal(m["PagoTotal"]),
                                         //Saldo = Convert.ToDecimal(m["Saldo"]),
                                         //Gestionado = Convert.ToBoolean(m["Gestionado"]),
                                         //Contactado = Convert.ToBoolean(m["Contactado"]),
                                         //PromesaPago = Convert.ToBoolean(m["PromesaPago"])
                                     }).ToList<object>();

                        var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                        var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                        respuestaJson = Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
                    }
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
                if (gestionCliente == 1 || gestionCliente == 4) { 
                
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
                else if (gestionCliente == 2 || gestionCliente == 5)
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

            public void ExportarGestion(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, DateTime fechaDesde, DateTime fechaHasta, bool mejorGestion, bool promesa)
            {
                if (gestionCliente == 1 || gestionCliente == 4 || gestionCliente == 6) {
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
                    if (gestionCliente == 1)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=GestCable-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    else if (gestionCliente == 4)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=GestFija-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    else if (gestionCliente == 6)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=GestMovil-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    if (gestionCliente == 1)
                    {
                        Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestCABLE").ToArray());
                    }
                    else if (gestionCliente == 4) 
                    {
                        Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestFIJA").ToArray());
                    }
                    else if (gestionCliente == 6)
                    {
                        Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestMOVIL").ToArray());
                    }
                    Response.End();
                }
                else if (gestionCliente == 2 || gestionCliente == 5) {
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
                        if (!promesa)
                        {
                            dt = Cartera.Instancia.ListarGestionesIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                            Response.Clear();
                            if (gestionCliente == 2)
                            {
                                Response.AddHeader("content-disposition", "attachment;filename=GestVINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            }
                            else if (gestionCliente == 5)
                            {
                                Response.AddHeader("content-disposition", "attachment;filename=GestCINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            }
                            
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.csv";
                            Response.Charset = "UTF-8";
                            Response.ContentEncoding = System.Text.Encoding.Unicode;
                            if (gestionCliente == 2)
                            {
                                Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestVINTE").ToArray());
                            }
                            else if (gestionCliente == 5)
                            {
                                Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestCINTE").ToArray());
                            }
                            
                            Response.End();
                        }
                        else
                        {
                            dt = Cartera.Instancia.ListarPromesasIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment;filename=PromVINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.csv";
                            Response.Charset = "UTF-8";
                            Response.ContentEncoding = System.Text.Encoding.Unicode;
                            Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestProm").ToArray());
                            Response.End();
                        }
                    }
                    else
                    {
                        dt = Cartera.Instancia.ListarMejorGestionIBK(cliente, gestionCliente, fechaInicio, xmlDpto, fechaDesde, fechaHasta);
                        Response.Clear();
                        Response.AddHeader("content-disposition", "attachment;filename=GestVINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.csv";
                        Response.Charset = "UTF-8";
                        Response.ContentEncoding = System.Text.Encoding.Unicode;
                        Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestVINTE").ToArray());
                        Response.End();
                    }
                    //DataTable dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
                }
                else if (gestionCliente == 7 || gestionCliente == 8)
                {
                    string xmlDpto = "<root>";

                    DataTable dt = new DataTable();
                    if (!mejorGestion)
                    {
                        if (!promesa)
                        {
                            dt = Cartera.Instancia.ListarGestionesBBVA(cliente, gestionCliente, fechaDesde, fechaHasta);
                            Response.Clear();
                            if (gestionCliente == 7)
                            {
                                Response.AddHeader("content-disposition", "attachment;filename=Gest_BBVA_Vencida-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            }
                            else if (gestionCliente == 8)
                            {
                                Response.AddHeader("content-disposition", "attachment;filename=Gest_BBVA_EXJ-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            }

                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.csv";
                            Response.Charset = "UTF-8";
                            Response.ContentEncoding = System.Text.Encoding.Unicode;
                            if (gestionCliente == 7)
                            {
                                Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestBBVAV").ToArray());
                            }
                            else if (gestionCliente == 8)
                            {
                                Response.BinaryWrite(ConvertirTablaToExcel(dt, "GestBBVAE").ToArray());
                            }

                            Response.End();
                        }
                        else
                        {
                            dt = Cartera.Instancia.ListarPromesasBBVA(cliente, gestionCliente, fechaDesde, fechaHasta);
                            Response.Clear();
                            if (gestionCliente == 7)
                            {
                                Response.AddHeader("content-disposition", "attachment;filename=Promesas_BBVA_Vencida-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            }
                            else if (gestionCliente == 8)
                            {
                                Response.AddHeader("content-disposition", "attachment;filename=Promesas_BBVA_EXJ-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                            }
                            
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.csv";
                            Response.Charset = "UTF-8";
                            Response.ContentEncoding = System.Text.Encoding.Unicode;
                            Response.BinaryWrite(ConvertirTablaToExcel(dt, "PromBBVA_V").ToArray());
                            Response.End();
                        }
                    }
                    else
                    {
                        dt = Cartera.Instancia.ListarMejorGestionBBVA(cliente, gestionCliente, fechaDesde, fechaHasta);
                        Response.Clear();
                        Response.AddHeader("content-disposition", "attachment;filename=MejorGestion_BBVA" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.csv";
                        Response.Charset = "UTF-8";
                        Response.ContentEncoding = System.Text.Encoding.Unicode;
                        Response.BinaryWrite(ConvertirTablaToExcel(dt, "MGBBVA_V").ToArray());
                        Response.End();
                    }
                    //DataTable dt = Cartera.Instancia.ListarGestiones(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, fechaDesde, fechaHasta);
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
                Response.AddHeader("content-disposition", "attachment;filename=PagosCABLE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.csv";
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.BinaryWrite(ConvertirTablaToExcel(dt, "PagosCable").ToArray());
                Response.End();
            }

            public void ExportarListaMorosos(string cliente, short gestionCliente, string fechaFin, string fechaInicio, object[] zonal, object[] departamento, object[] tramo)
            {
                if (gestionCliente == 1 || gestionCliente == 4 || gestionCliente == 6) { 
                
                    string xmlZonal = "<root>";
                    string xmlDpto = "<root>";
                    string xmlTramo = "<root>";
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

                    if (tramo != null)
                    {
                        for (int i = 0; i < tramo.Length; i++)
                        {
                            xmlTramo += "<tramo Tramo = '" + tramo[i].ToString() + "' />";
                        }
                    }
                    xmlTramo += "</root>";
                    if (gestionCliente == 1)
                    {
                        DataTable dt = Cartera.Instancia.ListarMorosos(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo);
                        dv = dt.DefaultView;
                        dv.Sort = "Servicio";
                        DataTable dtFinal = JuntarDetalle(dv.ToTable());
                        Response.Clear();
                        Response.AddHeader("content-disposition", "attachment;filename=CABLE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.csv";
                        Response.Charset = "UTF-8";
                        Response.ContentEncoding = System.Text.Encoding.Unicode;
                        Response.BinaryWrite(ConvertirTablaToExcel(dtFinal, "Cable-Morosos").ToArray());
                        Response.End();
                    }
                    else if (gestionCliente == 4) 
                    {
                        DataTable dt = Cartera.Instancia.ListarMorosos(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo);
                        dv = dt.DefaultView;
                        dv.Sort = "Telefono";
                        DataTable dtFinal = JuntarDetalleFija(dv.ToTable());
                        Response.Clear();
                        Response.AddHeader("content-disposition", "attachment;filename=FIJA-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.csv";
                        Response.Charset = "UTF-8";
                        Response.ContentEncoding = System.Text.Encoding.Unicode;
                        Response.BinaryWrite(ConvertirTablaToExcel(dtFinal, "Fija-Morosos").ToArray());
                        Response.End();
                    }
                    else if (gestionCliente == 6)
                    {
                        DataTable dt = Cartera.Instancia.ListarMorosos(cliente, gestionCliente, fechaFin, xmlZonal, xmlDpto, xmlTramo);
                        dv = dt.DefaultView;
                        dv.Sort = "Telefono";
                        DataTable dtFinal = JuntarDetalleMovil(dv.ToTable());
                        Response.Clear();
                        Response.AddHeader("content-disposition", "attachment;filename=Movil-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                        Response.Charset = "";
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.csv";
                        Response.Charset = "UTF-8";
                        Response.ContentEncoding = System.Text.Encoding.Unicode;
                        Response.BinaryWrite(ConvertirTablaToExcel(dtFinal, "Movil-Morosos").ToArray());
                        Response.End();
                    }
                }
                else if (gestionCliente == 7 || gestionCliente == 8) {
                    string xmlZonal = "<root>";
                    DataView dv = new DataView();

                    if (zonal != null)
                    {
                        for (int i = 0; i < zonal.Length; i++)
                        {
                            xmlZonal += "<zonal Zonal = '" + zonal[i].ToString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";
                    DataTable dt = Cartera.Instancia.ListarMorososBBVA(cliente, gestionCliente, fechaInicio, xmlZonal);
                    dv = dt.DefaultView;
                    //dv.Sort = "Servicio";
                    //DataTable dtFinal = JuntarDetalle(dv.ToTable());
                    Response.Clear();
                    if (gestionCliente == 7)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=BBVA_V-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    else if (gestionCliente == 8)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=BBVA_EXJ-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    Response.BinaryWrite(ConvertirTablaToExcel(dt, "BBVA-Morosos").ToArray());
                    Response.End();
                
                }
                else if (gestionCliente == 2 || gestionCliente == 5)
                {
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
                    if (gestionCliente == 2)
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=VINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    else
                    {
                        Response.AddHeader("content-disposition", "attachment;filename=CINTE-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".xls");
                    }
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.csv";
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = System.Text.Encoding.Unicode;
                    Response.BinaryWrite(ConvertirTablaToExcel(dt, "IBK-Morosos").ToArray());
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
                    int moroso = Convert.ToInt32(lista[0]["Moroso"]);
                    short dclaseGestion = Convert.ToInt16(lista[0]["DClaseGestion"]);
                    short trabajador = Convert.ToInt16(lista[0]["Trabajador"]);
                    DateTime fechaGestion = Convert.ToDateTime(lista[0]["FechaGestion"]);
                    DateTime? fechaPromesa = ( lista[0]["FechaPromesa"] == null ? new DateTime?() : Convert.ToDateTime(lista[0]["FechaPromesa"]));
                    string horaGestion = Convert.ToDateTime(lista[0]["HoraGestion"]).ToString("HH:mm");
                    decimal monto = Convert.ToDecimal(lista[0]["Monto"]);
                    string observacion = lista[0]["Observacion"].ToString();
                    string razonNoPago = (lista[0]["RazonNoPago"] == null ? null : lista[0]["RazonNoPago"].ToString());
                    int result = Cartera.Instancia.InsUpdGestionMoroso(gestionMoroso,cartera,detalleMoroso,tipoGestion,claseGestion,moroso,dclaseGestion,fechaGestion,horaGestion,fechaPromesa,monto,observacion,trabajador,Session["Login"].ToString(), razonNoPago);
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
                    if (gestionCliente == 1 || gestionCliente == 4)
                    {
                        zonal = lista[0]["Zonal"].ToString();
                        fechaFin = lista[0]["FechaFin"].ToString();
                    }
                    else if (gestionCliente == 2 || gestionCliente == 5) 
                    {
                        fechaInicio = lista[0]["FechaInicio"].ToString();
                    }
                    string departamento = lista[0]["Departamento"].ToString();
                    string provincia = lista[0]["Provincia"].ToString();
                    string distrito = lista[0]["Distrito"].ToString();
                    string sector = lista[0]["Sector"].ToString();
                    if (sector.Length == 1) sector = "0" + sector;

                    if (gestionCliente == 1 || gestionCliente == 4) 
                    {
                        Cartera.Instancia.InsUpdSectores(cliente, gestionCliente, fechaFin, zonal, departamento, provincia, distrito, sector);
                    }
                    else if (gestionCliente == 2 || gestionCliente == 5)
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
                JsonResult respuestaJson = new JsonResult();
                int contadorlinea = 1;
                try
                {
                    if (gestionCliente == 5)
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
                                    valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                    //xml += (sheet[0, j].Value.Equals("FechaAsignacion") ? "CodCartera = 'VINTE" + DateTime.Now.ToString("yyyyMM") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FechaAsignacion") ? "FechaInicio = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' FechaFin = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Código") ? "CodigoCliente = '" + valor + "' CodCartera = 'CINTE" + DateTime.Now.ToString("yyyyMM") + "' FechaInicio = '" + DateTime.Now.ToString("yyyyMMdd") + "' FechaFin = '" + DateTime.Now.ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NroProdOrig") ? "NroProdOrig = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("NroTC") ? "NroTarjeta = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Número de CJ") ? "NroCJ = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NomProd") ? "Producto = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NomSubProd") ? "SubProducto = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DescripMotivoBloqueo") ? "MotivoBloqueo = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("TramoClienteReal Inicial") ? "Tramo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Días de Mora") ? "IniDMProducto = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("IniDMCliente") ? "IniDMCliente = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("MinimoMN") ? "MinimoMN = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("MinimoME") ? "MinimoME = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("VencidoMN") ? "VencidoMN = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("VencidoME") ? "VencidoME = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("TotalVencido") ? "ToralVencido = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Importe_Cancelación") ? "ImporteCancelacion = '" + (valor.Equals("")? "0" : valor) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Deuda_Total") ? "DeudaTotal = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Saldo Capital") ? "SaldoCapital = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Nombre_Empresa") ? "NombreEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("RUC") ? "RucEmpresa = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DIRECCION") ? "Direccion = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Dist") ? "Distrito = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Prov") ? "Provincia = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Dep") ? "Departamento = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Direccion_Empresa") ? "DireccionEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Distrito_Empresa") ? "DistritoEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Provincia_Empresa") ? "ProvinciaEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Dep_Empresa") ? "DepartamentoEmpresa = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("SITUACION LABORAL") ? "SituacionLaboral = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("ESTATAL?") ? "Estatal = '" + (valor.Equals("Si") ? Convert.ToString(true) : Convert.ToString(false)) + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("RANGO_SUELDO") ? "RangoSueldo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Moneda") ? "Moneda = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FEC-DESEMB") && valor.Length > 0 ? "FechaPrestamo = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FEC-1ER-VENC") && valor.Length > 0 ? "FechaCuota = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CANT CUOTAS") ? "NroCuotas = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DIRECC-CLIENTE") ? "DireccionCliente = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("IMP-CUO-ORD") ? "MontoCuota = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FEC-PROX-VENC") && valor.Length > 0 ? "FechaProxVencimiento = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CUOTAS PAGADAS") ? "CuotasPagadas = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CUOTAS PENDIENTES") ? "CuotasPendientes = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CUOTAS VENCIDAS") ? "CuotasVencidas = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("IMP-CANCELACION") ? "ImporteCancelacion = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NroDoc") ? "NumeroDocumento = '" + valor + "' TipoDocumento = 'DN' " : "");
                                    xml += (sheet[0, j].Value.Equals("Nombre") ? "RazonSocial = '" + valor + "' " : "");
                                    //distrito = (distrito.Length == 0 ? (sheet[0, j].Value.Equals("Distrito") ? valor : "") : distrito);
                                    //provincia = (provincia.Length == 0 ? (sheet[0, j].Value.Equals("Provincia") ? valor : "") : provincia);
                                    //departamento = (departamento.Length == 0 ? (sheet[0, j].Value.Equals("Departamento") ? valor : "") : departamento);
                                    direccion = (direccion.Length == 0 ? (sheet[0, j].Value.Equals("Dir") ? valor : "") : direccion);

                                    //detallemoroso
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Casa") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Celular") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Trabajo") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Referencia") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    //xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfDesconocido") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.Equals("Email1") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '4' />" : "");
                                }
                            }
                            xmlDetalle += "<detalle Descripcion = '" + direccion + "' TipoDetalle = '2' />";
                            cadena.Append(xml + " >");
                            cadena.Append(xmlDetalle);
                            cadena.Append("</cartera>");
                        }
                        cadena.Append("</root>");
                        Cartera.Instancia.GuardarCarteraIBK(gestionCliente, cadena, Session["Login"].ToString());
                        respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                    }
                    if (gestionCliente == 9)
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
                                    valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                    //xml += (sheet[0, j].Value.Equals("FechaAsignacion") ? "CodCartera = 'VINTE" + DateTime.Now.ToString("yyyyMM") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FechaAsignacion") ? "FechaInicio = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' FechaFin = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Código") ? "CodigoCliente = '" + valor + "' CodCartera = 'CINTE" + DateTime.Now.ToString("yyyyMM") + "' FechaInicio = '" + DateTime.Now.ToString("yyyyMMdd") + "' FechaFin = '" + DateTime.Now.ToString("yyyyMMdd") + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NroProdOrig") ? "NroProdOrig = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("NroTC") ? "NroTarjeta = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Número de CJ") ? "NroCJ = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NomProd") ? "Producto = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NomSubProd") ? "SubProducto = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DescripMotivoBloqueo") ? "MotivoBloqueo = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("TramoClienteReal Inicial") ? "Tramo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Días de Mora") ? "IniDMProducto = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("IniDMCliente") ? "IniDMCliente = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("MinimoMN") ? "MinimoMN = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("MinimoME") ? "MinimoME = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("VencidoMN") ? "VencidoMN = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("VencidoME") ? "VencidoME = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("TotalVencido") ? "ToralVencido = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Importe_Cancelación") ? "ImporteCancelacion = '" + (valor.Equals("") ? "0" : valor) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Deuda_Total") ? "DeudaTotal = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Saldo Capital") ? "SaldoCapital = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Nombre_Empresa") ? "NombreEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("RUC") ? "RucEmpresa = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DIRECCION") ? "Direccion = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Dist") ? "Distrito = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Prov") ? "Provincia = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Dep") ? "Departamento = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Direccion_Empresa") ? "DireccionEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Distrito_Empresa") ? "DistritoEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Provincia_Empresa") ? "ProvinciaEmpresa = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Dep_Empresa") ? "DepartamentoEmpresa = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("SITUACION LABORAL") ? "SituacionLaboral = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("ESTATAL?") ? "Estatal = '" + (valor.Equals("Si") ? Convert.ToString(true) : Convert.ToString(false)) + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("RANGO_SUELDO") ? "RangoSueldo = '" + valor + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("Moneda") ? "Moneda = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FEC-DESEMB") && valor.Length > 0 ? "FechaPrestamo = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FEC-1ER-VENC") && valor.Length > 0 ? "FechaCuota = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CANT CUOTAS") ? "NroCuotas = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("DIRECC-CLIENTE") ? "DireccionCliente = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("IMP-CUO-ORD") ? "MontoCuota = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("FEC-PROX-VENC") && valor.Length > 0 ? "FechaProxVencimiento = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CUOTAS PAGADAS") ? "CuotasPagadas = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CUOTAS PENDIENTES") ? "CuotasPendientes = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("CUOTAS VENCIDAS") ? "CuotasVencidas = '" + valor + "' " : "");
                                    //xml += (sheet[0, j].Value.Equals("IMP-CANCELACION") ? "ImporteCancelacion = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                    xml += (sheet[0, j].Value.Equals("NroDoc") ? "NumeroDocumento = '" + valor + "' TipoDocumento = 'DN' " : "");
                                    xml += (sheet[0, j].Value.Equals("Nombre") ? "RazonSocial = '" + valor + "' " : "");
                                    //distrito = (distrito.Length == 0 ? (sheet[0, j].Value.Equals("Distrito") ? valor : "") : distrito);
                                    //provincia = (provincia.Length == 0 ? (sheet[0, j].Value.Equals("Provincia") ? valor : "") : provincia);
                                    //departamento = (departamento.Length == 0 ? (sheet[0, j].Value.Equals("Departamento") ? valor : "") : departamento);
                                    direccion = (direccion.Length == 0 ? (sheet[0, j].Value.Equals("Dir") ? valor : "") : direccion);

                                    //detallemoroso
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Casa") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Celular") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Trabajo") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("Telef_Referencia") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    //xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfDesconocido") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                    xmlDetalle += (sheet[0, j].Value.Equals("Email1") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '4' />" : "");
                                }
                            }
                            xmlDetalle += "<detalle Descripcion = '" + direccion + "' TipoDetalle = '2' />";
                            cadena.Append(xml + " >");
                            cadena.Append(xmlDetalle);
                            cadena.Append("</cartera>");
                        }
                        cadena.Append("</root>");
                        Cartera.Instancia.GuardarCarteraIBK(gestionCliente, cadena, Session["Login"].ToString());
                        respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                    }
                    else if (gestionCliente == 2)
                    {
                        C1XLBook book = new C1XLBook();
                        book.Load(file.InputStream);
                        XLSheet sheet = book.Sheets[0];

                        string xml = string.Empty;
                        string xmlDetalle = string.Empty;
                        string valor = string.Empty;
                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");

                        if (tipo == 1)
                        {
                            for (int i = 1; i < sheet.Rows.Count; i++)
                            {
                                xml = "<cartera ";
                                xmlDetalle = "";
                                string distrito = "", provincia = "", departamento = "", direccion = "";
                                for (int j = 0; j < sheet.Columns.Count; j++)
                                {
                                    if (sheet[0, j].Value != null)
                                    {
                                        valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
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
                                        xml += (sheet[0, j].Value.Equals("IniFIMProd") ? "IniFIMProd = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("IniFIMCliente") ? "IniFIMCliente = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("IniDMCliente") ? "IniDMCliente = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("Capital Producto Inicial") ? "CapitalProductoInicial = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("Capital Producto Actual") ? "CapitalProductoActual = '" + valor + "' " : "");
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
                                        xml += (sheet[0, j].Value.Equals("IMP-CUO-ORD") ? "MontoCuota = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("FEC-PROX-VENC") && valor.Length > 0 ? "FechaProxVencimiento = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("CUOTAS PAGADAS") ? "CuotasPagadas = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("CUOTAS PENDIENTES") ? "CuotasPendientes = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("CUOTAS VENCIDAS") ? "CuotasVencidas = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("IMP-CANCELACION") ? "ImporteCancelacion = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("DNI") ? "NumeroDocumento = '" + valor + "' TipoDocumento = 'DN' " : "");
                                        xml += (sheet[0, j].Value.Equals("Nombre") ? "RazonSocial = '" + valor + "' " : "");
                                        distrito = (distrito.Length == 0 ? (sheet[0, j].Value.Equals("Distrito") ? valor : "") : distrito);
                                        provincia = (provincia.Length == 0 ? (sheet[0, j].Value.Equals("Provincia") ? valor : "") : provincia);
                                        departamento = (departamento.Length == 0 ? (sheet[0, j].Value.Equals("Departamento") ? valor : "") : departamento);
                                        direccion = (direccion.Length == 0 ? (sheet[0, j].Value.Equals("Direccion") ? valor : "") : direccion);

                                        //detallemoroso
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfCasa") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfCelular") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfTrabajo") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfReferencia") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfDesconocido") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.Equals("Email") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '4' />" : "");
                                    }
                                }
                                xmlDetalle += "<detalle Descripcion = '" + direccion + "' TipoDetalle = '2' />";
                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCarteraIBK(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (tipo == 3) 
                        {
                            for (int i = 1; i < sheet.Rows.Count; i++)
                            {
                                xml = "<cartera ";
                                xmlDetalle = "";
                                for (int j = 0; j < sheet.Columns.Count; j++)
                                {
                                    if (sheet[0, j].Value != null)
                                    {
                                        valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                        
                                        xml += (sheet[0, j].Value.Equals("NroProd") ? "NroProducto = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("NroCJ") ? "NroCJ = '" + valor + "' " : "");
                                        
                                        xml += (sheet[0, j].Value.Equals("VencidoMN") ? "VencidoMN = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("VencidoME") ? "VencidoME = '" + valor + "' " : "");
                                    }
                                }
                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.ActualizarCarteraIBK(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (gestionCliente == 9)
                    {
                        C1XLBook book = new C1XLBook();
                        book.Load(file.InputStream);
                        XLSheet sheet = book.Sheets[0];

                        string xml = string.Empty;
                        string xmlDetalle = string.Empty;
                        string valor = string.Empty;
                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");

                        if (tipo == 1)
                        {
                            for (int i = 1; i < sheet.Rows.Count; i++)
                            {
                                xml = "<cartera ";
                                xmlDetalle = "";
                                string distrito = "", provincia = "", departamento = "", direccion = "";
                                for (int j = 0; j < sheet.Columns.Count; j++)
                                {
                                    if (sheet[0, j].Value != null)
                                    {

                                        valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                        xml += (sheet[0, j].Value.Equals("FechaReporte") ? "CodCartera = 'ENOSA" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("FechaReporte") ? "FechaInicio = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' FechaFin = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                 
                                        
                                        
                                        xml += (sheet[0, j].Value.Equals("IdNroServicio") ? "IdNroServicio = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("UltimoPeriodoFacturado") ? "UltimoPeriodoFacturado = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("IdRutaReparto") ? "IdRutaReparto = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("NroSecuenciaReparto") ? "NroSecuenciaReparto = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("RutaReparto") ? "RutaReparto = '" + valor + "' " : "");


                                        xml += (sheet[0, j].Value.Equals("IniFIMProd") ? "IniFIMProd = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("IniFIMCliente") ? "IniFIMCliente = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("ESTATAL?") ? "Estatal = '" + (valor.Equals("Si") ? Convert.ToString(true) : Convert.ToString(false)) + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("DIRECC-CLIENTE") ? "DireccionCliente = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("IMP-CUO-ORD") ? "MontoCuota = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("FEC-PROX-VENC") && valor.Length > 0 ? "FechaProxVencimiento = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("IMP-CANCELACION") ? "ImporteCancelacion = '" + (valor.Length == 0 ? "0" : valor.Replace(",", "")) + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("DNI") ? "NumeroDocumento = '" + valor + "' TipoDocumento = 'DN' " : "");
                                        xml += (sheet[0, j].Value.Equals("Nombre") ? "RazonSocial = '" + valor + "' " : "");
                                        distrito = (distrito.Length == 0 ? (sheet[0, j].Value.Equals("Distrito") ? valor : "") : distrito);
                                        provincia = (provincia.Length == 0 ? (sheet[0, j].Value.Equals("Provincia") ? valor : "") : provincia);
                                        departamento = (departamento.Length == 0 ? (sheet[0, j].Value.Equals("Departamento") ? valor : "") : departamento);
                                        direccion = (direccion.Length == 0 ? (sheet[0, j].Value.Equals("Direccion") ? valor : "") : direccion);

                                        //detallemoroso
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfCasa") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfCelular") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfTrabajo") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfReferencia") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.ToString().Substring(0, sheet[0, j].Value.ToString().Length - 2).Equals("TlfDesconocido") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '1' />" : "");
                                        xmlDetalle += (sheet[0, j].Value.Equals("Email") && valor.Length > 0 ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '4' />" : "");
                                    }
                                }
                                xmlDetalle += "<detalle Descripcion = '" + direccion + "' TipoDetalle = '2' />";
                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCarteraIBK(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (gestionCliente == 4)
                    {
                        MemoryStream ms = new MemoryStream();
                        file.InputStream.CopyTo(ms);
                        StreamReader r = new StreamReader(ms);

                        string xml = string.Empty;
                        string xmlDetalle = string.Empty;
                        string linea = "";
                        //string[] valores;
                        int contador = 10;
                        Dictionary<string, string> morosos = new Dictionary<string, string>();

                        r.BaseStream.Position = 0;

                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");

                        if (tipo == 1) 
                        {
                            while (!r.EndOfStream)
                            {
                                string via  = "", calle = "", numero = "" , direccion = "", direccionc = "";
                                string tipodoc = "", nrodoc = "", nombres = "";
                                xmlDetalle = "";
                                linea = r.ReadLine();
                                xml = "<cartera Cliente = '" + linea.Substring(0,10).Trim() + "' ";
                                xml += "Cuenta= '" + linea.Substring(10, 10).Trim() + "' ";
                                xml += "Inscripcion= '" + linea.Substring(20, 10).Trim() + "' ";
                                xml += "Telefono= '" + linea.Substring(30, 15).Trim() + "' ";
                                //Información Cartera
                                xml += "CodCartera= '" + linea.Substring(45, 30).Trim() + "' ";
                                xml += "FechaInicio= '" + linea.Substring(75, 10).Trim() + "' ";
                                xml += "FechaFin= '" + linea.Substring(85, 10).Trim() + "' ";
                                //Fin Información Cartera
                                xml += "COD_Estado_PC= '" + linea.Substring(115, 4).Trim() + "' ";
                                xml += "DES_Estado_PC= '" + linea.Substring(119, 10).Trim() + "' ";
                                //xml += "Nombre_Empresa= '" + linea.Substring(176, 30).Trim() + "' ";
                                xml += "S_TipoDocumento= '" + linea.Substring(214, 30).Trim() + "' ";
                                xml += "Negocio= '" + linea.Substring(289, 4).Trim() + "' ";
                                
                                xml += "S_NumeroDocumento= '" + linea.Substring(295, 13).Trim() + "' ";

                                xml += "FechaEmision= '" + linea.Substring(318, 10).Trim() + "' ";
                                xml += "FechaVencimiento= '" + linea.Substring(328, 10).Trim() + "' ";
                                xml += "MTO_Exigible= '" + linea.Substring(338, 12).Trim() + "' ";
                                xml += "MTO_Total= '" + linea.Substring(362, 12).Trim() + "' ";
                                xml += "Fecha_Alta_PC= '" + linea.Substring(410, 10).Trim() + "' ";

                                xml += (linea.Substring(420, 10).Trim() == "") ? "" : "Fecha_Baja_PC= '" + linea.Substring(420, 10).Trim() + "' ";
                                //xml += "Fecha_Baja_PC= '" + linea.Substring(420, 10).Trim() + "' ";
                                //Información Moroso

                                nombres = devuelveCadena(linea.Substring(430, 100).Trim().Replace("'", " "));
                                tipodoc = linea.Substring(530, 3).Trim();
                                if (tipodoc == "DNI")
                                {
                                    tipodoc = "DN";
                                }
                                else if (tipodoc == "RUC")
                                {
                                    tipodoc = "RU";
                                }
                                else if (tipodoc == "CEX")
                                {
                                    tipodoc = "CE";
                                }
                                else  tipodoc = "DN"; 
                                nrodoc = linea.Substring(533, 20).Trim();
                                string nrodoctemp = "";

                                if (nrodoc == ".")
                                {
                                    if (morosos.TryGetValue(nombres, out nrodoctemp))
                                    {
                                        xml += "RazonSocial= '" + nombres + "' ";
                                        xml += "TipoDocumento= '" + tipodoc + "' ";
                                        xml += "NumeroDocumento= '" + nrodoctemp + "' ";
                                    }
                                    else
                                    {
                                        nrodoctemp = "X" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + contador.ToString();
                                        contador++;
                                        xml += "RazonSocial= '" + nombres + "' ";
                                        xml += "TipoDocumento= '" + tipodoc + "' ";
                                        xml += "NumeroDocumento= '" + nrodoctemp + "' ";
                                        morosos.Add(nombres, nrodoctemp);
                                    }
                                }
                                else
                                {
                                    xml += "RazonSocial= '" + nombres + "' ";
                                    xml += "TipoDocumento= '" + tipodoc + "' ";
                                    xml += "NumeroDocumento= '" + nrodoc + "' ";
                                }


                                //xml += "RazonSocial= '" + nombres + "' ";
                                //xml += "TipoDocumento= '" + tipodoc + "' ";
                                //xml += "NumeroDocumento= '" + nrodoc + "' ";


                                //Fin Información Moroso


                                //Inicio Dirección
                                via = linea.Substring(553, 3).Trim();
                                calle = linea.Substring(556, 40).Trim().Replace("'", " ");
                                numero = linea.Substring(596, 6).Trim();
                                direccion = linea.Substring(602, 100).Trim().Replace("'", " ");
                                //Fin Dirección
                                xml += "Departamento= '" + linea.Substring(702, 20).Trim().Replace("'", " ") + "' "; //(Provincia)
                                //xml += "Area= '" + linea.Substring(722, 10).Trim().Replace("'", " ") + "' ";
                                xml += "Zonal= '" + linea.Substring(732, 3).Trim() + "' ";
                                xml += "Distrito= '" + linea.Substring(745, 30).Trim().Replace("'", " ") + "' "; //(Des_Sub_Localidad)
                                xml += "Provincia= '" + linea.Substring(853, 30).Trim().Replace("'", " ") + "' "; //(DES_Localidad)
                                xml += "Corte= '" + linea.Substring(1095, 10).Trim() + "' ";
                                xml += "Campana= '" + linea.Substring(1105, 50).Trim() + "' ";
                                //xml += "Telefono_Referencia= '" + linea.Substring(1185, 30).Trim() + "' ";

                                //xml += "Aplica_Financiamiento= '" + linea.Substring(1215, 2).Trim() + "' ";
                                //xml += "Financiamiento_Actual= '" + linea.Substring(1217, 50).Trim() + "' ";

                                xml += "Cluster= '" + linea.Substring(1517, 40).Trim() + "' ";
                                xml += "Tramo= '" + linea.Substring(1557, 40).Trim() + "' ";

                                //detalle de morosos
                                direccionc = via + " " + calle + " " + numero + " " + direccion;
                                xmlDetalle += "<detalle Descripcion = '" + linea.Substring(30, 15).Trim() + "' TipoDetalle = '1' />";
                                xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(direccionc.Trim()) + "' TipoDetalle = '2' />";
                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                            }
                            
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCarteraFija(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (gestionCliente == 7)
                    {
                        MemoryStream ms = new MemoryStream();
                        file.InputStream.CopyTo(ms);
                        StreamReader r = new StreamReader(ms);

                        string xml = string.Empty;
                        string xmlDetalle = string.Empty;
                        string linea = "";
                        //string[] valores;
                        int contador = 10;
                        Dictionary<string, string> morosos = new Dictionary<string, string>();

                        r.BaseStream.Position = 0;

                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");

                        if (tipo == 1)
                        {
                            while (!r.EndOfStream)
                            {
                                xmlDetalle = "";
                                linea = r.ReadLine();
                                //INICIO DETALLECARTERABBVA
                                xml = "<cartera CodCent = '" + linea.Substring(275, 8).Trim() + "' ";
                                xml += "NroProducto= '" + linea.Substring(257, 18).Trim() + "' ";
                                if (linea.Substring(821, 100).Trim() == "COMERCIAL")
                                {
                                    if (Convert.ToInt16(linea.Substring(435, 5).Trim()) < 9)
                                    {
                                        xml += "Tramo= 'T1' ";
                                    }
                                    else if (Convert.ToInt16(linea.Substring(435, 5).Trim()) < 16)
                                    {
                                        xml += "Tramo= 'T2' ";
                                    }
                                    else if (Convert.ToInt16(linea.Substring(435, 5).Trim()) < 31)
                                    {
                                        xml += "Tramo= 'T3' ";
                                    }
                                    else xml += "Tramo= 'T4' ";
                                }
                                else
                                {
                                    if (Convert.ToInt16(linea.Substring(435, 5).Trim()) < 31)
                                    {
                                        xml += "Tramo= 'T1' ";
                                    }
                                    else if (Convert.ToInt16(linea.Substring(435, 5).Trim()) < 61)
                                    {
                                        xml += "Tramo= 'T2' ";
                                    }
                                    else
                                    {
                                        xml += "Tramo= 'T3' ";
                                    }
                                }
                                xml += "Moneda= '" + linea.Substring(336, 3).Trim() + "' ";
                                xml += "DeudaVencida= '" + Convert.ToDouble(linea.Substring(339, 15).Trim())*0.01 + "' ";
                                xml += "FechaMora= '" + linea.Substring(425, 10).Trim() + "' ";
                                xml += "DiasMora= '" + linea.Substring(435, 5).Trim() + "' ";
                                xml += "Distrito= '" + linea.Substring(500, 30).Trim() + "' ";
                                xml += "TramoBBVA= '" + linea.Substring(530, 14).Trim() + "' ";
                                xml += "StatusBanco= '" + linea.Substring(921, 100).Trim() + "' ";
                                //FIN DETALLECARTERABBVA

                                //INICIO CARTERA
                                xml += "CodCartera= '" + Convert.ToDateTime(linea.Substring(701, 10).Trim()).ToString("yyyyMMdd") + "BBVA_Vencida' ";
                                xml += "FechaInicio= '" + Convert.ToDateTime(linea.Substring(701, 10).Trim()).ToString("yyyyMMdd") + "' ";
                                xml += "FechaFin= '" + Convert.ToDateTime(linea.Substring(701, 10).Trim()).ToString("yyyyMMdd") + "' ";
                                //FIN CARTERA

                                xml += "ZonalBBVA= '" + linea.Substring(9, 50).Trim() + "' ";
                                xml += "ZonaBBVA= '" + linea.Substring(159, 40).Trim() + "' ";
                                xml += "CodigoOficina= '" + linea.Substring(199, 4).Trim() + "' ";
                                xml += "Oficina= '" + linea.Substring(721, 100).Trim() + "' ";
                                xml += "Producto= '" + linea.Substring(821, 100).Trim() + "' ";
                                xml += "CodigoSubProducto= '" + linea.Substring(203, 4).Trim() + "' ";
                                xml += "SubProducto= '" + devuelveCadena(linea.Substring(207, 50).Trim()) + "' ";

                                //INICIO MOROSO
                                if (linea.Substring(544, 2).Trim() == "R")
                                {
                                    xml += "TipoDocumento= 'RU' ";
                                    xml += "NumeroDocumento= '" + linea.Substring(546, 15).Trim().Substring(4) + "' ";
                                    xml += "RazonSocial= '" + devuelveCadena(linea.Substring(286, 50).Trim()) + "' ";
                                }
                                else
                                {
                                    xml += "TipoDocumento= 'DN' ";
                                    xml += "NumeroDocumento= '" + linea.Substring(546, 15).Trim().Substring(7) + "' ";
                                    xml += "RazonSocial= '" + devuelveCadena(linea.Substring(286, 50).Trim()) + "' ";
                                }
                                //FIN MOROSO
                                                                                                
                                //detalle de morosos
                                if (linea.Substring(561, 3).Trim() != "\0" && linea.Substring(561, 3).Trim() != "" && linea.Substring(567, 10)!="0000000000")
                                {
                                    string numero1 = linea.Substring(567, 10).Trim().Substring(devuelvePosicion(linea.Substring(567, 10).Trim()));
                                    if (linea.Substring(561, 3).Trim() == "D" )
                                    {
                                        if (numero1.Substring(0, 1) == "#" || numero1.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero1) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(564, 3).Trim().Substring(1) + numero1 + "' TipoDetalle = '1' />";
                                    }
                                    else if (linea.Substring(561, 3).Trim() == "O")
                                    {
                                        if (numero1.Substring(0, 1) == "#" || numero1.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero1) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(564, 3).Trim().Substring(1) + numero1 + "' TipoDetalle = '1' />";
                                    }   else
                                        xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero1) + "' TipoDetalle = '1' />";
                                }
                                if (linea.Substring(581, 3).Trim() != "\0" && linea.Substring(581, 3).Trim() != "" && linea.Substring(587, 10) != "0000000000")
                                {
                                    string numero2 = linea.Substring(587, 10).Trim().Substring(devuelvePosicion(linea.Substring(587, 10).Trim()));
                                    if (linea.Substring(581, 3).Trim() == "D")
                                    {
                                        if (numero2.Substring(0, 1) == "#" || numero2.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero2) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(584, 3).Trim().Substring(1) + numero2 + "' TipoDetalle = '1' />";
                                    }
                                    else if (linea.Substring(581, 3).Trim() == "O")
                                    {
                                        if (numero2.Substring(0, 1) == "#" || numero2.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero2) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(584, 3).Trim().Substring(1) + numero2 + "' TipoDetalle = '1' />";
                                    }
                                    else
                                        xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero2) + "' TipoDetalle = '1' />";
                                }
                                if (linea.Substring(601, 3).Trim() != "\0" && linea.Substring(601, 3).Trim() != "" && linea.Substring(607, 10) != "0000000000")
                                {
                                    string numero3 = linea.Substring(607, 10).Trim().Substring(devuelvePosicion(linea.Substring(607, 10).Trim()));
                                    if (linea.Substring(601, 3).Trim() == "D")
                                    {
                                        if (numero3.Substring(0, 1) == "#" || numero3.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero3) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(604, 3).Trim().Substring(1) + numero3 + "' TipoDetalle = '1' />";
                                    }
                                    else if (linea.Substring(601, 3).Trim() == "O")
                                    {
                                        if (numero3.Substring(0, 1) == "#" || numero3.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero3) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(604, 3).Trim().Substring(1) + numero3 + "' TipoDetalle = '1' />";
                                    }
                                    else
                                        xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero3) + "' TipoDetalle = '1' />";
                                }
                                if (linea.Substring(621, 3).Trim() != "\0" && linea.Substring(621, 3).Trim() != "" && linea.Substring(627, 10) != "0000000000")
                                {
                                    string numero4 = linea.Substring(627, 10).Trim().Substring(devuelvePosicion(linea.Substring(627, 10).Trim()));
                                    if (linea.Substring(621, 3).Trim() == "D")
                                    {
                                        if (numero4.Substring(0, 1) == "#" || numero4.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero4) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(624, 3).Trim().Substring(1) + numero4 + "' TipoDetalle = '1' />";
                                    }
                                    else if (linea.Substring(621, 3).Trim() == "O")
                                    {
                                        if (numero4.Substring(0, 1) == "#" || numero4.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero4) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(624, 3).Trim().Substring(1) + numero4 + "' TipoDetalle = '1' />";
                                    }
                                    else
                                        xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero4) + "' TipoDetalle = '1' />";
                                }
                                if (linea.Substring(641, 3).Trim() != "\0" && linea.Substring(641, 3).Trim() != "" && linea.Substring(647, 10) != "0000000000")
                                {
                                    string numero5 = linea.Substring(647, 10).Trim().Substring(devuelvePosicion(linea.Substring(647, 10).Trim()));
                                    if (linea.Substring(641, 3).Trim() == "D")
                                    {
                                        if (numero5.Substring(0, 1) == "#" || numero5.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero5) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(644, 3).Trim().Substring(1) + numero5 + "' TipoDetalle = '1' />";
                                    }
                                    else if (linea.Substring(641, 3).Trim() == "O")
                                    {
                                        if (numero5.Substring(0, 1) == "#" || numero5.Substring(0, 1) == "*")
                                            xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero5) + "' TipoDetalle = '1' />";
                                        else
                                            xmlDetalle += "<detalle Descripcion = '" + linea.Substring(644, 3).Trim().Substring(1) + numero5 + "' TipoDetalle = '1' />";
                                    }
                                    else
                                        xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(numero5) + "' TipoDetalle = '1' />";
                                }
                                xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(linea.Substring(354, 50).Trim()) + "' TipoDetalle = '2' />";
                                
                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                            }

                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCarteraBBVA(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (gestionCliente==1)
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
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (tipo == 2)
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
                            
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (gestionCliente == 8)
                    { 
                    C1XLBook book = new C1XLBook();
                        book.Load(file.InputStream);
                        XLSheet sheet = book.Sheets[0];

                        string xml = string.Empty;
                        string xmlDetalle = string.Empty;
                        string valor = string.Empty;
                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");

                        if (tipo == 1)
                        {
                            for (int i = 1; i < sheet.Rows.Count; i++)
                            {
                                xml = "<cartera ";
                                xmlDetalle = "";
                                string codigo = "", telefono1 = "", telefono2 = "", telefono3 = "", telefono4 = "", telefono5 = "", departamento = "", direccion = "", prefijo1 = "", prefijo2 = "", prefijo3 = "", prefijo4 = "", prefijo5 = "";

                                for (int j = 0; j < sheet.Columns.Count; j++)
                                {
                                    if (sheet[0, j].Value != null)
                                    {
                                        valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                        
                                        xml += (sheet[0, j].Value.Equals("fproceso") ? "CodCartera = 'BBVAEXJ" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("fproceso") ? "FechaInicio = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' FechaFin = '" + new DateTime(Convert.ToDateTime(sheet[i, j].Value).Year, Convert.ToDateTime(sheet[i, j].Value).Month, DateTime.DaysInMonth(Convert.ToDateTime(sheet[i, j].Value).Year, Convert.ToDateTime(sheet[i, j].Value).Month)).ToString("yyyyMMdd") + /*Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") +*/ "' " : "");

                                        xml += (sheet[0, j].Value.Equals("nomb_gest") ? "NombGest = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("territorio") ? "ZonaBBVA = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("tipodoc") ? valor.Equals("L") ? "TipoDocumento = 'DN' " : "TipoDocumento = 'RU' " : "");
                                        xml += (sheet[0, j].Value.Equals("nrodoc") ? "NumeroDocumento = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("nombres") ? "RazonSocial = '" + valor + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("contrato") ? "NroProducto = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("codcent") ? "CodCent = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("coddoca") ? "CodDoca = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("uga") ? "UGA = '" + valor + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("tramo") ? "Tramo = '" + valor + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("agencia") ? "ZonalBBVA = '" + valor + "' " : "");
                                        //Inicio Oficina
                                        xml += (sheet[0, j].Value.Equals("oficina") ? "CodigoOficina = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("nomb_of") ? "Oficina = '" + valor + "' " : "");
                                        //Fin Oficina

                                        xml += (sheet[0, j].Value.Equals("saldo") ? "Saldo = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("alcambio") ? "AlCambio = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("liq_total") ? "LiqTotal = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("moneda") ? "Moneda = '" + valor + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("fecha_venc") ? "FechaMora = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("diasvenc") ? "DiasMora = '" + valor + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("producto") ? "Producto = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("descpro") ? "SubProducto = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("subprod") ? "CodigoSubProducto = '" + valor + "' " : "");

                                        xml += (sheet[0, j].Value.Equals("dpto_ubig") ? "Departamento = '" + valor + "' " : "");
                                        xml += (sheet[0, j].Value.Equals("dist_prov") ? "Distrito = '" + valor + "' " : "");

                                        
                                        
                                        //detallemoroso

                                        xmlDetalle += (sheet[0, j].Value.Equals("direccion") ? "<detalle Descripcion = '" + valor + "' TipoDetalle = '2' />" : "");

                                        if (sheet[0, j].Value.Equals("fono1"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "0000000"))
                                            {
                                                telefono1 = valor.Substring(devuelvePosicion(valor)) ;
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("fono2"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "0000000"))
                                            {
                                                telefono2 = valor.Substring(devuelvePosicion(valor));
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("fono3"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "0000000"))
                                            {
                                                telefono3 = valor.Substring(devuelvePosicion(valor));
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("fono4"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "0000000"))
                                            {
                                                telefono4 = valor.Substring(devuelvePosicion(valor));
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("fono5"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "0000000"))
                                            {
                                                telefono5 = valor.Substring(devuelvePosicion(valor));
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("prefijo1"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "000"))
                                            {
                                                prefijo1 = valor;
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("prefijo2"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "000"))
                                            {
                                                prefijo2 = valor;
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("prefijo3"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "000"))
                                            {
                                                prefijo3 = valor;
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("prefijo4"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "000"))
                                            {
                                                prefijo4 = valor;
                                            }
                                        }

                                        if (sheet[0, j].Value.Equals("prefijo5"))
                                        {
                                            if (!(valor.Length == 0 || valor.Trim() == "" || valor.Trim() == "00" || valor.Trim() == "000"))
                                            {
                                                prefijo5 = valor;
                                            }
                                        }


                                    }
                                }
                                string telefonocompleto1 = "";
                                string telefonocompleto2 = "";
                                string telefonocompleto3 = "";
                                string telefonocompleto4 = "";
                                string telefonocompleto5 = "";
                                if (telefono1 != "")
                                {
                                    telefonocompleto1 += telefono1;
                                    if (prefijo1 != "") telefonocompleto1 = prefijo1 + "-" + telefonocompleto1;
                                    xmlDetalle += "<detalle Descripcion = '" + telefonocompleto1 + "' TipoDetalle = '1' />";
                                }
                                if (telefono2 != "")
                                {
                                    telefonocompleto2 += telefono2;
                                    if (prefijo2 != "") telefonocompleto2 = prefijo2 + "-" + telefonocompleto2;
                                    xmlDetalle += "<detalle Descripcion = '" + telefonocompleto2 + "' TipoDetalle = '1' />";
                                }
                                if (telefono3 != "") { 
                                    telefonocompleto3 += telefono3;
                                    if (prefijo3 != "") telefonocompleto3 = prefijo3 + "-" + telefonocompleto3;
                                    xmlDetalle += "<detalle Descripcion = '" + telefonocompleto3 + "' TipoDetalle = '1' />";
                                }
                                if (telefono4 != "") {
                                    telefonocompleto4 += telefono4;
                                    if (prefijo4 != "") telefonocompleto4 = prefijo4 + "-" + telefonocompleto4;
                                    xmlDetalle += "<detalle Descripcion = '" + telefonocompleto4 + "' TipoDetalle = '1' />";
                                }
                                if (telefono5 != "")
                                {
                                    telefonocompleto5 += telefono5;
                                    if (prefijo5 != "") telefonocompleto5 = prefijo5 + "-" + telefonocompleto5;
                                    xmlDetalle += "<detalle Descripcion = '" + telefonocompleto5 + "' TipoDetalle = '1' />";
                                }

                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCarteraBBVA(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (gestionCliente == 6)
                    {
                        MemoryStream ms = new MemoryStream();
                        file.InputStream.CopyTo(ms);
                        StreamReader r = new StreamReader(ms);

                        string xml;
                        string xmlDetalle = string.Empty;
                        string linea = "";
                        string direccion = "";
                        string[] valores;

                        double deudatotal = 0;

                        r.BaseStream.Position = 0;

                        StringBuilder cadena = new StringBuilder();
                        cadena.Append("<root>");
                        if (tipo == 1)
                        {
                            while (!r.EndOfStream)
                            {
                                xmlDetalle = "";
                                deudatotal = 0;
                                linea = r.ReadLine();
                                valores = linea.Split(new char[] { '|' });

                                if (valores.Length != 57)
                                {
                                    throw new Exception("La estructura del archivo no es correcta.");
                                }
                                xml = "<cartera Anexo = '" + valores[0].Trim().Replace("C", "") + "' ";
                                xml += "Telefono = '" + valores[1].Trim().Trim().Replace("C", "") + "' ";
                                xml += "Cliente = '" + valores[2].Trim().Trim().Replace("C", "") + "' ";
                                xml += "Cluster = '" + valores[4].Trim() + "' ";
                                xml += "Tramo = '" + valores[6].Trim() + "' ";
                                xml += "CodCartera = '" + valores[9].Trim() + "' ";
                                xml += "FechaInicio = '" + Convert.ToDateTime(valores[10]).ToString("yyyyMMdd") + "' ";
                                xml += "FechaFin = '" + Convert.ToDateTime(valores[11]).ToString("yyyyMMdd") + "' ";
                                xml += "RazonSocial = '" + devuelveCadena(valores[16].Trim()) + "' ";
                                xml += "TipoCliente = '" + valores[17].Trim() + "' ";
                                xml += "DesPlan = '" + valores[19].Trim() + "' ";
                                xml += "GrupoPlan = '" + valores[20].Trim() + "' ";
                                xml += "MontoVencidoSol = '" + Convert.ToDouble(valores[22].Trim()) + "' ";
                                xml += "MontoVencidoDol = '" + Convert.ToDouble(valores[23].Trim()) + "' ";
                                xml += "MontoPorVencerSol = '" + Convert.ToDouble(valores[24].Trim()) + "' ";
                                xml += "MontoPorVencerDol = '" + Convert.ToDouble(valores[25].Trim()) + "' ";
                                deudatotal = Convert.ToDouble(valores[22].Trim()) + Convert.ToDouble(valores[23].Trim()) * 2.9 + Convert.ToDouble(valores[24].Trim()) + Convert.ToDouble(valores[25].Trim()) * 2.9;
                                xml += "DeudaTotal = '" + deudatotal + "' ";
                                xml += "Departamento = '" + valores[27].Trim() + "' ";
                                xml += "Provincia = '" + valores[28].Trim() + "' ";
                                xml += "Distrito = '" + valores[29].Trim() + "' ";
                                xml += "Zonal = '" + valores[30].Trim() + "' ";
                                xml += "Segmento = '" + valores[36].Trim() + "' ";
                                if (!(valores[41].ToString().Trim() == "\0" || valores[41].ToString().Trim() == ""))
                                {
                                    xml += "PersonaSolicCorte = '" + devuelveCadena(valores[41].Trim()) + "' ";
                                }
                                if (!(valores[42].ToString().Trim() == "\0" || valores[42].ToString().Trim() == ""))
                                {
                                    xml += "TelefonoPersonaSolicCorte = '" + devuelveCadena(valores[42].Trim()) + "' ";
                                }
                                if (!(valores[43].ToString().Trim() == "\0" || valores[43].ToString().Trim() == ""))
                                {
                                    xml += "FechaCorteRobo = '" + Convert.ToDateTime(valores[43]).ToString("yyyyMMdd") + "' ";
                                }
                                if (!(valores[44].ToString().Trim() == "\0" || valores[44].ToString().Trim() == ""))
                                {
                                    xml += "RepresentanteLegal = '" + devuelveCadena(valores[44].Trim()) + "' ";
                                }
                                if (!(valores[49].ToString().Trim() == "\0" || valores[49].ToString().Trim() == ""))
                                {
                                    xml += "Canal = '" + devuelveCadena(valores[49].Trim()) + "' ";
                                }
                                if (!(valores[50].ToString().Trim() == "\0" || valores[50].ToString().Trim() == ""))
                                {
                                    xml += "Entidad = '" + devuelveCadena(valores[50].Trim()) + "' ";
                                }


                                /*INICIO REGISTRO MOROSOS!!!*/
                                if (valores[32].ToString().Trim() == "\0" || valores[32].ToString().Trim() == "")
                                {
                                    xml += "TipoDocumento = 'RU' ";
                                    xml += "NumeroDocumento = '" + valores[34].Trim() + "' ";
                                }
                                else if (valores[32].ToString().Trim() == "RUC")
                                {
                                    if (valores[34].ToString().Trim() == "\0" || valores[34].ToString().Trim() == "")
                                    {
                                        if (valores[33].Trim().Substring(0, 1) == "#")
                                        {
                                            xml += "TipoDocumento = 'RU' ";
                                            xml += "NumeroDocumento = '" + valores[33].Trim().Substring(2) + "' ";
                                        }
                                        else
                                        {
                                            xml += "TipoDocumento = 'RU' ";
                                            xml += "NumeroDocumento = '" + valores[33].Trim() + "' ";
                                        }

                                    }
                                    else
                                    {
                                        xml += "TipoDocumento = 'RU' ";
                                        xml += "NumeroDocumento = '" + valores[34].Trim() + "' ";
                                    }
                                }
                                else
                                {
                                    if (valores[33].Trim().Length > 1)
                                    {
                                        xml += "TipoDocumento = 'DN' ";
                                        xml += "NumeroDocumento = '" + valores[33].Trim().Substring(2) + "' ";
                                    }
                                    else
                                    {
                                        xml += "TipoDocumento = 'RU' ";
                                        xml += "NumeroDocumento = '" + valores[34].Trim().Substring(2) + "' ";
                                    }

                                }
                                xml += "FechaVencMin = '" + Convert.ToDateTime(valores[39]).ToString("yyyyMMdd") + "' ";
                                xml += "FechaAlta = '" + Convert.ToDateTime(valores[35]).ToString("yyyyMMdd") + "' ";


                                xmlDetalle += "<detalle Descripcion = '" + valores[1].Trim() + "' TipoDetalle = '1' />";

                                if (!(valores[26].ToString().Trim() == "\0" || valores[26].ToString().Trim() == ""))
                                {
                                    xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(valores[26].Trim()) + "' TipoDetalle = '2' />";
                                }


                                if (!(valores[37].ToString().Trim() == "\0" || valores[37].ToString().Trim() == ""))
                                {
                                    xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(valores[37].Trim()) + "' TipoDetalle = '1' />";
                                }

                                if (!(valores[38].ToString().Trim() == "\0" || valores[38].ToString().Trim() == ""))
                                {
                                    xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(valores[38].Trim()) + "' TipoDetalle = '1' />";
                                }

                                if (!(valores[40].ToString().Trim() == "\0" || valores[40].ToString().Trim() == ""))
                                {
                                    xmlDetalle += "<detalle Descripcion = '" + devuelveCadena(valores[40].Trim().Replace("C", "#").Replace("B", "*")) + "' TipoDetalle = '1' />";
                                }
                                if (valores[45].ToString().Trim() == "Ofrecer Descuento")
                                {
                                    xml += "OfrecerDescuento = '1' ";
                                }
                                else
                                {
                                    xml += "OfrecerDescuento = '0' ";
                                }
                                if (valores[46].ToString().Trim() == "Tiene Financiamiento")
                                {
                                    xml += "ConFinanciamiento = '1' ";
                                }
                                else
                                {
                                    xml += "ConFinanciamiento = '0' ";
                                }

                                cadena.Append(xml + " >");
                                cadena.Append(xmlDetalle);
                                cadena.Append("</cartera>");
                                contadorlinea++;
                                //cadena.Append(xml);
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarCarteraMovil(gestionCliente, cadena, Session["Login"].ToString());
                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (tipo == 2)
                        {
                            while (!r.EndOfStream)
                            {
                                linea = r.ReadLine();
                                valores = linea.Split(new char[] { '|' });

                                if (valores.Length != 13)
                                {
                                    throw new Exception("La estructura del archivo no es correcta.");
                                }
                                xml = "<cartera CodCartera = '" + valores[1].Trim() + "' ";
                                xml += "Anexo = '" + valores[2].Trim() + "' ";
                                xml += "Zonal = '" + valores[3].Trim() + "' ";
                                xml += "TipoDeta = '" + devuelveCadena(valores[4].Trim()) + "' ";
                                xml += "DescripcionDoc = '" + devuelveCadena(valores[5].Trim()) + "' ";
                                xml += "NDoc = '" + valores[6].Trim() + "' ";
                                xml += "LDoc = '" + devuelveCadena(valores[7].Trim()) + "' ";
                                xml += "FechaVencimiento = '" + Convert.ToDateTime(valores[8]).ToString("yyyyMMdd") + "' ";
                                if (valores[9].Trim() == "L")
                                {
                                    xml += "Deuda = '" + valores[10].Trim() + "' ";
                                }
                                else
                                {
                                    xml += "Deuda = '" + Convert.ToDouble(valores[10].Trim()) * 2.9 + "' ";
                                }

                                xml += "LCA = '" + valores[11].Trim() + "' />";

                                cadena.Append(xml);
                            }
                            cadena.Append("</root>");
                            Cartera.Instancia.GuardarDetalleCarteraMovil(gestionCliente, cadena, Session["Login"].ToString());

                            respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return respuestaJson;
                }
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message+" "+contadorlinea.ToString() }, JsonRequestBehavior.AllowGet);
                }
            
        }

        public JsonResult CargarSectores(HttpPostedFileBase file)
        {
            JsonResult respuestaJson = new JsonResult();
            try
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
                    for (int j = 0; j < sheet.Columns.Count; j++)
                    {
                        if (sheet[0, j].Value != null)
                        {
                            valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                            
                            xml += (sheet[0, j].Value.Equals("Sector") ? "Sector = '" + valor + "' " : "");
                            xml += (sheet[0, j].Value.Equals("DetalleMoroso") ? "DetalleMoroso = '" + valor + "' " : "");

                        }
                    }
                    cadena.Append(xml + " >");
                    cadena.Append("</cartera>");

                }
                cadena.Append("</root>");
                Cartera.Instancia.GuardarSectores(cadena, Session["Login"].ToString());
                respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return respuestaJson;
        }

        private int devuelvePosicion(string Cadena) 
        {
            char[] chars = HttpUtility.HtmlEncode(Cadena).ToCharArray();
            int contador = 0;
            foreach (char c in chars)
            {
                if (c == '0') {
                    contador++;}
                else return contador;
            }
            return contador;
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

            XLStyle style = new XLStyle(book);
            style.BackColor = System.Drawing.Color.RoyalBlue;
            style.ForeColor = System.Drawing.Color.White;
            style.Font = new System.Drawing.Font("Arial", 10,System.Drawing.FontStyle.Bold);
            
            //style.WordWrap = true;
            XLStyle fechaStyle = new XLStyle(book);
            fechaStyle.Format = "dd/MM/yyyy";
            //fechaStyle.WordWrap = true;

            XLStyle detalle = new XLStyle(book);
            detalle.WordWrap = true;

            XLStyle doubleStyle = new XLStyle(book);
            doubleStyle.Format = "#,##0.00";

            XLStyle pagoStyle = new XLStyle(book);
            pagoStyle.Format = "#,##0.00";
            pagoStyle.ForeColor = System.Drawing.Color.Green;

            XLStyle pagoCeroStyle = new XLStyle(book);
            pagoCeroStyle.Format = "#,##0.00";
            pagoCeroStyle.ForeColor = System.Drawing.Color.Red;
            pagoCeroStyle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);

            XLStyle SaldoCeroStyle = new XLStyle(book);
            SaldoCeroStyle.Format = "#,##0.00";
            SaldoCeroStyle.ForeColor = System.Drawing.Color.Green;
            SaldoCeroStyle.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);

            int c = 0;
            foreach (DataColumn item in dt.Columns)
            {
                XLCell cell = sheet[0, col + c];
                cell.Value = item.ColumnName;
                cell.Style=  style;
                int r = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    cell = sheet[row + r, col + c];
                    if (item.DataType == typeof(DateTime))
                        //cell.Value = Convert.ToDateTime(dr[item.Ordinal]).ToString("dd/MM/yyyy");
                        cell.Value=dr[item.Ordinal]==DBNull.Value? "":Convert.ToDateTime(dr[item.Ordinal]).ToString("dd/MM/yyyy");
                    else if (nombreHoja.Substring(0, 5).ToUpper() == "CABLE" && item.ColumnName == "Saldo")
                    {
                        cell.Value = Convert.ToDouble(dr[item.Ordinal]);
                        if (Convert.ToDouble(cell.Value) == 0)
                            cell.Style = SaldoCeroStyle;
                        else
                            cell.Style = doubleStyle;
                    }
                    else if (item.ColumnName.Contains("DeudaTotal") || item.ColumnName.Contains("Pago") || item.ColumnName == "Reclamo" || item.ColumnName == "NotaCredito" || item.ColumnName == "Saldo")
                    {
                        cell.Value = Convert.ToDouble(dr[item.Ordinal]);
                    }
                    else if (item.ColumnName.Contains("NroGestiones") || item.ColumnName.Contains("NroLineas"))
                    {
                        cell.Value = Convert.ToInt32(dr[item.Ordinal]);
                       
                    }
                    else if (item.ColumnName.Contains("Fecha") || item.ColumnName.Contains("UltimaGestion"))
                    {
                        cell.Value = dr[item.Ordinal] == DBNull.Value ? "" : dr[item.Ordinal].ToString();
                        if (cell.Value.ToString() != "") cell.Value = Convert.ToDateTime(dr[item.Ordinal]);
                        cell.Style = fechaStyle;
                        sheet.Columns[col + c].Width = 1150;
                    }
                    else if (nombreHoja.Substring(0, 5).ToUpper() == "CABLE" && item.ColumnName == "PagoTotal")
                    {
                        cell.Value = Convert.ToDouble(dr[item.Ordinal]);
                        if (Convert.ToDouble(cell.Value) == 0) 
                            cell.Style = pagoCeroStyle; 
                        else 
                            cell.Style = pagoStyle;
                    }
                    
                    else if (nombreHoja.Substring(0, 5).ToUpper() == "CABLE" && item.ColumnName.Contains("MontoPromesa"))
                    {
                        cell.Value = dr[item.Ordinal] == DBNull.Value ? "" : dr[item.Ordinal].ToString();
                        if (cell.Value.ToString() != "") cell.Value = Convert.ToDouble(dr[item.Ordinal]);
                        cell.Style = doubleStyle;
                    }
                    else if (item.ColumnName.Contains("DetalleDeuda"))
                    {
                        cell.Value = dr[item.Ordinal];
                        sheet.Columns[col + c].Width = 3000;
                        cell.Style = detalle;
                    }
                    else
                        cell.Value = dr[item.Ordinal];
                    r++;
                }
                c++;
            }
            if (nombreHoja.Substring(0, 7).ToUpper() == "CABLE-M")
                sheet.Columns[c-1].Width = 2500;

            MemoryStream ms = new MemoryStream();
            book.Save(ms, FileFormat.Biff8);

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
                    //dr2["Sector"]= "\'" + dr2["Sector"];
                    //dr2["DeudaTotal"] = Convert.ToDouble(dr2["DeudaTotal"]);
                    //dr2["PagoTotal"] = Convert.ToDouble(dr2["PagoTotal"]);
                    //dr2["FechaPromesa"] = Convert.ToDateTime(dr2["FechaPromesa"]);
                    dtFinal.Rows.Add(dr2);
                    contador += 1;
                }
                else 
                {
                    if (dr["Servicio"].ToString() == dtFinal.Rows[contador]["Servicio"].ToString())
                    {
                        dtFinal.Rows[contador]["DetalleDeuda"] += "\r\n" + dr["DetalleDeuda"].ToString();
                    }
                    else
                    {
                        //dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                        for (int i = 0; i <= totalColumnas; i++)
                        {
                            dr2[i] = dr[i];
                        }
                        //dr2["Sector"] = "\'" + dr2["Sector"];

                        dtFinal.Rows.Add(dr2);
                        contador += 1;
                    }
                    //if (contadorFilas == nroFilas)
                    //{
                    //dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                    //}
                }
            }
            return dtFinal;
        }

        private DataTable JuntarDetalleFija(DataTable dt)
        {
            DataTable dtFinal = new DataTable();
            CrearTablaDetalleDeudaFija(dtFinal);
            int nroFilas = dt.Rows.Count;
            int contador = -1;
            int contadorFilas = 0;
            int totalColumnas = dtFinal.Columns.Count - 1;

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
                    //dr2["Sector"]= "\'" + dr2["Sector"];
                    //dr2["DeudaTotal"] = Convert.ToDouble(dr2["DeudaTotal"]);
                    //dr2["DeudaExigible"] = Convert.ToDouble(dr2["DeudaExigible"]);
                    //dr2["FechaPromesa"] = Convert.ToDateTime(dr2["FechaPromesa"]);
                    dtFinal.Rows.Add(dr2);
                    contador += 1;
                }
                else
                {
                    if (dr["Telefono"].ToString() == dtFinal.Rows[contador]["Telefono"].ToString())
                    {
                        dtFinal.Rows[contador]["DetalleDeuda"] += "\r\n" + dr["DetalleDeuda"].ToString();
                    }
                    else
                    {
                        //dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                        for (int i = 0; i <= totalColumnas; i++)
                        {
                            dr2[i] = dr[i];
                        }
                        //dr2["Sector"] = "\'" + dr2["Sector"];

                        dtFinal.Rows.Add(dr2);
                        contador += 1;
                    }
                    //if (contadorFilas == nroFilas)
                    //{
                    //dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                    //}
                }
            }
            return dtFinal;
        }

        private DataTable JuntarDetalleMovil(DataTable dt)
        {
            DataTable dtFinal = new DataTable();
            CrearTablaDetalleDeudaMovil(dtFinal);
            int nroFilas = dt.Rows.Count;
            int contador = -1;
            int contadorFilas = 0;
            int totalColumnas = dtFinal.Columns.Count - 1;

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
                    //dr2["Sector"]= "\'" + dr2["Sector"];
                    //dr2["DeudaTotal"] = Convert.ToDouble(dr2["DeudaTotal"]);
                    //dr2["DeudaExigible"] = Convert.ToDouble(dr2["DeudaExigible"]);
                    //dr2["FechaPromesa"] = Convert.ToDateTime(dr2["FechaPromesa"]);
                    dtFinal.Rows.Add(dr2);
                    contador += 1;
                }
                else
                {
                    if (dr["Telefono"].ToString() == dtFinal.Rows[contador]["Telefono"].ToString())
                    {
                        dtFinal.Rows[contador]["DetalleDeuda"] += "\r\n" + dr["DetalleDeuda"].ToString();
                    }
                    else
                    {
                        //dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                        for (int i = 0; i <= totalColumnas; i++)
                        {
                            dr2[i] = dr[i];
                        }
                        //dr2["Sector"] = "\'" + dr2["Sector"];

                        dtFinal.Rows.Add(dr2);
                        contador += 1;
                    }
                    //if (contadorFilas == nroFilas)
                    //{
                    //dtFinal.Rows[contador]["DetalleDeuda"] = "\"" + dtFinal.Rows[contador]["DetalleDeuda"].ToString() + "\"";
                    //}
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
            dt.Columns.Add("NroLineas");//20
            dt.Columns.Add("DeudaTotalCliente");//19
            dt.Columns.Add("DeudaTotalLinea");//20
            dt.Columns.Add("Segmento");//20
            dt.Columns.Add("PagoTotal");//20
            dt.Columns.Add("Reclamo");//20
            dt.Columns.Add("Saldo");//21
            dt.Columns.Add("EstadoGestionCall");//20
            dt.Columns.Add("EstadoGestionCampo");//20
            dt.Columns.Add("UltimaGestionCall");//20
            dt.Columns.Add("UltimaGestionCampo");//20
            dt.Columns.Add("NroGestionesCall");//20
            dt.Columns.Add("NroGestionesCampo");//20
            dt.Columns.Add("MejorIncidenciaCall");//20
            dt.Columns.Add("MejorIncidenciaCampo");//20
            dt.Columns.Add("MejorObservacionCall");//20
            dt.Columns.Add("MejorObservacionCampo");//20
            //dt.Columns.Add("ContactadoCall");//20
            //dt.Columns.Add("ContactadoCampo");//20
            dt.Columns.Add("FechaPromesaCall");//20
            dt.Columns.Add("FechaPromesaCampo");//20
            dt.Columns.Add("DetalleDeuda");//32
        }

        private void CrearTablaDetalleDeudaFija(DataTable dt)
        {
            dt.Columns.Add("DCliente");//0
            dt.Columns.Add("DGestionCliente");//1
            dt.Columns.Add("CodCartera");//2
            dt.Columns.Add("FechaInicio");
            dt.Columns.Add("FechaFin");
            dt.Columns.Add("Zonal");//3
            dt.Columns.Add("Departamento");//4
            dt.Columns.Add("Provincia");//5
            dt.Columns.Add("Distrito");//6
            dt.Columns.Add("Cluster");//7
            dt.Columns.Add("Tramo");//8
            dt.Columns.Add("Telefono");//9
            dt.Columns.Add("Cuenta");//10
            dt.Columns.Add("Inscripcion");//11
            dt.Columns.Add("CodCliente");//12
            dt.Columns.Add("NumeroDocumento");//13
            dt.Columns.Add("DMoroso");//14
            dt.Columns.Add("Sector");//15
            dt.Columns.Add("DetalleMoroso");//15
            dt.Columns.Add("DireccionBase");//16
            dt.Columns.Add("DireccionUbicada");//17
            dt.Columns.Add("DireccionPorVerificar");//18
            dt.Columns.Add("NroLineas");//19
            dt.Columns.Add("DeudaTotalCliente");//19
            dt.Columns.Add("DeudaTotalLinea");//20
            dt.Columns.Add("Segmento");//20
            dt.Columns.Add("PagoTotal");//20
            dt.Columns.Add("Saldo");//21
            dt.Columns.Add("EstadoGestionCall");//20
            dt.Columns.Add("EstadoGestionCampo");//20
            dt.Columns.Add("UltimaGestionCall");//20
            dt.Columns.Add("UltimaGestionCampo");//20
            dt.Columns.Add("NroGestionesCall");//20
            dt.Columns.Add("NroGestionesCampo");//20
            dt.Columns.Add("MejorIncidenciaCall");//20
            dt.Columns.Add("MejorIncidenciaCampo");//20
            dt.Columns.Add("MejorObservacionCall");//20
            dt.Columns.Add("MejorObservacionCampo");//20
            //dt.Columns.Add("ContactadoCall");//20
            //dt.Columns.Add("ContactadoCampo");//20
            dt.Columns.Add("FechaPromesaCall");//20
            dt.Columns.Add("FechaPromesaCampo");//20
            dt.Columns.Add("DetalleDeuda");//22
        }
        private void CrearTablaDetalleDeudaMovil(DataTable dt)
        {
            dt.Columns.Add("DCliente");//0
            dt.Columns.Add("DGestionCliente");//1
            dt.Columns.Add("CodCartera");//2
            dt.Columns.Add("FechaInicio");//3
            dt.Columns.Add("FechaFin");//4
            dt.Columns.Add("Zonal");//5
            dt.Columns.Add("Departamento");//6
            dt.Columns.Add("Provincia");//7
            dt.Columns.Add("Distrito");//8
            dt.Columns.Add("Cluster");//9
            dt.Columns.Add("GrupoPlan");//10
            dt.Columns.Add("FechaVencMin");//11
            dt.Columns.Add("Tramo");//12
            dt.Columns.Add("Telefono");//13
            dt.Columns.Add("Anexo");//14
            dt.Columns.Add("CodCliente");//15
            dt.Columns.Add("NumeroDocumento");//16
            dt.Columns.Add("DMoroso");//17
            dt.Columns.Add("PersonaSolicCorte");//18
            dt.Columns.Add("TelefonoPersonaSolicCorte");//19
            dt.Columns.Add("FechaCorteRobo");//20
            dt.Columns.Add("RepresentanteLegal");//21
            dt.Columns.Add("Entidad");//22
            dt.Columns.Add("CanalVenta");//23
            dt.Columns.Add("Sector");//24
            dt.Columns.Add("DetalleMoroso");//25
            dt.Columns.Add("DireccionBase");//26
            dt.Columns.Add("DireccionUbicada");//27
            dt.Columns.Add("DireccionPorVerificar");//28
            dt.Columns.Add("NroLineas");//29
            dt.Columns.Add("DeudaTotalCliente");//30
            dt.Columns.Add("DeudaTotalLinea");//31
            dt.Columns.Add("Segmento");//32
            dt.Columns.Add("Pago");//33
            dt.Columns.Add("Reclamo");//34
            dt.Columns.Add("NotaCredito");//35
            dt.Columns.Add("Saldo");//36
            dt.Columns.Add("OfrecerDescuento");//37
            dt.Columns.Add("EstadoGestionCall");//38
            dt.Columns.Add("estadoGestionCampo");//39
            dt.Columns.Add("UltimaGestionCall");//40
            dt.Columns.Add("UltimaGestionCampo");//41
            dt.Columns.Add("NroGestionesCall");//42
            dt.Columns.Add("NroGestionesCampo");//43
            dt.Columns.Add("MejorIncidenciaCall");//44
            dt.Columns.Add("MejorIncidenciaCampo");//45
            dt.Columns.Add("MejorObservacionCall");//46
            dt.Columns.Add("MejorObservacionCampo");//47
            //dt.Columns.Add("ContactadoCall");//20
            //dt.Columns.Add("ContactadoCampo");//20
            dt.Columns.Add("FechaPromesaCall");//20
            dt.Columns.Add("FechaPromesaCampo");//20
            dt.Columns.Add("DetalleDeuda");//21
        }
    }
}
