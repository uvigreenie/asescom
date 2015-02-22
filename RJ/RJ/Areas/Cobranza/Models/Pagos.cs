using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Areas.Cobranza.Entity;
using RJ.Models;
using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace RJ.Areas.Cobranza.Models
{
    public class Pagos
    {
        #region Variables

            private static readonly Pagos _instancia = new Pagos();

            public static Pagos Instancia
            {
                get { return Pagos._instancia; }
            }

        #endregion

        #region Insertar

            public int CargarPagos(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarPagosServicio");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);
                db.ExecuteNonQuery(cmd);

                return 0;
            }

            public int CargarPagosIbk(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarPagosIbk");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);
                db.ExecuteNonQuery(cmd);

                return 0;
            }

        #endregion
    }
}