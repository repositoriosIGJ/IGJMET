using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;

namespace Domain.Logics
{
    public class FormularioLogic: IFormularioLogic
    {
        private readonly IFormularioRepositorio _formularioRepositorio;

        public FormularioLogic(IFormularioRepositorio formularioRepositorio)
        {
            _formularioRepositorio = formularioRepositorio;
        }

        public Formulario GetFormulario(string codFormulario)
        {
            return _formularioRepositorio.GetFormulario(codFormulario);
        }

        public Formulario GetFormulario(long nroFormulario)
        {
            return _formularioRepositorio.GetFormulario(nroFormulario);
        }

        public bool EsUrgente(long nroFormulario)
        {
            Formulario formulario = GetFormulario(nroFormulario);
            return formulario.Urgente;
        }
    }
}
