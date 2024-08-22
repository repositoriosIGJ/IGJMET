using System;
using Domain.Models;

namespace Domain.Contracts.Services
{
    public interface IHerramientaService
    {
        VerificarCaratulaModel VerificarCaratula(string hashcode);
    }
}
