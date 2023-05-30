using ServerPolaris.Models.ViewModels;

namespace ServerPolaris.Utilidades.Tools
{
    public class Tools
    {

        public static string getConexion(VMConexion vMConexion)
        {

            string conexion = $"Data Source = {vMConexion.DataBaseInstance}; " +
                               $"Initial Catalog = {vMConexion.DataBaseName}; " +
                               $"User ID = {vMConexion.DataBaseUser};" +
                               $" Password = {vMConexion.DataBasePassword}; " +
                               $"Trust Server Certificate = true; ";
            return conexion;
        }


    }
}
