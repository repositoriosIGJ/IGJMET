using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Test
{
    [TestClass]
    public class HashCode
    {
        [TestMethod]
        public void GetMD5Hash()
        {
            string codigo = 70092100001.ToString("X2");
            //string codigo = Utils.HashCode.Codificar(70092100001994);
            long stamp = long.Parse(DateTime.Now.ToString("yyMMddhhmm"));
            //string fecha = Utils.HashCode.Codificar(stamp);
            string fecha = stamp.ToString("X2");
            string usr = 994.ToString("X2");
            string hash = codigo + "." + fecha;

            Image img = Utils.BarCodeImage.GenerateH(hash);

            string[] splits = hash.Split('.');
            string codigoUnico = Convert.ToInt64(splits[0], 16).ToString();
            string presentacion = codigoUnico.Substring(codigo.Length - 3, 4);

            string fechaPar = Convert.ToInt64(splits[1], 16).ToString();

            DateTime fechaParseada = DateTime.ParseExact(fechaPar, "yyMMddhhmm", null);

            Assert.IsTrue(true);
        }

        private long GetTimestamp()
        {
            DateTime unixRef = new DateTime(1970, 1, 1, 0, 0, 0);
            return (DateTime.Now.Ticks - unixRef.Ticks) / 10000000;
        }

        [TestMethod]
        public void EncodeBase64()
        {
            string text = "70092100001.1.994.789456121";
            byte[] encodeBytes = System.Text.Encoding.UTF8.GetBytes(text);
            string encode = Convert.ToBase64String(encodeBytes);

            byte[] decodeByte = Convert.FromBase64String(encode);

            string decode = System.Text.Encoding.UTF8.GetString(decodeByte);

            Assert.AreEqual(decode, text);
        }

        [TestMethod]
        public void CreateUri()
        {
            var route = new System.Web.Routing.RouteValueDictionary();
            route.Add("operacion", 7188125);
            route.Add("timestamp", System.Diagnostics.Stopwatch.GetTimestamp());

            string token = Utils.HashCode.GetHashCode(route);

            route.Add("token", token);

            string uri = route.ToString();

        }
    }
}
