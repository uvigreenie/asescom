using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Cobranza.Models
{
    public class TipoDetalle
    {
        #region Variables

            private static readonly TipoDetalle _instancia = new TipoDetalle();

            public static TipoDetalle Instancia
            {
                get { return TipoDetalle._instancia; }
            }

        #endregion

        #region Read

            public DataTable Listar()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTipoDetalle");
                cmd.CommandTimeout = 180;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

        #endregion
    }
}