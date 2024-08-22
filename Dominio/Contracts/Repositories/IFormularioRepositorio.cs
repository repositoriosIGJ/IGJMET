using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface IFormularioRepositorio
    {
        bool Existe(Formulario formulario);
        bool Existe(long nroFormulario);
        Formulario GetFormulario(long nroFormulario);
        Formulario GetFormulario(string codFormulario);
        void Guardar(Formulario formulario);
        void Eliminar(Formulario formulario);
    }
}
