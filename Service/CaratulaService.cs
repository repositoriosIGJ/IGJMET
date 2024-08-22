using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using System.Threading.Tasks;
using Domain.Contracts.Logics;
using Domain.Contracts.Services;
using Domain.Models;

namespace Services
{
    public class CaratulaService:ICaratulaService
    {
        private readonly ICaratulaLogic _caratulaLogic;
        private readonly IOperacionLogic _operacionLogic;
        private readonly IPresentacionTramiteLogic _presentacionLogic;

        public CaratulaService(ICaratulaLogic caratulaLogic, IOperacionLogic operacionLogic, IPresentacionTramiteLogic presentacionLogic)
        {
            _caratulaLogic = caratulaLogic;
            _operacionLogic = operacionLogic;
            _presentacionLogic = presentacionLogic;
        }

        public ReporteModel GetCaratulaPorFormulario(string codFormulario, string usr)
        {
            try
            {
                Caratula caratula = _caratulaLogic.GetReportePorFormulario(codFormulario, usr);
                var reporte = new ReporteModel
                {
                    NroTramite = caratula.NroTramite,
                    CodigoFormulario = codFormulario,
                    NroFormulario = caratula.NroFormulario,
                    NroPresentacion = caratula.NroPresentacion,
                    Urgente = caratula.Urgente,
                    Denominacion = "",
                    TipoSocietario = "",
                    NroCorrelativo = 0,
                    NroOperacion = caratula.Operacion.NroOperacion
                };

                if (caratula.Sociedad != null)
                { 
                    reporte.Denominacion = caratula.Sociedad.Nombre;
                    reporte.TipoSocietario = caratula.Sociedad.TipoSociedad.Descripcion;
                    reporte.NroCorrelativo = caratula.Sociedad.NroCorrelativo;
                }

                return reporte;
            }
            catch (Exception ex)
            {
                return new ReporteModel
                {
                    Resultado = 1,
                    Mensaje = ex.Message
                };
            }
        }

        public ReporteModel GetCaratulaPorTramite(int nroTramite, string usr)
        {
            try
            {
                Caratula caratula = _caratulaLogic.GetReportePorTramite(nroTramite, usr);
                var reporte = new ReporteModel
                {
                    Resultado = 0,
                    Mensaje = "Caratula Reimpresa",
                    NroTramite = caratula.NroTramite,
                    NroFormulario = caratula.NroFormulario,
                    NroPresentacion = caratula.NroPresentacion,
                    Urgente = caratula.Urgente,
                    Denominacion = "",
                    TipoSocietario = "",
                    NroCorrelativo = 0,
                    NroOperacion = caratula.Operacion.NroOperacion
                };

                if (caratula.Sociedad != null)
                {
                    reporte.Denominacion = caratula.Sociedad.Nombre;
                    reporte.TipoSocietario = caratula.Sociedad.TipoSociedad.Descripcion;
                    reporte.NroCorrelativo = caratula.Sociedad.NroCorrelativo;
                }

                return reporte;
            }
            catch (Exception ex)
            {
                return new ReporteModel
                {
                    Resultado = 1,
                    Mensaje = ex.Message
                };
            } 
        }

        public ReporteModel GetCaratulaPorOperacion(long codOperacion)
        {
            try
            {
                var caratula = _caratulaLogic.GetReportePorOperacion(codOperacion);
                
                var reporte = new ReporteModel
                {
                    Resultado = 0,
                    NroTramite = caratula.NroTramite,
                    NroFormulario = caratula.NroFormulario,
                    NroPresentacion = caratula.NroPresentacion,
                    Urgente = caratula.Urgente,
                    Denominacion = "",
                    TipoSocietario = "",
                    NroCorrelativo = 0,
                    NroOperacion = caratula.Operacion.NroOperacion
                };

                if (caratula.Sociedad != null)
                {
                    reporte.Denominacion = caratula.Sociedad.Nombre;
                    reporte.TipoSocietario = caratula.Sociedad.TipoSociedad.Descripcion;
                    reporte.NroCorrelativo = caratula.Sociedad.NroCorrelativo;
                }

                return reporte;
            }
            catch (Exception ex)
            {
                return new ReporteModel
                {
                    Resultado = 1,
                    Mensaje = ex.Message
                };
            } 
        }
    }
}
