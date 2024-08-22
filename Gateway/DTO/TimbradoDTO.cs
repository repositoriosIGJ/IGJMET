using System;
using System.Collections.Generic;
using System.Linq;

namespace Gateway.DTO
{
    public enum Codigo
    { 
        OK,
        ER
    }

    public class TimbradoDTO
    {
        public string Resultado { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public short Caja { get; set; }
        public int NroPagoElectronico { get; set; }
        public Codigo CodRetorno { get; set; }

        public TimbradoDTO(string respuestaENTE)
        {
            string codigo = respuestaENTE.Substring(0, 2);

            if (string.Compare(codigo, "OK", true) == 0)
            {
                if (respuestaENTE.Length >= 5)
                {
                    CodRetorno = Codigo.OK;
                    Monto = (Convert.ToDecimal(respuestaENTE.Substring(13, 7)) / 100);
                    Caja = short.Parse(respuestaENTE.Substring(20, 2));
                    NroPagoElectronico = Convert.ToInt32(respuestaENTE.Substring(22, 9));
                    FechaPago = DateTime.ParseExact(respuestaENTE.Substring(5, 8), "ddMMyyyy", null);
                }
            }
            else
            {
                CodRetorno = Codigo.ER;
            }

            Resultado = respuestaENTE;
        }
    }
}
