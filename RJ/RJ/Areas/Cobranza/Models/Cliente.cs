using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Cobranza.Models
{
    public class Cliente
    {
        #region Variables

            private static readonly Cliente _instancia = new Cliente();

            public static Cliente Instancia
            {
                get { return Cliente._instancia; }
            }

        #endregion

        #region Read

            public DataTable ListarClientes()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarClientes");
                cmd.CommandTimeout = 180;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

        #endregion
    }
}