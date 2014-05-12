namespace RJ.Models
{
    public class ConexionDB
    {
        private static readonly ConexionDB _instancia = new ConexionDB();

        public static ConexionDB Instancia
        {
            get { return ConexionDB._instancia; }
        }

        public string CadenaConexion()
        {
            string cnn = "Data Source = APPWEB001; Initial Catalog = RJAbogados; User ID=sa; Password=admin2013.rar";
            return cnn;
        }
    }
}