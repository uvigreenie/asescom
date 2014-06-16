using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Areas.Seguridad.Entity;
using RJ.Models;

namespace RJ.Areas.Seguridad.Models
{
    public class Grupo
    {
        #region Variables

            private static readonly Grupo _instancia = new Grupo();

            public static Grupo Instancia
            {
                get { return Grupo._instancia; }
            }

        #endregion

        #region Read

            public List<object> ObtenerGrupos(short empresa)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarGrupos");
                db.AddInParameter(cmd, "@prmintEmpresa", DbType.Int16, empresa);
                SeguridadDs ds = new SeguridadDs();
                db.LoadDataSet(cmd, ds, "Grupo");

                var lista = (from p in ds.Grupo.AsEnumerable()
                             select new { p.Grupo, p.DGrupo, p.Empresa }).ToList<object>();
                return lista;
            }

        #endregion
    }
}