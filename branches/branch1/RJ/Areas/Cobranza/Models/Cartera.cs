﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Cobranza.Models
{
    public class Cartera
    {
        #region Variables

            private static readonly Cartera _instancia = new Cartera();

            public static Cartera Instancia
            {
                get { return Cartera._instancia; }
            }

        #endregion

        #region Read

            public DataTable ListarFechaFinCartera(short gestionCliente)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarFinCartera");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarTramo(int gestionCliente, string fechaFin)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTramo");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarCluster(int gestionCliente, string fechaFin, string tramo)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarCluster");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarDepartamento(int gestionCliente, string fechaFin, string tramo, string clusters)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDepartamento");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                db.AddInParameter(cmd, "@prmstrCluster", DbType.String, clusters);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososEnCartera(string cliente, short gestionCliente, string fechaFin, string tramo, string cluster, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCartera");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                db.AddInParameter(cmd, "@prmstrCluster", DbType.String, cluster);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public List<object> ListarServicio(int detalleCartera)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaServiciosxDetalleCartera");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleCartera", DbType.Int32, detalleCartera);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 IDServicio = Convert.ToInt32(m["IDServicio"]),
                                 TipoDocumento = m["TipoDocumento"].ToString(),
                                 NumeroDocumento = m["NumeroDocumento"].ToString(),
                                 FechaEmision = Convert.ToDateTime(m["FechaEmision"]),
                                 FechaVencimiento = Convert.ToDateTime(m["FechaVencimiento"]),
                                 Moneda = m["Moneda"].ToString(),
                                 MontoDeuda = Convert.ToDecimal(m["MontoDeuda"]),
                                 MontoPagado = Convert.ToDecimal(m["MontoPagado"])
                             }).ToList<object>();
                return lista;
            }


            public List<object> ListarDetalleMoroso(int moroso)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaDetalleMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 DetalleMoroso = Convert.ToInt32(m["DetalleMoroso"]),
                                 TipoDetalle = Convert.ToByte(m["TipoDetalle"]),
                                 DTipoDetalle = m["DTipoDetalle"].ToString(),
                                 Descripcion = m["Descripcion"].ToString(),
                                 DescripcionEstado = m["DescripcionEstado"].ToString(),
                                 TipoEstado = Convert.ToByte(m["TipoEstado"]),
                                 DTipoEstado = m["DTipoEstado"].ToString(),
                                 Editable = Convert.ToBoolean(m["Editable"])
                             }).ToList<object>();
                return lista;
            }

            public List<object> ListarGestionMoroso(int detalleMoroso)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleMoroso", DbType.Int32, detalleMoroso);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 GestionMoroso = Convert.ToInt32(m["GestionMoroso"]),
                                 DetalleMoroso = Convert.ToInt32(m["DetalleMoroso"]),
                                 TipoGestion = Convert.ToByte(m["TipoGestion"]),
                                 DTipoGestion = m["DTipoGestion"].ToString(),
                                 ClaseGestion = Convert.ToByte(m["ClaseGestion"]),
                                 Codigo = m["Codigo"].ToString(),
                                 DescClaseGestion = m["DescClaseGestion"].ToString(),
                                 DClaseGestion = Convert.ToInt16(m["DClaseGestion"]),
                                 DescDClaseGestion = m["DescDClaseGestion"].ToString(),
                                 FechaGestion = Convert.ToDateTime(m["FechaGestion"]),
                                 FechaPromesa = (m["FechaPromesa"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(m["FechaPromesa"])),
                                 Monto = Convert.ToDecimal(m["Monto"]),
                                 Observacion = m["Observacion"].ToString()
                             }).ToList<object>();
                return lista;
            }

        #endregion

        #region Insert

            public int InsUpdGestionMoroso(int? gestionMoroso, int cartera, int detalleMoroso, byte tipoGestion, byte claseGestion, short dclaseGestion, DateTime fechaGestion, DateTime? fechaPromesa, decimal monto, string observacion, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsUpdGestionMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionMoroso", DbType.Int32, gestionMoroso);
                db.AddInParameter(cmd, "@prmintCartera", DbType.Int32, cartera);
                db.AddInParameter(cmd, "@prmintDetalleMoroso", DbType.Int32, detalleMoroso);
                db.AddInParameter(cmd, "@prmintTipoGestion", DbType.Byte, tipoGestion);
                db.AddInParameter(cmd, "@prmintClaseGestion", DbType.Byte, claseGestion);
                db.AddInParameter(cmd, "@prmintDClaseGestion", DbType.Int16, dclaseGestion);
                db.AddInParameter(cmd, "@prmdatFechaGestion", DbType.DateTime, fechaGestion);
                db.AddInParameter(cmd, "@prmdatFechaPromesa", DbType.DateTime, fechaPromesa);
                db.AddInParameter(cmd, "@prmdecMonto", DbType.Decimal, monto);
                db.AddInParameter(cmd, "@prmstrObservacion", DbType.String, observacion);
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);

                return Convert.ToInt32(db.ExecuteScalar(cmd));
            }

            public void InsUpdDetalleMoroso(int detalleMoroso, int moroso, byte tipoDetalle, string descripcion, string descripcionEstado, byte tipoEstado, bool editable, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsUpdDetalleMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleMoroso", DbType.Int32, detalleMoroso);
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                db.AddInParameter(cmd, "@prmintTipoDetalle", DbType.Byte, tipoDetalle);
                db.AddInParameter(cmd, "@prmstrDescripcion", DbType.String, descripcion);
                db.AddInParameter(cmd, "@prmstrDescripcionEstado", DbType.String, descripcionEstado);
                db.AddInParameter(cmd, "@prmintTipoEstado", DbType.Byte, tipoEstado);
                db.AddInParameter(cmd, "@prmbitEditable", DbType.Boolean, editable);
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarCartera(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarCabeceraCartera");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarDetalleCartera(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarDetalleCartera");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

        #endregion
    }
}