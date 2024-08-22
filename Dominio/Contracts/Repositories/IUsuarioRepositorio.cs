using System;
using System.Collections.Generic;

namespace Domain.Contracts.Repositories
{
    public interface IUsuarioRepositorio
    {
        Usuario GetUsuario(string nombreUsuaro);
        IList<Usuario> GetAvailables();
    }
}
