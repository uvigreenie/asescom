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
            string cnn = "Data Source = localhost; Initial Catalog = RJAbogados; User ID=mgeldres; Password=asescomadmin2014.rar";
            return cnn;
        }
    }
}