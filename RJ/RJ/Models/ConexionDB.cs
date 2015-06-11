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
            string cnn = "Data Source = 181.65.184.171; Initial Catalog = RJAbogados; User ID=mgeldres; Password=asescomadmin2014.rar";
            return cnn;
        }
    }
}