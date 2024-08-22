using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface ITimbradoRepositorio
    {
        IList<Timbrado> GetTimbrados(Formulario formulario);
        IList<Timbrado> GetTimbrados(int nroTramite);
        bool Existe(Timbrado timbrado);
        
        void Guardar(Timbrado timbrado);
        void Eliminar(Timbrado timbrado);
        int ObtenerPagoElectronico();
        void Actualizotimbrado(Timbrado timbrado);
    }
}
