using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using RJ.Areas.Cobranza.Models;
using System.Web.Script.Serialization;
using C1.C1Excel;

namespace RJ.Areas.Cobranza.Controllers
{
    public class PagosController : Controller
    {
        #region Insert
        int contador = 1;
        public JsonResult CargarPagos(int gestionCliente, int tipo, HttpPostedFileBase file)
        {
            JsonResult respuestaJson = new JsonResult();
            try
            {
                
                if (gestionCliente == 1)
                {
                    MemoryStream ms = new MemoryStream();
                    file.InputStream.CopyTo(ms);
                    StreamReader r = new StreamReader(ms);

                    string xml = "";
                    string linea = "";
                    string[] valores;


                    r.BaseStream.Position = 0;

                    StringBuilder cadena = new StringBuilder();
                    cadena.Append("<root>");

                    while (!r.EndOfStream)
                    {
                        linea = r.ReadLine();
                        valores = linea.Split(new char[] { '\t' });

                        if (valores[2].Equals("RJ"))
                        {
                            xml = "<pagos TipoDocumento = '" + valores[7] + "' ";
                            xml += "NumeroDocumento = '" + valores[8] + "' ";
                            xml += "FechaPago = '" + valores[9] + "' ";
                            xml += "MontoDiferencia = '" + valores[10] + "' ";
                            xml += "MontoPagado = '" + valores[12] + "' ";
                            xml += "TipoPago = '" + (tipo == 1 ? "Pago" : valores[13]) + "' />";
                            cadena.Append(xml);
                        }
                        contador++;
                    }

                    cadena.Append("</root>");
                    Pagos.Instancia.CargarPagos(gestionCliente, cadena, Session["Login"].ToString());
                    respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                }else if (gestionCliente == 2)
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
                        xml = "<pagos ";
                        xmlDetalle = "";
                        for (int j = 0; j < sheet.Columns.Count; j++)
                        {
                            if (sheet[0, j].Value != null)
                            {
                                valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                xml += (sheet[0, j].Value.Equals("FecPago") ? "FechaPago = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                xml += (sheet[0, j].Value.Equals("NroProd") ? "NroProducto = '" + valor + "' " : "");
                                xml += (sheet[0, j].Value.Equals("Pago MN") ? "MontoPago = '" + valor + "' " : "");
                            }
                        }
                        cadena.Append(xml + " >");
                        cadena.Append("</pagos>");
                    }
                    cadena.Append("</root>");
                    Pagos.Instancia.CargarPagosIbk(gestionCliente, cadena, Session["Login"].ToString());
                    respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 5)
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
                        xml = "<pagos ";
                        xmlDetalle = "";
                        for (int j = 0; j < sheet.Columns.Count; j++)
                        {
                            if (sheet[0, j].Value != null)
                            {
                                valor = (sheet[i, j].Value == null ? "" : devuelveCadena(sheet[i, j].Value.ToString().Trim()));
                                xml += (sheet[0, j].Value.Equals("FechaAmort") ? "FechaPago = '" + Convert.ToDateTime(sheet[i, j].Value).ToString("yyyyMMdd") + "' " : "");
                                xml += (sheet[0, j].Value.Equals("CJ") ? "NroCJ = '" + valor + "' " : "");
                                xml += (sheet[0, j].Value.Equals("Total Pagado") ? "MontoPago = '" + valor + "' " : "");
                            }
                        }
                        cadena.Append(xml + " >");
                        cadena.Append("</pagos>");
                    }
                    cadena.Append("</root>");
                    Pagos.Instancia.CargarPagosIbk(gestionCliente, cadena, Session["Login"].ToString());
                    respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                }else if (gestionCliente == 4) 
                {
                    MemoryStream ms = new MemoryStream();
                    file.InputStream.CopyTo(ms);
                    StreamReader r = new StreamReader(ms);

                    string xml = "";
                    string linea = "";
                    string[] valores;


                    r.BaseStream.Position = 0;

                    StringBuilder cadena = new StringBuilder();
                    cadena.Append("<root>");

                    while (!r.EndOfStream)
                    {
                        linea = r.ReadLine();
                        valores = linea.Split(new char[] { '|' });

                        if (valores[3].Equals("RJ"))
                        {
                            xml = "<pagos CodCartera = '" + valores[0] + "' ";
                            xml += "Inscripcion = '" + valores[1] + "' ";
                            xml += "Telefono = '" + valores[2] + "' ";
                            xml += "NumeroDocumento = '" + valores[4] + "' ";
                            xml += "FechaPago = '" + valores[5] + "' ";
                            xml += "MontoPagado = '" + valores[6] + "' ";
                            xml += "TipoPago = '"+ valores[7] + "' />";
                            cadena.Append(xml);
                        }
                        contador++;
                    }

                    cadena.Append("</root>");
                    Pagos.Instancia.CargarPagos(gestionCliente, cadena, Session["Login"].ToString());
                    respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                }
                else if (gestionCliente == 6)
                {
                    MemoryStream ms = new MemoryStream();
                    file.InputStream.CopyTo(ms);
                    StreamReader r = new StreamReader(ms);

                    string xml = "";
                    string linea = "";
                    string[] valores;


                    r.BaseStream.Position = 0;

                    StringBuilder cadena = new StringBuilder();
                    cadena.Append("<root>");

                    while (!r.EndOfStream)
                    {
                        linea = r.ReadLine();
                        valores = linea.Split(new char[] { '|' });

                        if (valores[1].Equals("RJ"))
                        {
                            xml = "<pagos CodCartera = '" + valores[0].Trim() + "' ";
                            xml += "Anexo = '" + valores[3].Trim() + "' ";
                            xml += "Telefono = '" + valores[4].Trim() + "' ";
                            xml += "Cliente = '" + valores[5].Trim() + "' ";
                            xml += "TipoDeta = '" + devuelveCadena(valores[6].Trim()) + "' ";
                            xml += "LDoc = '" + devuelveCadena(valores[7].Trim()) + "' ";
                            xml += "NDoc = '" + devuelveCadena(valores[8].Trim()) + "' ";
                            xml += "LCA = '" + devuelveCadena(valores[9].Trim()) + "' ";
                            xml += "TipoPago = '" + devuelveCadena(valores[10].Trim()) + "' ";
                            xml += "LetraPago = '" + devuelveCadena(valores[11].Trim()) + "' ";
                            xml += "NDocPago = '" + devuelveCadena(valores[12].Trim()) + "' ";
                            //xml += "MontoPagado = '" + valores[13] + "' ";

                            if (valores[15].Trim() == "L")
                            {
                                xml += "MontoPagado = '" + Convert.ToDouble(valores[13].Trim()) + "' ";
                            }
                            else
                            {
                                xml += "MontoPagado = '" + Convert.ToDouble(valores[13].Trim()) * 2.9 + "' ";
                            }

                            xml += "FechaPago = '" + Convert.ToDateTime(valores[14]).ToString("yyyyMMdd") + "' />";
                            cadena.Append(xml);
                        }
                        contador++;
                    }

                    cadena.Append("</root>");
                    Pagos.Instancia.CargarPagos(gestionCliente, cadena, Session["Login"].ToString());
                    respuestaJson = Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = "false", data = ex.Message+" linea="+contador.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return respuestaJson;
        }
        #endregion
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
            }
            catch (Exception)
            {
                return prmstrCadena;
            }
        }
        
    }
}
