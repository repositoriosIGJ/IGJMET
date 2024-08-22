using System.Text;
using System.Web.Mvc;
using Domain.Contracts.Common;
using Utils;

namespace Web.Filters
{
    public class LoggerFilterAttribute: ActionFilterAttribute
    {
        private readonly ILogger _logger = new Logger();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var request = filterContext.HttpContext.Request;
            var sb = new StringBuilder();
            sb.AppendFormat("{0}-", request.RequestType);
            sb.AppendFormat("{0}-", request.RawUrl);
            sb.AppendFormat("{0}-", request.UserHostAddress);
            sb.AppendFormat("{0}-", request.UserAgent);
            sb.AppendFormat("{0}", filterContext.HttpContext.User.Identity.Name);

            _logger.Info(sb.ToString());
        }
    }
}