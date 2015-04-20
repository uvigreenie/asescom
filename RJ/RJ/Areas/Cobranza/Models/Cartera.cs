using System;
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

            public DataTable ListarFechaInicioCartera(short gestionCliente)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarInicioCartera");
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

            public DataTable ListarDepartamentoIBK(int gestionCliente, string fechaInicio)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDepartamentoIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                return db.ExecuteDataSet(cmd).Tables[0];
            }
            public DataTable ListarZonal(int gestionCliente, string fechaFin)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarZonal");
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

            public DataTable ListarDepartamentoxZonal(int gestionCliente, string fechaFin, string zonales)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDepartamentoxZonal");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarTramoxDepartamento(int gestionCliente, string fechaFin, string zonales, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTramoxDpto");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProductoxZonalBBVA(int gestionCliente, string fechaInicio, string zonales)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProductoxZonalBBVA");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarTramoxDptoIBK(int gestionCliente, string fechaInicio, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTramoxDptoIBK");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarTramoxProductoBBVA(int gestionCliente, string fechaInicio, string zonales, string producto)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarTramoxProductoBBVA");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                db.AddInParameter(cmd, "@prmstrProducto", DbType.String, producto);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProductoxDptoIBK(int gestionCliente, string fechaInicio, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProductoxDptoIBK");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarClusterxTramo(int gestionCliente, string fechaFin, string zonales, string departamento, string tramo)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarClusterxTramo");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProvinciaxDpto(int gestionCliente, string fechaFin, string zonales, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProvinciaxDpto");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProvinciaxDptoIBK(int gestionCliente, string fechaInicio, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProvinciaxDptoIBK");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarDistritoxProv(int gestionCliente, string fechaFin, string zonales, string departamento, string provincia)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDistritoxProv");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonales);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrProvincia", DbType.String, provincia);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarDistritoxProvIBK(int gestionCliente, string fechaInicio, string departamento, string provincia)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDistritoxProvIBK");
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrProvincia", DbType.String, provincia);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarGestiones(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestiones");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarGestionesGrid(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionesGrid");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarGestionesBBVA(string cliente, short gestionCliente, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionesBBVA");
                cmd.CommandTimeout = 30;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarGestionesIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionesIBK");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarGestionesGridIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarGestionesGridIBK");
                cmd.CommandTimeout = 30;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarPromesasBBVA(string cliente, short gestionCliente, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPromesasBBVA");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarPromesasIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPromesasIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarPromesasGridIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPromesasGridIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMejorGestion(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarMejorGestion");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMejorGestionGrid(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarMejorGestionGrid");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMejorGestionBBVA(string cliente, short gestionCliente, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarMejorGestionBBVA");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMejorGestionIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarMejorGestionIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMejorGestionGridIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarMejorGestionGridIBK");
                cmd.CommandTimeout = 30;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarPagos(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPagos");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }
            public DataTable ListarPagosAcumulados(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, DateTime fechaDesde, DateTime fechaHasta)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPagosAgrupados");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmdatFechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(cmd, "@prmdatFechaHasta", DbType.DateTime, fechaHasta);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorosos(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, string tramo)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ExportarMorosos");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososIBK(string cliente, short gestionCliente, string fechaInicio, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ExportarMorososIBK");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososBBVA(string cliente, short gestionCliente, string fechaInicio, string zonal)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ExportarMorososBBVA");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososGrid(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, string tramo)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ExportarMorososGrid");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososGridIBK(string cliente, short gestionCliente, string fechaInicio, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ExportarMorososGridIBK");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
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

            public DataTable ListarMorososEnCarteraV2(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento, string tramo, string cluster)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCarteraV2");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                db.AddInParameter(cmd, "@prmstrCluster", DbType.String, cluster);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososEnCarteraIBK(string cliente, short gestionCliente, string fechaInicio, string departamento, string tramo)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCarteraIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososEnCarteraBBVA(string cliente, short gestionCliente, string fechaInicio, string zonal, string tramo, string producto)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCarteraBBVA");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrTramo", DbType.String, tramo);
                db.AddInParameter(cmd, "@prmstrProducto", DbType.String, producto);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarAsignadosBBVA(string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaAsignadosBBVA");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososEnCarteraOtrosFiltros(string cliente, short gestionCliente, int idParametro, string parametro, double valor1, double valor2)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCartera_OF");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmintidParametro", DbType.String, idParametro);
                db.AddInParameter(cmd, "@prmintValor1", DbType.String, valor1);
                db.AddInParameter(cmd, "@prmintValor2", DbType.String, valor2);
                db.AddInParameter(cmd, "@prmstrParametro", DbType.String, parametro);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososEnCarteraIBKOtrosFiltros(string cliente, short gestionCliente, int idParametro, string parametro)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCarteraIBK_OF");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmintidParametro", DbType.String, idParametro);
                db.AddInParameter(cmd, "@prmstrParametro", DbType.String, parametro);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarMorososEnCarteraBBVAOtrosFiltros(string cliente, short gestionCliente, int idParametro, string parametro)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaMorososEnCarteraBBVA_OF");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmintidParametro", DbType.String, idParametro);
                db.AddInParameter(cmd, "@prmstrParametro", DbType.String, parametro);
                return db.ExecuteDataSet(cmd).Tables[0];
            }
            public DataTable ListarServicioV2(int detalleCartera) {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaServiciosxDetalleCartera");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleCartera", DbType.Int32, detalleCartera);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarServicioFija(int detalleCarteraFija)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaServicioFijaxDetalleCarteraFija");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleCarteraFija", DbType.Int32, detalleCarteraFija);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarServicioMovil(int detalleCarteraMovil)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListaServicioMovilxDetalleCarteraMovil");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleCarteraMovil", DbType.Int32, detalleCarteraMovil);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarPagoXServicio(int idservicio)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPagoXServicio");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintIDServicio", DbType.Int32, idservicio);
                return db.ExecuteDataSet(cmd).Tables[0];
            }    

            public DataTable ListarPagoXProducto(int detallecarterabanco)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarPagoXProducto");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleCarteraBanco", DbType.Int32, detallecarterabanco);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarControlGestionXTrabajador(string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarControlGestionXTrabajador");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarCampañaXProducto(int detallecarterabanco)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarCampañaXProducto");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleCarteraBanco", DbType.Int32, detallecarterabanco);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProductos(int cartera, int moroso)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProductosxMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintCartera", DbType.Int32, cartera);
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarRazonNoPago(short gestionCliente)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarRazonNoPago");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProductosRecovery(int cartera, int moroso)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProductosxMorosoRecovery");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintCartera", DbType.Int32, cartera);
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProductosBBVA(int gestionCliente, int cartera, int moroso)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarProductosxMorosoBBVA");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintCartera", DbType.Int32, cartera);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public List<object> ListarDirecciones(string cliente, short gestionCliente, string fechaFin, string zonal, string departamento) 
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDirecciones");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 DetalleMoroso = Convert.ToInt32(m["DetalleMoroso"]),
                                 //NumeroDocumento = m["NumeroDocumento"].ToString(),
                                 RazonSocial = m["RazonSocial"].ToString(),
                                 Departamento = m["Departamento"].ToString(),
                                 Provincia = m["Provincia"].ToString(),
                                 Distrito = m["Distrito"].ToString(),
                                 Direccion = m["Direccion"].ToString(),
                                 Sector = m["Sector"].ToString()
                             }).ToList<object>();
                return lista;
            }

            public List<object> ListarDireccionesIBK(string cliente, short gestionCliente, string fechaInicio, string departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ListarDireccionesIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];
                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 DetalleMoroso = Convert.ToInt32(m["DetalleMoroso"]),
                                 //NumeroDocumento = m["NumeroDocumento"].ToString(),
                                 RazonSocial = m["RazonSocial"].ToString(),
                                 Departamento = m["Departamento"].ToString(),
                                 Provincia = m["Provincia"].ToString(),
                                 Distrito = m["Distrito"].ToString(),
                                 Direccion = m["Direccion"].ToString(),
                                 Sector = m["Sector"].ToString()
                             }).ToList<object>();
                return lista;
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
                                 FechaPago = (m["FechaPago"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(m["FechaPago"])),
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
                                 Trabajador = Convert.ToInt16(m["Trabajador"]),
                                 DTrabajador = m["DTrabajador"].ToString(),
                                 DescDClaseGestion = m["DescDClaseGestion"].ToString(),
                                 FechaGestion = Convert.ToDateTime(m["FechaGestion"]),
                                 HoraGestion = Convert.ToDateTime(m["HoraGestion"]).ToString("HH:mm"),
                                 FechaPromesa = (m["FechaPromesa"] == DBNull.Value ? new DateTime?() : Convert.ToDateTime(m["FechaPromesa"])),
                                 Monto = Convert.ToDecimal(m["Monto"]),
                                 RazonNoPago = Convert.ToDecimal(m["RazonNoPago"]),
                                 Observacion = m["Observacion"].ToString()
                             }).ToList<object>();
                return lista;
            }

        #endregion

        #region Insert

            public int InsUpdGestionMoroso(int? gestionMoroso, int cartera, int detalleMoroso, byte tipoGestion, byte claseGestion, int moroso, short dclaseGestion, DateTime fechaGestion, string horaGestion, DateTime? fechaPromesa, decimal monto, string observacion, short trabajador,string login, string razonNoPago)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsUpdGestionMoroso");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintGestionMoroso", DbType.Int32, gestionMoroso);
                db.AddInParameter(cmd, "@prmintCartera", DbType.Int32, cartera);
                db.AddInParameter(cmd, "@prmintDetalleMoroso", DbType.Int32, detalleMoroso);
                db.AddInParameter(cmd, "@prmintTipoGestion", DbType.Byte, tipoGestion);
                db.AddInParameter(cmd, "@prmintClaseGestion", DbType.Byte, claseGestion);
                db.AddInParameter(cmd, "@prmintMoroso", DbType.Int32, moroso);
                db.AddInParameter(cmd, "@prmintDClaseGestion", DbType.Int16, dclaseGestion);
                db.AddInParameter(cmd, "@prmintTrabajador", DbType.Int16, trabajador);
                db.AddInParameter(cmd, "@prmdatFechaGestion", DbType.DateTime, fechaGestion);
                db.AddInParameter(cmd, "@prmstrHoraGestion", DbType.String, horaGestion);
                db.AddInParameter(cmd, "@prmdatFechaPromesa", DbType.DateTime, fechaPromesa);
                db.AddInParameter(cmd, "@prmdecMonto", DbType.Decimal, monto);
                db.AddInParameter(cmd, "@prmstrObservacion", DbType.String, observacion);
                db.AddInParameter(cmd, "@prmstrRazonNoPago", DbType.String, razonNoPago);
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

            public void InsUpdSector(int detalleMoroso, string Sector)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsUpdSector");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmintDetalleMoroso", DbType.Int32, detalleMoroso);
                db.AddInParameter(cmd, "@prmstrSector", DbType.String, Sector);

                db.ExecuteNonQuery(cmd);
            }

            public void InsUpdSectores(string cliente, int gestionCliente, string fechaFin, string zonal, string departamento, string provincia, string distrito,string sector)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsUpdSectores");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaFin", DbType.String, fechaFin);
                db.AddInParameter(cmd, "@prmstrZonal", DbType.String, zonal);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrProvincia", DbType.String, provincia);
                db.AddInParameter(cmd, "@prmstrDistrito", DbType.String, distrito);
                db.AddInParameter(cmd, "@prmstrSector", DbType.String, sector);

                db.ExecuteNonQuery(cmd);
            }

            public void InsUpdSectoresIBK(string cliente, int gestionCliente, string fechaInicio, string departamento, string provincia, string distrito, string sector)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsUpdSectoresIBK");
                cmd.CommandTimeout = 180;
                db.AddInParameter(cmd, "@prmstrCliente", DbType.String, cliente);
                db.AddInParameter(cmd, "@prmintGestionCliente", DbType.Int16, gestionCliente);
                db.AddInParameter(cmd, "@prmdatFechaInicio", DbType.String, fechaInicio);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, departamento);
                db.AddInParameter(cmd, "@prmstrProvincia", DbType.String, provincia);
                db.AddInParameter(cmd, "@prmstrDistrito", DbType.String, distrito);
                db.AddInParameter(cmd, "@prmstrSector", DbType.String, sector);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarSectores(StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_GuardarSectores");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

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

            public void GuardarDetalleCarteraMovil(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarDetalleCarteraMovil");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarCarteraIBK(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarCarteraIBK");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarCarteraFija(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarCarteraFija");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarCarteraBBVA(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarCarteraBBVA");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void GuardarCarteraMovil(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_InsertarCarteraMovil");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

            public void ActualizarCarteraIBK(int gestionCliente, StringBuilder xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspCOB_ActualizarCarteraIBK");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintGestionCliente", System.Data.DbType.Int32, gestionCliente);
                db.AddInParameter(cmd, "@prmstrXML", System.Data.DbType.String, xml.ToString());
                db.AddInParameter(cmd, "@prmstrLogin", System.Data.DbType.String, login);

                db.ExecuteNonQuery(cmd);
            }

        #endregion
    }
}