using System;
using System.Collections.Generic;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Domain.Models;
using Utils;

namespace Services
{
    public class HerramientaService : IHerramientaService
    {
        private readonly ICaratulaService _caratulaService;

        public HerramientaService(ICaratulaService caratulaService)
        {
            _caratulaService = caratulaService;
        }

        public VerificarCaratulaModel VerificarCaratula(string hashcode)
        {
            try
            {

                long codOperacion = HashCode.CodigoUnicoCaratula(hashcode);

                ReporteModel reporte = _caratulaService.GetCaratulaPorOperacion(codOperacion);

                return new VerificarCaratulaModel
                {
                    CodigoSeguridad = hashcode,
                    Caratula = reporte
                };
            }
            catch (Exception)
            {
                return new VerificarCaratulaModel
                {
                    CodigoSeguridad = hashcode,
                    Caratula = new ReporteModel { Mensaje = "Código de verificación inválido", Resultado = 1 }
                };
            }
        }
    }
}
