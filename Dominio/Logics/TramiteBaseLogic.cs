using System;
using System.Collections.Generic;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;

namespace Domain.Logics
{
    public abstract class TramiteBaseLogic
    {
        protected Usuario _usuario;
        protected readonly IOperacionLogic _operacionLogic;

        public TramiteBaseLogic(Usuario usuario, IOperacionLogic operacionLogic)
        {
            _usuario = usuario;
            _operacionLogic = operacionLogic;

        }

        public abstract int Procesar();
    }
}
