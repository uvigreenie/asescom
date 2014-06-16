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
    public class Menu
    {
        #region Variables

        private static readonly Menu _instancia = new Menu();

        public static Menu Instancia
        {
            get { return Menu._instancia; }
        }

        #endregion

        #region Read

        public List<object> ObtenerMenus(byte empresa, byte grupo)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_ListarMenus");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintEmpresa", DbType.Byte, empresa);
            db.AddInParameter(cmd, "@prmintGrupo", DbType.Byte, grupo);
            DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

            var lista = (from m in dt.AsEnumerable()
                         select new
                         {
                             Menu = Convert.ToInt16(m["Menu"]),
                             Grupo = Convert.ToByte(m["Grupo"]),
                             DGrupo = m["DGrupo"].ToString(),
                             Padre = Convert.ToInt16(m["Padre"]),
                             DPadre = m["DPadre"].ToString(),
                             DMenu = m["DMenu"].ToString(),
                             Tooltip = m["Tooltip"].ToString(),
                             Clase = m["Clase"].ToString(),
                             Ruta = m["Ruta"].ToString(),
                             Icono = m["Icono"].ToString(),
                             Leaf = Convert.ToBoolean(m["Leaf"])
                         }).ToList<object>();
            return lista;
        }

        #endregion

        #region Insert

        public short InsUpdMenus(short menu, byte grupo, short padre, string nombre, string tooltip, string clase, string icono, bool leaf, string login)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_InsUpdMenus");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintMenu", DbType.Int16, menu);
            db.AddInParameter(cmd, "@prmintGrupo", DbType.Byte, grupo);
            db.AddInParameter(cmd, "@prmintPadre", DbType.Int16, (padre == 0 ? new short?() : padre));
            db.AddInParameter(cmd, "@prmstrNombre", DbType.String, nombre);
            db.AddInParameter(cmd, "@prmstrTooltip", DbType.String, tooltip);
            db.AddInParameter(cmd, "@prmstrClase", DbType.String, clase);
            db.AddInParameter(cmd, "@prmstrIcono", DbType.String, icono);
            db.AddInParameter(cmd, "@prmbitLeaf", DbType.Boolean, leaf);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);

            return Convert.ToInt16(db.ExecuteScalar(cmd));
        }

        public void GuardarMenuUsuario(string xml, short usuario, string login)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_GuardarMenuUsuario");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
            db.AddInParameter(cmd, "@prmstrXML", DbType.String, xml);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
            db.ExecuteNonQuery(cmd);
        }

        #endregion

        #region Anular

        public void AnularMenu(short menu, string login)
        {
            Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
            DbCommand cmd = db.GetStoredProcCommand("uspSEG_AnularMenu");
            cmd.CommandTimeout = 180;
            db.AddInParameter(cmd, "@prmintMenu", DbType.Int16, menu);
            db.AddInParameter(cmd, "@prmstrLogin", DbType.String, login);
            db.ExecuteNonQuery(cmd);
        }

        #endregion
    }
}