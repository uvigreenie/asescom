using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RJ.Models;

namespace RJ.Areas.Seguridad.Models
{
    public class TreeMenu
    {
        private static readonly TreeMenu _instancia = new TreeMenu();

        public static TreeMenu Instancia
        {
            get { return TreeMenu._instancia; }
        }

        #region Atributos

            public short menu { get; set; }

            public string text { get; set; }

            public string qtip { get; set; }

            public string id { get; set; }

            public bool leaf { get; set; }

            public string iconCls { get; set; }

            public bool acceso { get; set; }

            public bool multiple { get; set; }

            public List<TreeMenu> children { get; set; }

        #endregion

        #region Read

            public List<TreeMenu> ListarMenuxUsuario(byte grupo, short usuario)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ObtenerMenusxUsuario");
                db.AddInParameter(cmd, "@prmintGrupo", DbType.Int16, grupo);
                db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                return ObtenerNodos(dt, 0);
            }

            public List<TreeMenu> ListarAccesoMenuUsuario(byte grupo, short usuario)
            {
                Database db = new SqlDatabase(ConexionDB.Instancia.CadenaConexion());
                DbCommand cmd = db.GetStoredProcCommand("uspSEG_ObtenerAccesoMenuUsuario");
                db.AddInParameter(cmd, "@prmintGrupo", DbType.Int16, grupo);
                db.AddInParameter(cmd, "@prmintUsuario", DbType.Int16, usuario);
                DataTable dt = db.ExecuteDataSet(cmd).Tables[0];

                return ObtenerNodos(dt, 0);
            }

            private List<TreeMenu> ObtenerNodos(DataTable dt, short menu)
            {
                List<TreeMenu> hijos = new List<TreeMenu>();

                var child = from c in dt.AsEnumerable()
                            where Convert.ToInt16(c["Padre"]) == menu
                            select new
                            {
                                Menu = Convert.ToInt16(c["Menu"]),
                                Nombre = c["Nombre"].ToString(),
                                Tooltip = c["Tooltip"].ToString(),
                                Clase = c["Clase"].ToString(),
                                Leaf = Convert.ToBoolean(c["Leaf"]),
                                IconCls = c["Icono"].ToString(),
                                Acceso = Convert.ToBoolean(c["Acceso"]),
                                Multiple = Convert.ToBoolean(c["Multiple"])
                            };
                foreach (var c in child)
                {
                    TreeMenu nodo = new TreeMenu();
                    nodo.menu = c.Menu;
                    nodo.text = c.Nombre;
                    nodo.qtip = c.Tooltip;
                    nodo.id = c.Clase;
                    nodo.leaf = c.Leaf;
                    nodo.iconCls = c.IconCls;
                    nodo.acceso = c.Acceso;
                    nodo.multiple = c.Multiple;
                    nodo.children = ObtenerNodos(dt, c.Menu);
                    if (nodo.leaf || nodo.children.Count > 0)
                    {
                        hijos.Add(nodo);
                    }
                }
                return hijos;
            }

        #endregion
    }
}