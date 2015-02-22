using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Areas.Cobranza.Entity;
using RJ.Models;

namespace RJ.Areas.Cobranza.Models
{
    public class GestionCliente
    {
        #region Variables

            private static readonly GestionCliente _instancia = new GestionCliente();

            public static GestionCliente Instancia
            {
                get { return GestionCliente._instancia; }
            }

        #endregion

        #region Read

            public DataTable ListarGestionClientes(string cliente)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionClientes");
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                cmd.CommandTimeout = 180;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public List<object> ListarTipoGestion(byte tipoDetalle)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTipoGestion");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintTipoDetalle", DbType.Byte, tipoDetalle);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 TipoGestion = Convert.ToByte(m["TipoGestion"]),
                                 DTipoGestion = m["DTipoGestion"].ToString()
                             }).ToList<object>();
                return lista;
            }

            public List<object> ListarClaseGestion(short gestionCliente)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarClaseGestion");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 ClaseGestion = Convert.ToByte(m["ClaseGestion"]),
                                 Codigo = m["Codigo"].ToString(),
                                 DClaseGestion = m["DClaseGestion"].ToString(),
                                 AplicaPromesa = Convert.ToBoolean(m["AplicaPromesa"])
                             }).ToList<object>();
                return lista;
            }

            public List<object> ListarDClaseGestion(short claseGestion)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDClaseGestion");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintClaseGestion", DbType.Byte, claseGestion);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 DClaseGestion = Convert.ToByte(m["DClaseGestion"]),
                                 Descripcion = m["Descripcion"].ToString()
                             }).ToList<object>();
                return lista;
            }

            public List<object> ListarDClaseGestionBBVA()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDClaseGestionBBVA");
                cmd.CommandTimeout = 180;
                //db.AddInParameter(cmd, "@prmintClaseGestion", DbType.Byte, claseGestion);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 DClaseGestion = Convert.ToByte(m["DClaseGestion"]),
                                 Descripcion = m["Descripcion"].ToString()
                             }).ToList<object>();
                return lista;
            }

            public CobranzaDs.GestionMorosoDataTable ObtenerGestionMoroso(int gestionCliente, string tramo, string cluster, string departamento, string provincia, string distrito, int tipoEstado)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionCliente");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                db.AddInParameter(cmd, "@prmstrCluster", DbType.String, cluster);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrProvincia", DbType.String, provincia);
                db.AddInParameter(cmd, "@prmstrDistrito", DbType.String, distrito);
                db.AddInParameter(cmd, "@prmintTipoEstado", DbType.Int32, tipoEstado);
                CobranzaDs ds = new CobranzaDs();
                db.LoadDataSet(cmd, ds, "GestionMoroso");

                return ds.GestionMoroso;
            }

        #endregion
    }
}