using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;
using System;
using System.Collections.Generic;

namespace RJ.Areas.Laboral.Models
{
    public class Empleado
    {
        #region Variables
        private static readonly Empleado _instancia = new Empleado();

        public static Empleado Instancia 
        {
            get { return Empleado._instancia; } 
        }
        #endregion

        #region Read
        public DataTable Listar(int usuario)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarEmpleados");
            cmd.CommandTimeout = 300;
            db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
            return db.ExecuteDataSet(cmd).Tables[0];
        }
        #endregion
    }
}
