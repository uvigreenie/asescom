using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace RJ.Areas.Cobranza.Controllers
{
    public class IndexController : Controller
    {
        //
        // GET: /Cobranza/Index/
//

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Carga(HttpPostedFileBase file)
        {
            MemoryStream ms = new MemoryStream();
            file.InputStream.CopyTo(ms);

            StreamReader r = new StreamReader(ms);

            string xml = "<root><cobranza ";
            string linea = "";
            string[] valores;

            while (!r.EndOfStream)
            {
                linea = r.ReadLine();
                valores = linea.Split(new char[] { '|' });
                //for (int i = 0; i < valores.Length; i++)
                //{
                    xml += "Nombre = '" + valores[0] + "' ";
                    xml += "Apellido = '" + valores[1] + "' ";
                    xml += "Deuda = '" + valores[2] + "' ";
                    xml += "Pagado = '" + valores[3] + "' />";
                //}
            }

            xml += "</root>";

            //Modelo.Instacia.Guarda(xml);

            return new JsonResult();
        }

    }
}
