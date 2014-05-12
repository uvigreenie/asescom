using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Cobranza.Models
{
    public class TipoEstado
    {
        #region Variables

            private static readonly TipoEstado _instancia = new TipoEstado();

            public static TipoEstado Instancia
            {
                get { return TipoEstado._instancia; }
            }

        #endregion

        #region Read

            public DataTable Listar()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTipoEstado");
                cmd.CommandTimeout = 180;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

        #endregion
    }
}