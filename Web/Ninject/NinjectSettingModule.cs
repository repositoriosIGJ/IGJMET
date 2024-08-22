using Domain;
using Domain.Contracts.Common;
using Domain.Contracts.Logics;
using Domain.Contracts.Repositories;
using Domain.Contracts.Reports;
using Domain.Contracts.Services;
using Domain.Logics;
using Domain.Common;
using Ninject.Modules;
using Ninject.Web.Common;
using Repositorio;
using Gateway;
using Services;
using System.Web;
using Reports;
using Utils;

namespace WebApp.Ninject
{
    public class NinjectSettingModule : NinjectModule
    {
        public override void Load()
        {
            #region Repositories

            Bind<IUsuarioRepositorio>().To<UsuarioRepositorio>().InRequestScope();
            Bind<ITramiteRepositorio>().To<TramiteRepositorio>().InRequestScope();
            Bind<ITipoTramiteRepositorio>().To<TipoTramiteRepositorio>().InRequestScope();
            Bind<ITimbradoRepositorio>().To<TimbradoRepositorio>().InRequestScope();
            Bind<IFormularioRepositorio>().To<FormularioRepositorio>().InRequestScope();
            Bind<IOperacionRepositorio>().To<OperacionRepositorio>().InRequestScope();
            Bind<IPresentacionTramiteRepositorio>().To<PresentacionTramiteRepositorio>().InRequestScope();
            Bind<INomencladorRepositorio>().To<NomencladorRepositorio>().InRequestScope();
            Bind<ISociedadRepositorio>().To<SociedadRepositorio>().InRequestScope();
            Bind<IVistaRepositorio>().To<VistaRepositorio>().InRequestScope();
            Bind<IDestraRepositorio>().To<DestraRepositorio>().InRequestScope();
            Bind<IReimpresionRespositorio>().To<ReimpresionRepositorio>().InRequestScope();
            Bind<IENTEServiceClient>().To<ENTEServiceClient>().InRequestScope();
            Bind<IBPMServiceClient>().To<BPMServiceClient>().InRequestScope();
            Bind<ITipoTramiteRepository>().To<TipoTramiteRepository>().InRequestScope();
            Bind<IGestionTramiteServiceClient>().To<GestionTramiteServiceClient>().InRequestScope();
            
            #endregion Repositories

            #region Services

            Bind<ITramiteService>().To<TramiteService>().InRequestScope();
            Bind<IReporteService>().To<ReporteService>().InRequestScope();
            Bind<ICaratulaService>().To<CaratulaService>().InRequestScope();
            Bind<IHerramientaService>().To<HerramientaService>().InRequestScope();
            Bind<ISociedadService>().To<SociedadService>().InRequestScope();

            #endregion Services

            #region Logics

            Bind<IFormularioLogic>().To<FormularioLogic>().InRequestScope();
            Bind<ITramiteLogic>().To<TramiteLogic>().InRequestScope();
            Bind<ITimbradoLogic>().To<TimbradoLogic>().InRequestScope();
            Bind<IOperacionLogic>().To<OperacionLogic>().InRequestScope();
            Bind<INomencladorLogic>().To<NomencladorLogic>().InRequestScope();
            Bind<IPresentacionTramiteLogic>().To<PresentacionTramiteLogic>().InRequestScope();
            Bind<ISociedadLogic>().To<SociedadLogic>().InRequestScope();
            Bind<IVistaLogic>().To<VistaLogic>().InRequestScope();
            Bind<IDestraLogic>().To<DestraLogic>().InRequestScope();
            Bind<ICaratulaReport>().To<CaratulaReporte>().InRequestScope();
            Bind<ICaratulaLogic>().To<CaratulaLogic>().InRequestScope();
            Bind<IModuloTramiteLogic>().To<ModuloTramiteLogic>().InRequestScope();

            #endregion Logics

            #region DbContext

            #endregion DbContext

            #region Commons

            Bind<ICacheStore<Usuario>>().To<CacheStore<Usuario>>().InSingletonScope();
            Bind<ILogger>().To<Logger>().InSingletonScope();

            #endregion Commons

            #region Workers

            #endregion Workers
        }
    }
}