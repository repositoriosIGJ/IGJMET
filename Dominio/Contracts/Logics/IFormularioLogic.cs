using System;
using System.Collections.Generic;
using Domain;

namespace Domain.Contracts.Logics
{
    public interface IFormularioLogic
    {
        Formulario GetFormulario(string codFormulario);
        Formulario GetFormulario(long nroFormulario);
        bool EsUrgente(long nroFormulario);
    }
}
