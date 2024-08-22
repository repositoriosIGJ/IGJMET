using System;
using System.Collections.Generic;

namespace Domain.Contracts.Mappers
{
    public interface IMapper<Domain,Entidad>
    {
        Domain Map(Entidad entidad);
        Entidad Reverse(Domain Domain);
        void Reverse(Entidad entidad, Domain Domain);
    }
}
