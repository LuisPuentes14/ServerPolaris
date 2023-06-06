using ServerPolaris.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

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


        public static string getSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        public static string getMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }


        public static string getMD5SHA1(string str) {
            return getMD5(str + getSHA1(getMD5(str)));
        }


    }
}
