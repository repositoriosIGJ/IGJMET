using System;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Domain.Contracts.Repositories;
using Repositorio;
using Utils;

namespace Web.Filters
{
    public class AuthorizeGroupAttribute : AuthorizeAttribute
    {
        private string _controller;
        private string _action;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public AuthorizeGroupAttribute()
            :this("","")
        {

        }

        public AuthorizeGroupAttribute(string controller, string action)
        {
            _action = action;
            _controller = controller;
            _usuarioRepositorio = new UsuarioRepositorio(new Logger());
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext)) 
                return false;

            //GetCurrentAction(httpContext);
            //var roles = PermissionHelper.Instance.GetActionRoles(_controller, _action);
            //return !roles.Any() && roles.Any(httpContext.User.IsInRole);
            var usuario = httpContext.User.Identity.Name.Contains("\\") ? httpContext.User.Identity.Name.Split('\\')[1] : httpContext.User.Identity.Name;
            return CheckUserDB(usuario);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
                filterContext.Result = new ViewResult
                {
                    ViewName = "Unauthorized"
                };
            else
                base.HandleUnauthorizedRequest(filterContext);
        }

        private void GetCurrentAction(HttpContextBase httpContext)
        {
            if (!String.IsNullOrEmpty(_action) && !String.IsNullOrEmpty(_controller))
                return;

            var routes = httpContext.Request.RequestContext.RouteData.Values;
            _action = routes.ContainsKey("action") ? routes["action"].ToString(): "";
            _controller = routes.ContainsKey("controller") ? routes["controller"].ToString() : "";
        }

        private bool CheckUserDB(string userName)
        {
            return _usuarioRepositorio.GetUsuario(userName) != null;
        }
    }
}