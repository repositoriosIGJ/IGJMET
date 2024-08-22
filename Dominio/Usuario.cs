using System;
using Domain.Entities;

namespace Domain
{
    public class Usuario: Entidad
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Alias { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }

        public string GetNombreYApellido()
        {
            return string.Format("{0} {1}", Nombre, Apellido);
        }

        public string GetNombreYApellidoFormal()
        {
            return string.Format("{0}, {1}", Apellido, Nombre);
        }
    }
}
