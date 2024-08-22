using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;
using Ninject.Web;
using Ninject.Web.Mvc;

namespace WebApp.Ninject
{
    public class NinjectMvcDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly NinjectDependencyResolver resolver;

        public NinjectMvcDependencyResolver(IKernel kernel)
        {
            resolver = new NinjectDependencyResolver(kernel);
        }

        public object GetService(Type serviceType)
        {
            return resolver.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return resolver.GetServices(serviceType);
        }
    }
}