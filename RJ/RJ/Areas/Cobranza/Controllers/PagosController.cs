using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using RJ.Areas.Cobranza.Models;

namespace RJ.Areas.Cobranza.Controllers
{
    public class PagosController : Controller
    {
        #region Insert
        int contador = 1;
        public JsonResult CargarPagos(int gestionCliente, int tipo, HttpPostedFileBase file)
        {
            try
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
                return Json(new { success = "true", data = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = "false", data = ex.Message+" linea="+contador.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        
    }
}
