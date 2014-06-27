using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;
using System;
using System.Collections.Generic;

namespace RJ.Areas.Seguridad.Models
{
    public class Trabajador
    {
        #region Variables

            private static readonly Trabajador _instancia = new Trabajador();

            public static Trabajador Instancia
            {
                get { return Trabajador._instancia; }
            }

        #endregion

        #region Read

            /// <summary>
            /// Devuelve lista de todos los trabajadores.
            /// </summary>
            /// <returns>DataTable</returns>
            public DataTable Listar()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarTrabajadores");
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarTipoDocumento()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarTipoDocumento");
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarDepartamento()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarDepartamento");
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarProvincia(string Departamento)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarProvincia");
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, Departamento);
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarDistrito(string Departamento,string Provincia)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarDistrito");
                db.AddInParameter(cmd, "@prmstrProvincia", DbType.String, Provincia);
                db.AddInParameter(cmd, "@prmstrDepartamento", DbType.String, Departamento);
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarZonal()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarZonal");
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }

            public DataTable ListarPuesto()
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarPuesto");
                cmd.CommandTimeout = 300;
                return db.ExecuteDataSet(cmd).Tables[0];
            }
            /// <summary>
            /// Lista trabajadores por usuario.
            /// </summary>
            /// <param name="usuario">id usuario.</param>
            /// <returns>Lista</returns>
            public List<object> Listar(short usuario)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarTrabajadoresAsignados");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Trabajador = Convert.ToInt16(m["Trabajador"]),
                                 DTrabajador = m["DTrabajador"].ToString()
                             }).ToList<object>();
                return lista;
            }

            /// <summary>
            /// Lista trabajadores asignados a un usuario.
            /// </summary>
            /// <param name="usuario">id usuario.</param>
            /// <returns>Lista</returns>
            public List<object> ListarTrabajadoresxAsignar(short usuario)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarTrabajadoresxAsignar");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                var lista = (from m in dt.AsEnumerable()
                             select new
                             {
                                 Trabajador = Convert.ToInt16(m["Trabajador"]),
                                 DTrabajador = m["DTrabajador"].ToString()
                             }).ToList<object>();
                return lista;
            }

        #endregion

        #region Insert

            public short InsUpdTrabajador(short trabajador, byte tipoDocumento, string numeroDocumento, string apellidoPaterno, string apellidoMaterno, string nombre, 
                DateTime fecha, string sexo, string telefonoFijo, string celular, string correo, string correoAlternativo, string direccion, string direccionAlterna, bool activo, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_InsUpdTrabajador");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintTrabajador", DbType.Int16, trabajador);
                db.AddInParameter(cmd, "@prmintTipoDocumento", DbType.Byte, tipoDocumento);
                db.AddInParameter(cmd, "@prmstrNumeroDocumento", DbType.String, numeroDocumento);
                db.AddInParameter(cmd, "@prmstrApellidoPaterno", DbType.String, apellidoPaterno);
                db.AddInParameter(cmd, "@prmstrApellidoMaterno", DbType.String, apellidoMaterno);
                db.AddInParameter(cmd, "@prmstrNombre", DbType.String, nombre);
                db.AddInParameter(cmd, "@prmdatFechaNacimiento", DbType.DateTime, fecha);
                db.AddInParameter(cmd, "@prmstrSexo", DbType.String, sexo);
                db.AddInParameter(cmd, "@prmstrTelefonoFijo", DbType.String, telefonoFijo);
                db.AddInParameter(cmd, "@prmstrCelular", DbType.String, celular);
                db.AddInParameter(cmd, "@prmstrCorreo", DbType.String, correo);
                db.AddInParameter(cmd, "@prmstrCorreoAlternativo", DbType.String, correoAlternativo);
                db.AddInParameter(cmd, "@prmstrDireccion", DbType.String, direccion);
                db.AddInParameter(cmd, "@prmstrDireccionAlterna", DbType.String, direccionAlterna);
                db.AddInParameter(cmd, "@prmbitActivo", DbType.Boolean, activo);
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);

                return Convert.ToInt16(db.ExecuteScalar(cmd));
            }

            
            public void InsUpdUsuarioTrabajador(short usuario, string xml, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_GuardarUsuarioTrabajador");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
                db.AddInParameter(cmd, "@prmstrXML", DbType.String, xml);
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
                db.ExecuteNonQuery(cmd);
            }

        #endregion

        #region Anular

            /// <summary>
            /// Anula un trabajador.
            /// </summary>
            /// <param name="trabajador">id del trabajador.</param>
            /// <param name="login">usuario que anula.</param>
            public void Anular(short trabajador, string login)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_AnularTrabajador");
                cmd.CommandTimeout = 300;
                db.AddInParameter(cmd, "@prmintTrabajador", DbType.Int16, trabajador);
                db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
                db.ExecuteNonQuery(cmd);
            }

        #endregion
    }
}