using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web.Routing;

namespace Utils
{
    public class HashCode
    {
        private const int base10 = 10;
        private static char[] cHexa = new char[] { 'T', 'Z', 'R', 'Q', 'K', 'W', 'Y', 'H', 'I', 'J', 'S', 'L', 'M' };
        private static int[] iHexaNumeric = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        private static int[] iHexaIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private const int asciiDiff = 48;

        public static string CodigoUnicoCaratula(long codigoUnico)
        {
            return codigoUnico.ToString("X2");
        }

        public static long CodigoUnicoCaratula(string codigoUnico)
        {
            return long.Parse(codigoUnico, System.Globalization.NumberStyles.HexNumber);
        }

        public static string Codificar(long iDec)
        {
            //Codificacion a Base23
            int numbase = 23;
            string strBin = "";
            long[] result = new long[32];
            int MaxBit = 32;
            for (; iDec > 0; iDec /= numbase)
            {
                long rem = iDec % numbase;
                result[--MaxBit] = rem;
            }
            for (int i = 0; i < result.Length; i++)
                if ((long)result.GetValue(i) >= base10)
                {
                    string res = (Convert.ToInt32(result.GetValue(i).ToString().Substring(0, 1)) - 1).ToString();
                    string pos = res + result.GetValue(i).ToString().Substring(1, 1);
                    strBin += cHexa[Convert.ToInt32(pos)];
                }
                else
                    strBin += result.GetValue(i);
            strBin = strBin.TrimStart(new char[] { '0' });
            int total = 6 - strBin.Length;

            for (int i = 0; i < total; i++)
            {
                strBin = "0" + strBin;
            }

            return strBin;
        }

        public static int Decodificar(string sBase)
        {
            //Decodificacion a Base23
            int numbase = 23;
            int dec = 0;
            int b;
            int iProduct = 1;
            string sHexa = "";
            if (numbase > base10)
                for (int i = 0; i < cHexa.Length; i++)
                    sHexa += cHexa.GetValue(i).ToString();
            for (int i = sBase.Length - 1; i >= 0; i--, iProduct *= numbase)
            {
                string sValue = sBase[i].ToString();
                if (sValue.IndexOfAny(cHexa) >= 0)
                    b = iHexaNumeric[sHexa.IndexOf(sBase[i])];
                else
                    b = (int)sBase[i] - asciiDiff;
                dec += (b * iProduct);
            }
            return dec;
        }

        public static string GetHashCode(RouteValueDictionary route)
        {
            string url = "";

            foreach (var key in route.Keys)
            {
                url += String.Format("{0}={1}&", key, route[key]);
            }

            url = url.Substring(0, url.Length - 1);

            return GetHashCodeFromString(url);
        }

        public static string GetHashCodeFromString(string message)
        {
            string key = Config.StringConfig("tokenKey");

            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] bytes = encoding.GetBytes(message);

            bytes = hmacsha1.ComputeHash(bytes);

            return ByteToString(bytes);
        }

        public static string GetMD5Hash(string message)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            MD5 md5 = MD5.Create();

            bytes = md5.ComputeHash(bytes);

            return ByteToString(bytes);
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
