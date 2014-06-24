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
            //prueba 2
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

            public JsonResult ListarTramo(int gestionCliente, string fechaFin)
            {
                DataTable dt = Cartera.Instancia.ListarTramo(gestionCliente, fechaFin);

                string fields = "[{\"name\":\"Tramo\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Tramo = c["Tramo"].ToString() }).ToList<object>();

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

            public JsonResult ListarDepartamento(int gestionCliente, string fechaFin, string tramo, object[] clusters)
            {
                string xmlDpto = "<root>";

                for (int i = 0; i < clusters.Length; i++)
                {
                    xmlDpto += "<cluster Cluster = '" + clusters[i].ToString() + "' />";
                }
                xmlDpto += "</root>";

                DataTable dt = Cartera.Instancia.ListarDepartamento(gestionCliente,fechaFin,tramo,xmlDpto);

                string fields = "[{\"name\":\"Departamento\",\"type\":\"string\"}]";

                var lista = (from c in dt.AsEnumerable()
                             select new { Departamento = c["Departamento"].ToString() }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
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

        #endregion

            [HttpPost]
            public JsonResult CargarCartera(int gestionCliente, int tipo, HttpPostedFileBase file)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    file.InputStream.CopyTo(ms);
                    StreamReader r = new StreamReader(ms);

                    string xml;
                    string linea = "";
                    string[] valores;
                    int contador = 10;

                    r.BaseStream.Position = 0;

                    StringBuilder cadena = new StringBuilder();
                    cadena.Append("<root>");
                    if ( tipo == 1 )
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
                            xml += "Direccion = '" + devuelveCadena((valores[20] + " " + valores[19] + " " + valores[21])).Trim() + "' ";
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
                catch (Exception ex)
                {
                    return Json(new { success = "false", data = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            
        }

        private string devuelveCadena(string prmstrCadena)
        {
            try
            {
                prmstrCadena = prmstrCadena.Replace( "&", "&amp;");
                prmstrCadena = prmstrCadena.Replace( ">", "&gt;");
                prmstrCadena = prmstrCadena.Replace("–", "&#8211;");
                prmstrCadena = prmstrCadena.Replace( "<", "&lt;");
                prmstrCadena = prmstrCadena.Replace( "''", "&quot;");
                prmstrCadena = prmstrCadena.Replace( "'", "&apos;");
                prmstrCadena = prmstrCadena.Replace( "Á", "&#193;");
                prmstrCadena = prmstrCadena.Replace( "á", "&#225;");
                prmstrCadena = prmstrCadena.Replace( "É", "&#201;");
                prmstrCadena = prmstrCadena.Replace( "é", "&#233;");
                prmstrCadena = prmstrCadena.Replace( "Í", "&#205;");
                prmstrCadena = prmstrCadena.Replace( "í", "&#237;");
                prmstrCadena = prmstrCadena.Replace( "Ó", "&#211;");
                prmstrCadena = prmstrCadena.Replace( "ó", "&#243;");
                prmstrCadena = prmstrCadena.Replace( "Ú", "&#218;");
                prmstrCadena = prmstrCadena.Replace( "ú", "&#250;");
                prmstrCadena = prmstrCadena.Replace( "Ñ", "&#209;");
                prmstrCadena = prmstrCadena.Replace( "ñ", "&#241;");
                prmstrCadena = prmstrCadena.Replace( "`", "&#96;");
                prmstrCadena = prmstrCadena.Replace( "´", "&#180;");
                prmstrCadena = prmstrCadena.Replace( "¨", "&#168;");
                prmstrCadena = prmstrCadena.Replace( "ä", "&#228;");
                prmstrCadena = prmstrCadena.Replace( "ë", "&#235;");
                prmstrCadena = prmstrCadena.Replace( "ö", "&#246;");
                prmstrCadena = prmstrCadena.Replace( "ü", "&#252;");
                prmstrCadena = prmstrCadena.Replace( "à", "&#224;");
                prmstrCadena = prmstrCadena.Replace( "è", "&#232;");
                prmstrCadena = prmstrCadena.Replace( "ì", "&#236;");
                prmstrCadena = prmstrCadena.Replace( "ò", "&#242;");
                prmstrCadena = prmstrCadena.Replace( "ù", "&#249;");
                prmstrCadena = prmstrCadena.Replace( "ç", "&#231;");
                prmstrCadena = prmstrCadena.Replace( "Ç", "&#199;");
                prmstrCadena = prmstrCadena.Replace( "º", "&#186;");
                prmstrCadena = prmstrCadena.Replace( "Ø", "&#216;");
                prmstrCadena = prmstrCadena.Replace( "È", "&#200;");
                prmstrCadena = prmstrCadena.Replace( "À", "&#192;");
                prmstrCadena = prmstrCadena.Replace( "½", "&#189;");
                prmstrCadena = prmstrCadena.Replace( "°", "&#176;");
                prmstrCadena = prmstrCadena.Replace( "ª", "&#170;");
                prmstrCadena = prmstrCadena.Replace( "¡", "&#161;");
                prmstrCadena = prmstrCadena.Replace( "!", "&#33;");
                prmstrCadena = prmstrCadena.Replace( "/", "&#47;");
                prmstrCadena = prmstrCadena.Replace( "¿", "&#191;");
                prmstrCadena = prmstrCadena.Replace( "?", "&#63;");
                prmstrCadena = prmstrCadena.Replace( "=", "&#61;");
                prmstrCadena = prmstrCadena.Replace( "[", "&#91;");
                prmstrCadena = prmstrCadena.Replace( "]", "&#93;");
                prmstrCadena = prmstrCadena.Replace( "\\", "&#92;");
                prmstrCadena = prmstrCadena.Replace( "^", "&#94;");
                prmstrCadena = prmstrCadena.Replace( "_", "&#95;");
                prmstrCadena = prmstrCadena.Replace( "{", "&#123;");
                prmstrCadena = prmstrCadena.Replace( "|", "&#124;");
                prmstrCadena = prmstrCadena.Replace( "}", "&#125;");
                prmstrCadena = prmstrCadena.Replace( "~", "&#126;");
                prmstrCadena = prmstrCadena.Replace( "¬", "&#172;");
                prmstrCadena = prmstrCadena.Replace( "¦", "&#161;");
                prmstrCadena = prmstrCadena.Replace( "¯", "&#175;");
                prmstrCadena = prmstrCadena.Replace( "·", "&#183;");
                prmstrCadena = prmstrCadena.Replace( "$", "&#36;");
                prmstrCadena = prmstrCadena.Replace( "%", "&#37;");
                prmstrCadena = prmstrCadena.Replace( "€", "&euro;");
                prmstrCadena = prmstrCadena.Replace( "—", "&#8212;");
                prmstrCadena = prmstrCadena.Replace( "™", "&#8482;");
                prmstrCadena = prmstrCadena.Replace( "…", "&#8230;");
                prmstrCadena = prmstrCadena.Replace( "‰", "&#8240;");
                prmstrCadena = prmstrCadena.Replace( "•", "&#8226;");
                prmstrCadena = prmstrCadena.Replace( "†", "&#8224;");
                return prmstrCadena;
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
                        cell.Value = Convert.ToDateTime(dr[item.Ordinal]).ToString("dd/MM/yyyy");
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

    }
}
