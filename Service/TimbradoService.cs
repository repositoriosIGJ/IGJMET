using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Contracts.Logics;
using Domain.Contracts.Services;

namespace Services
{
    public class TimbradoService: ITimbradoService
    {
        private readonly ITimbradoLogic _timbradoLogic;

        public TimbradoService(ITimbradoLogic timbradoLogic)
        {
            _timbradoLogic = timbradoLogic;
        }

        public Timbrado GetPorFormulario(Formulario formulario)
        {
            return _timbradoLogic.GetUltimoProcesado(formulario);
        }
    }
}
