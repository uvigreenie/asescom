using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RJ.Areas.Seguridad.Models;
using RJ.Areas.Laboral.Models;
using System.Collections.Generic;

namespace RJ.Areas.Laboral.Controllers
{
    public class EmpleadoController : Controller
    {
        #region Read
            public JsonResult Listar()
            {
                int usuario = Convert.ToInt16(Session["Usuario"]);
                DataTable dt = Trabajador.Instancia.Listar(usuario);

                string fields = "[{\"name\":\"Trabajador\",\"type\":\"int\"},{\"name\":\"TipoDocumento\",\"type\":\"int\"},{\"name\":\"DTipoDocumento\",\"type\":\"string\"},";
                fields += "{\"name\":\"NumeroDocumento\",\"type\":\"string\"},{\"name\":\"ApellidoPaterno\",\"type\":\"string\"},{\"name\":\"ApellidoMaterno\",\"type\":\"string\"},";
                fields += "{\"name\":\"Nombre\",\"type\":\"string\"},{\"name\":\"FechaNacimiento\",\"type\":\"date\"},{\"name\":\"Sexo\",\"type\":\"string\"},";
                fields += "{\"name\":\"TelefonoFijo\",\"type\":\"string\"},{\"name\":\"Celular\",\"type\":\"string\"},{\"name\":\"Correo\",\"type\":\"string\"},";
                fields += "{\"name\":\"CorreoAlternativo\",\"type\":\"string\"},{\"name\":\"Direccion\",\"type\":\"string\"},{\"name\":\"DireccionAlterna\",\"type\":\"string\"},";
                fields += "{\"name\":\"Departamento\",\"type\":\"string\"},{\"name\":\"DDepartamento\",\"type\":\"string\"},{\"name\":\"Provincia\",\"type\":\"string\"},";
                fields += "{\"name\":\"DProvincia\",\"type\":\"string\"},{\"name\":\"Distrito\",\"type\":\"int\"},{\"name\":\"DDistrito\",\"type\":\"string\"},";
                fields += "{\"name\":\"Zonal\",\"type\":\"int\"},{\"name\":\"DZonal\",\"type\":\"string\"},{\"name\":\"Puesto\",\"type\":\"int\"},";
                fields += "{\"name\":\"DPuesto\",\"type\":\"string\"},{\"name\":\"TipoPuesto\",\"type\":\"string\"},";
                fields += "{\"name\":\"Activo\",\"type\":\"bool\"}]";

                string columns = "[{\"xtype\":\"rownumberer\",\"resizable\":true,\"width\":60},{\"text\":\"Trabajador\",\"dataIndex\":\"Trabajador\",\"hidden\":true,\"hideable\":false},";
                columns += "{\"text\":\"CodZonal\",\"dataIndex\":\"Zonal\",\"hidden\":true,\"hideable\":false},{\"text\":\"Zonal\",\"dataIndex\":\"DZonal\",\"width\":120,\"filterable\":true},";
                columns += "{\"text\":\"Área\",\"dataIndex\":\"TipoPuesto\",\"width\":180,\"filterable\":true},";
                columns += "{\"text\":\"CodPuesto\",\"dataIndex\":\"Puesto\",\"hidden\":true,\"hideable\":false},{\"text\":\"Puesto\",\"dataIndex\":\"DPuesto\",\"width\":150},";
                columns += "{\"text\":\"TipoDocumento\",\"dataIndex\":\"TipoDocumento\",\"hidden\":true,\"hideable\":false},{\"text\":\"Tipo Documento\",\"dataIndex\":\"DTipoDocumento\",\"width\":60},";
                columns += "{\"text\":\"N° Documento\",\"dataIndex\":\"NumeroDocumento\",\"width\":90,\"filterable\":true},{\"text\":\"Apellido Paterno\",\"dataIndex\":\"ApellidoPaterno\",\"width\":120,\"filterable\":true},";
                columns += "{\"text\":\"Apellido Materno\",\"dataIndex\":\"ApellidoMaterno\",\"width\":120,\"filterable\":true},{\"text\":\"Nombres\",\"dataIndex\":\"Nombre\",\"width\":120,\"filterable\":true},";
                columns += "{\"xtype\":\"datecolumn\",\"text\":\"Fecha Nacimiento\",\"dataIndex\":\"FechaNacimiento\",\"width\":100,\"format\":\"d/m/Y\"},{\"text\":\"Sexo\",\"dataIndex\":\"Sexo\",\"width\":40},";
                columns += "{\"text\":\"Teléfono\",\"dataIndex\":\"TelefonoFijo\",\"width\":90},{\"text\":\"Celular\",\"dataIndex\":\"Celular\",\"width\":90},";
                columns += "{\"text\":\"Correo\",\"dataIndex\":\"Correo\",\"width\":120},{\"text\":\"Correo Alterno\",\"dataIndex\":\"CorreoAlternativo\",\"width\":120},";
                columns += "{\"text\":\"Dirección\",\"dataIndex\":\"Direccion\",\"width\":120},{\"text\":\"Dirección Alterna\",\"dataIndex\":\"DireccionAlterna\",\"width\":150},";
                columns += "{\"text\":\"CodDepartamento\",\"dataIndex\":\"Departamento\",\"hidden\":true,\"hideable\":false},{\"text\":\"Departamento\",\"dataIndex\":\"DDepartamento\",\"width\":120},";
                columns += "{\"text\":\"CodProvincia\",\"dataIndex\":\"Provincia\",\"hidden\":true,\"hideable\":false},{\"text\":\"Provincia\",\"dataIndex\":\"DProvincia\",\"width\":120},";
                columns += "{\"text\":\"CodDistrito\",\"dataIndex\":\"Distrito\",\"hidden\":true,\"hideable\":false},{\"text\":\"Distrito\",\"dataIndex\":\"DDistrito\",\"width\":120},";
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
                                 //FechaNacimiento = Convert.ToDateTime(m["FechaNacimiento"]).ToString("yyyy/MM/dd"),
                                 FechaNacimiento = (m["FechaNacimiento"] == DBNull.Value ? "" : Convert.ToDateTime(m["FechaNacimiento"]).ToString("yyyy/MM/dd")),
                                 Sexo = m["Sexo"].ToString(),
                                 TelefonoFijo = m["TelefonoFijo"].ToString(),
                                 Celular = m["Celular"].ToString(),
                                 Correo = m["Correo"].ToString(),
                                 CorreoAlternativo = m["CorreoAlternativo"].ToString(),
                                 Direccion = m["Direccion"].ToString(),
                                 DireccionAlterna = m["DireccionAlterna"].ToString(),
                                 Departamento = m["Departamento"] == DBNull.Value ? "" : m["Departamento"].ToString(),
                                 DDepartamento = m["DDepartamento"] == DBNull.Value ? "" : m["DDepartamento"].ToString(),
                                 Provincia = m["Provincia"] == DBNull.Value ? "" : m["Provincia"].ToString(),
                                 DProvincia = m["DProvincia"] == DBNull.Value ? "" : m["DProvincia"].ToString(),
                                 Distrito = m["Distrito"] == DBNull.Value ? 0 : Convert.ToInt16(m["Distrito"]),
                                 DDistrito = m["DDistrito"] == DBNull.Value ? "" : m["DDistrito"].ToString(),
                                 Zonal = m["Zonal"] == DBNull.Value ? 0 : Convert.ToInt16(m["Zonal"]),
                                 DZonal = m["DZonal"] == DBNull.Value ? "" : m["DZonal"].ToString(),
                                 Puesto = m["Puesto"] == DBNull.Value ? 0 : Convert.ToInt16(m["Puesto"]),
                                 DPuesto = m["DPuesto"] == DBNull.Value ? "" : m["DPuesto"].ToString(),
                                 TipoPuesto = m["TipoPuesto"] == DBNull.Value ? "" : m["TipoPuesto"].ToString(),
                                 Activo = Convert.ToBoolean(m["Activo"])
                             }).ToList<object>();

                var jsFields = new JavaScriptSerializer().Deserialize(fields, typeof(object));
                var jsColumns = new JavaScriptSerializer().Deserialize(columns, typeof(object));
                return Json(new { success = "true", metaData = new { fields = jsFields, columns = jsColumns }, data = lista }, JsonRequestBehavior.AllowGet);
            }
        #endregion
    }
}
