using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Cobranza.Models
{
    public class Moroso
    {
        #region Variables

        private static readonly Moroso _instancia = new Moroso();

            public static Moroso Instancia
            {
                get { return Moroso._instancia; }
            }

        #endregion

        #region Read

            public List<object> ListarRubroEmpleo()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarRubroEmpleo");
                cmd.CommandTimeout = 180;
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 RubroEmpleo = Convert.ToInt32(m["RubroEmpleo"]),
                                 DRubroEmpleo = m["DRubroEmpleo"].ToString()
                             }).ToList<object>();
                return lista;
            }

            public List<object> ObtenerDatosMoroso(int moroso)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ObtenerDetalleMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Moroso = Convert.ToInt32(m["Moroso"]),
                                 NumeroDocumento = m["NumeroDocumento"].ToString(),
                                 DMoroso = m["DMoroso"].ToString(),
                                 Empleado = Convert.ToBoolean(m["Empleado"]),
                                 RubroEmpleo = Convert.ToInt32(m["RubroEmpleo"]),
                                 HoraContacto = (m["HoraContacto"] == DBNull.Value ? "" : Convert.ToDateTime(m["HoraContacto"]).ToString("hh:mm tt").ToUpper().Replace(".", "")),
                                 Observacion = m["Observacion"].ToString()
                             }).ToList<object>();
                return lista;
            }

        #endregion

        #region Insert

            public int UpdMoroso(int moroso, bool empleado, int? rubroEmpleo, DateTime? horaContacto, string observacion, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_UpdMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                db.AddInParameter(cmd, "@prmbolEmpleado", DbType.Boolean, empleado);
                db.AddInParameter(cmd, "@prmintRubroEmpleo", DbType.Int32, rubroEmpleo);
                db.AddInParameter(cmd, "@prmdatHoraContacto", DbType.DateTime, horaContacto);
                db.AddInParameter(cmd, "@prmstrObservacion", DbType.String, observacion);
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);

                return Convert.ToInt32(db.ExecuteScalar(cmd));
            }

        #endregion
    }
}