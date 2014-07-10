using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RJ.Areas.Common.Models
{
    public class FuncionComun
    {
        #region Variables

        private static readonly FuncionComun _instancia = new FuncionComun();

        public static FuncionComun Instancia
        {
            get { return FuncionComun._instancia; }
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Devuelve Stream para la creación de excel.
        /// </summary>
        /// <param name="dt">DataTable que se quiere migrar.</param>
        /// <param name="nombreHoja">Nombre de la hoja de excel.</param>
        /// <returns></returns>
        public MemoryStream ConvertirTablaToExcel(DataTable dt, string nombreHoja)
        {
            int col = 0;
            int row = 1;
            C1XLBook book = new C1XLBook();
            book.Sheets.Clear();
            XLSheet sheet = book.Sheets.Add(nombreHoja);

            int c = 0;
            foreach (DataColumn item in dt.Columns)
            {
                XLCell cell = sheet[0, col + c];
                cell.Value = item.ColumnName;
                int r = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    cell = sheet[row + r, col + c];
                    if (item.DataType == typeof(DateTime))
                        cell.Value = Convert.ToDateTime(dr[item.Ordinal]).ToString("dd/MM/yyyy");
                    else
                        cell.Value = dr[item.Ordinal];
                    r++;
                }
                c++;
            }

            MemoryStream ms = new MemoryStream();
            book.Save(ms, FileFormat.Csv);

            return ms;
        }

        /// <summary>
        /// Devuelve cadena compatible xml.
        /// </summary>
        /// <param name="prmstrCadena">Cadena para reemplazar.</param>
        /// <returns></returns>
        public string devuelveCadena(string prmstrCadena)
        {
            try
            {
                prmstrCadena = prmstrCadena.Replace("&", "&amp;");
                prmstrCadena = prmstrCadena.Replace(">", "&gt;");
                prmstrCadena = prmstrCadena.Replace("–", "&#8211;");
                prmstrCadena = prmstrCadena.Replace("<", "&lt;");
                prmstrCadena = prmstrCadena.Replace("''", "&quot;");
                prmstrCadena = prmstrCadena.Replace("'", "&apos;");
                prmstrCadena = prmstrCadena.Replace("Á", "&#193;");
                prmstrCadena = prmstrCadena.Replace("á", "&#225;");
                prmstrCadena = prmstrCadena.Replace("É", "&#201;");
                prmstrCadena = prmstrCadena.Replace("é", "&#233;");
                prmstrCadena = prmstrCadena.Replace("Í", "&#205;");
                prmstrCadena = prmstrCadena.Replace("í", "&#237;");
                prmstrCadena = prmstrCadena.Replace("Ó", "&#211;");
                prmstrCadena = prmstrCadena.Replace("ó", "&#243;");
                prmstrCadena = prmstrCadena.Replace("Ú", "&#218;");
                prmstrCadena = prmstrCadena.Replace("ú", "&#250;");
                prmstrCadena = prmstrCadena.Replace("Ñ", "&#209;");
                prmstrCadena = prmstrCadena.Replace("ñ", "&#241;");
                prmstrCadena = prmstrCadena.Replace("`", "&#96;");
                prmstrCadena = prmstrCadena.Replace("´", "&#180;");
                prmstrCadena = prmstrCadena.Replace("¨", "&#168;");
                prmstrCadena = prmstrCadena.Replace("ä", "&#228;");
                prmstrCadena = prmstrCadena.Replace("ë", "&#235;");
                prmstrCadena = prmstrCadena.Replace("ö", "&#246;");
                prmstrCadena = prmstrCadena.Replace("ü", "&#252;");
                prmstrCadena = prmstrCadena.Replace("à", "&#224;");
                prmstrCadena = prmstrCadena.Replace("è", "&#232;");
                prmstrCadena = prmstrCadena.Replace("ì", "&#236;");
                prmstrCadena = prmstrCadena.Replace("ò", "&#242;");
                prmstrCadena = prmstrCadena.Replace("ù", "&#249;");
                prmstrCadena = prmstrCadena.Replace("ç", "&#231;");
                prmstrCadena = prmstrCadena.Replace("Ç", "&#199;");
                prmstrCadena = prmstrCadena.Replace("º", "&#186;");
                prmstrCadena = prmstrCadena.Replace("Ø", "&#216;");
                prmstrCadena = prmstrCadena.Replace("È", "&#200;");
                prmstrCadena = prmstrCadena.Replace("À", "&#192;");
                prmstrCadena = prmstrCadena.Replace("½", "&#189;");
                prmstrCadena = prmstrCadena.Replace("°", "&#176;");
                prmstrCadena = prmstrCadena.Replace("ª", "&#170;");
                prmstrCadena = prmstrCadena.Replace("¡", "&#161;");
                prmstrCadena = prmstrCadena.Replace("!", "&#33;");
                prmstrCadena = prmstrCadena.Replace("/", "&#47;");
                prmstrCadena = prmstrCadena.Replace("¿", "&#191;");
                prmstrCadena = prmstrCadena.Replace("?", "&#63;");
                prmstrCadena = prmstrCadena.Replace("=", "&#61;");
                prmstrCadena = prmstrCadena.Replace("[", "&#91;");
                prmstrCadena = prmstrCadena.Replace("]", "&#93;");
                prmstrCadena = prmstrCadena.Replace("\\", "&#92;");
                prmstrCadena = prmstrCadena.Replace("^", "&#94;");
                prmstrCadena = prmstrCadena.Replace("_", "&#95;");
                prmstrCadena = prmstrCadena.Replace("{", "&#123;");
                prmstrCadena = prmstrCadena.Replace("|", "&#124;");
                prmstrCadena = prmstrCadena.Replace("}", "&#125;");
                prmstrCadena = prmstrCadena.Replace("~", "&#126;");
                prmstrCadena = prmstrCadena.Replace("¬", "&#172;");
                prmstrCadena = prmstrCadena.Replace("¦", "&#161;");
                prmstrCadena = prmstrCadena.Replace("¯", "&#175;");
                prmstrCadena = prmstrCadena.Replace("·", "&#183;");
                prmstrCadena = prmstrCadena.Replace("$", "&#36;");
                prmstrCadena = prmstrCadena.Replace("%", "&#37;");
                prmstrCadena = prmstrCadena.Replace("€", "&euro;");
                prmstrCadena = prmstrCadena.Replace("—", "&#8212;");
                prmstrCadena = prmstrCadena.Replace("™", "&#8482;");
                prmstrCadena = prmstrCadena.Replace("…", "&#8230;");
                prmstrCadena = prmstrCadena.Replace("‰", "&#8240;");
                prmstrCadena = prmstrCadena.Replace("•", "&#8226;");
                prmstrCadena = prmstrCadena.Replace("†", "&#8224;");
                return prmstrCadena;
            }
            catch (Exception)
            {
                return prmstrCadena;
            }
        }

        #endregion
    }
}