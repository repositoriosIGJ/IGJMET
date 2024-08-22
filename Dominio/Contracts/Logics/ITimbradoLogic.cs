using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Logics
{
    public interface ITimbradoLogic
    {
        Timbrado GetUltimoProcesado(Formulario formulario);
        Timbrado GetUltimoProcesado(Formulario formulario, int nroTramite);
        Timbrado GetUltimoValido(Int32 nroTramite);
        IList<Timbrado> GetTimbrados(int nroTramite);
        void AgregarTimbrado(string codFormulario);
        void Guardar(Timbrado timbrado);
        int ObtenerPagoElectronico();
        void Actualizotimbrado(Timbrado timbrado);

        Timbrado GetTimbradoActivo(int nroTramite);
    }
}
