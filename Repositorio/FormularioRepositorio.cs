using System;
using System.Collections.Generic;
using System.Linq;
using Oracle.DataAccess.Client;
using Domain;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using DAO;
using DAO.Mappers;

namespace Repositorio
{
    public class FormularioRepositorio: IFormularioRepositorio
    {
        public bool Existe(Formulario formulario)
        {
            return Existe(formulario.NroFormulario);
        }

        public bool Existe(long nroFormulario)
        {
            using (var context = new MetModel())
            {
                var contador = context.TRANSACCION.Count(t => t.IDTRANSACCION == nroFormulario);
                return contador == 1;
            }

        }

        public Formulario GetFormulario(string codFormulario)
        {
            using (var context = new MetModel())
            {
                var transaccion = context.TRANSACCION
                    .Where(t => String.Compare(t.CODBARRA, codFormulario) == 0)
                    .SingleOrDefault();

                if (transaccion == null)
                    throw new FormularioNotExistException("Formulario inexistente en el sistema de Formularios Digitales");

                Formulario formulario = new FormularioMapper().Map(transaccion);

                //Se verifica el tipo codificado

                var sql = "SELECT TABCONTEN1 FROM TABGEN WHERE TABTIPOTAB = '016' AND TABCLAVE = :1";

                OracleParameter[] parameters = new OracleParameter[1];
                parameters[0] = new OracleParameter { ParameterName = "paramTABCLAVE", Value = formulario.TipoFormulario };

                formulario.TipoDecodificado = context.Database.SqlQuery<string>(sql, parameters).SingleOrDefault();

                return formulario;
            }
        }

        public Formulario GetFormulario(long nroFormulario)
        {
            using (var context = new MetModel())
            {
                var formulario = context.TRANSACCION
                    .Where(t => t.IDTRANSACCION == nroFormulario)
                    .SingleOrDefault();

                if (formulario == null)
                    throw new FormularioNotExistException("Formulario inexistente en el sistema de Formularios Digitales");

                return new FormularioMapper().Map(formulario);
            }
        }

        public void Guardar(Formulario formulario)
        {
            using (var context = new MetModel())
            {
                TRANSACCION transaccion = new FormularioMapper().Reverse(formulario);
                context.TRANSACCION.Add(transaccion);
                context.SaveChanges();
            }
        }

        public void Eliminar(Formulario formulario)
        {
            using (var context = new MetModel())
            {
                var entidad = context.TRANSACCION
                    .Where(t => String.Compare(t.CODBARRA, formulario.CodBarra) == 0)
                    .SingleOrDefault();

                if (entidad != null)
                {
                    context.TRANSACCION.Remove(entidad);
                    context.SaveChanges();
                }
            }
        }
    }
}
