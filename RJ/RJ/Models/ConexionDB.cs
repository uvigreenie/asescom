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
            string cnn = "Data Source = 181.65.184.170; Initial Catalog = RJAbogados_Test; User ID=sa; Password=admin2013.rar";
            return cnn;
        }
    }
}