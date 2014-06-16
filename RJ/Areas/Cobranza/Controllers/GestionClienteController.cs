using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RJ.Areas.Cobranza.Entity;
using RJ.Areas.Cobranza.Models;
using System.Data;
using System;
using System.Web.Script.Serialization;

namespace RJ.Areas.Cobranza.Controllers
{
    public class GestionClienteController : Controller
    {
        #region Read

            public JsonResult Listar(string cliente)
            {
                DataTable dt = GestionCliente.Instancia.ListarGestionClientes(cliente);

                string fields = "[{\"name\":\"GestionCliente\",\"type\":\"int\"},{\"name\":\"DGestionCliente\",\"type\":\"string\"}]";

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 GestionCliente = Convert.ToInt16(m["GestionCliente"]),
                                 DGestionCliente = m["DGestionCliente"].ToString()
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields }, data = lista }, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarTipoGestion(byte tipoDetalle)
            {
                List<object> lista = GestionCliente.Instancia.ListarTipoGestion(tipoDetalle);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarClaseGestion()
            {
                List<object> lista = GestionCliente.Instancia.ListarClaseGestion();
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

            public JsonResult ListarDClaseGestion(short claseGestion)
            {
                List<object> lista = GestionCliente.Instancia.ListarDClaseGestion(claseGestion);
                return Json(lista, JsonRequestBehavior.AllowGet);
            }

        #endregion

        public JsonResult ObtenerGestionMoroso(int gestionCliente, string tramo, string cluster, string departamento, string provincia, string distrito, int tipoEstado)
        {
            CobranzaDs.GestionMorosoDataTable dt = GestionCliente.Instancia.ObtenerGestionMoroso(gestionCliente, tramo, cluster, departamento, provincia, distrito, tipoEstado);

            var lista = (from c in dt.AsEnumerable()
                         select new { c.Cluster, c.Tramo, c.CodCartera,c.Cuenta,c.NumeroDocumento,c.RazonSocial,c.Deuda, c.Moroso,c.DetalleCartera }).ToList<object>();

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
    }
}
