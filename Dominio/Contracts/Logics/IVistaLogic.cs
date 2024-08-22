using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Logics
{
    public interface IVistaLogic
    {
        bool VistaFinalizada(int nroTramite, int nroCorrelativo);
        Sociedad VerificarContestacionVista(Tramite tramite);

        bool TramiteArchivado(int nroTramite);
    }
}
