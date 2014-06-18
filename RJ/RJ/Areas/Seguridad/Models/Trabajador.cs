using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;
using System;

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