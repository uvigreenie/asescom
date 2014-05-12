using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Seguridad.Models
{
    public class Autenticacion
    {
        private static readonly Autenticacion _instancia = new Autenticacion();

        public static Autenticacion Instancia
        {
            get { return Autenticacion._instancia; }
        }

        #region Atributos

        public int Usuario { get; set; }

        public string Login { get; set; }

        public string Nombres { get; set; }

        public int Empresa { get; set; }

        public bool Activo { get; set; }

        #endregion

        #region Read

        public List<Autenticacion> VerificarLogin(string usuario, string password, UInt16 empresa)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_VerificarLogin");
            db.AddInParameter(cmd, "@prmintEmpresa", DbType.Int16, empresa);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, usuario);
            db.AddInParameter(cmd, "@prmstrPassword", DbType.String, password);
            IDataReader dr = db.ExecuteReader(cmd);

            List<Autenticacion> lista = new List<Autenticacion>();
            while (dr.Read())
            {
                lista.Add(new Autenticacion()
                {
                    Usuario = Convert.ToInt16(dr["Usuario"]),
                    Login = dr["Login"].ToString(),
                    Nombres = dr["Nombres"].ToString(),
                    Empresa = Convert.ToInt16(dr["Empresa"]),
                    Activo = Convert.ToBoolean(dr["Activo"])
                });
            }
            return lista;
        }

        public List<object> ListarUsuarios(byte empresa)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarUsuarios");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintEmpresa", DbType.Byte, empresa);
            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

            var lista = (from m in dt.AsEnumerable()
                         select new
                         {
                             Usuario = Convert.ToInt16(m["Usuario"]),
                             Login = m["Login"].ToString(),
                             Nombres = m["Nombres"].ToString(),
                             Correo = m["Correo"].ToString(),
                             Activo = Convert.ToBoolean(m["Activo"])
                         }).ToList<object>();
            return lista;
        }

        #endregion

        #region Insertar

        public short InsUpdUsuario(byte empresa, short usuario, string login, string nombres, string correo, bool activo, string user)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_InsUpdUsuarios");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintEmpresa", DbType.Byte, empresa);
            db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
            db.AddInParameter(cmd, "@prmstrNombres", DbType.String, nombres);
            db.AddInParameter(cmd, "@prmstrCorreo", DbType.String, correo);
            db.AddInParameter(cmd, "@prmbitActivo", DbType.Boolean, activo);
            db.AddInParameter(cmd, "@prmstrUser", DbType.String, user);

            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        #endregion

        #region Update

        public bool ChangePassword(int usuario, string passwordOld, string passwordNew, string login)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_CambiarPassword");
            db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
            db.AddInParameter(cmd, "@prmstrPasswordOld", DbType.String, passwordOld);
            db.AddInParameter(cmd, "@prmstrPasswordNew", DbType.String, passwordNew);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login.ToUpper());
            return Convert.ToBoolean(db.ExecuteScalar(cmd));
        }

        #endregion

        #region Anular

        public void AnularUsuario(short usuario, string login)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_AnularUsuario");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
            db.ExecuteNonQuery(cmd);
        }

        #endregion
    }
}