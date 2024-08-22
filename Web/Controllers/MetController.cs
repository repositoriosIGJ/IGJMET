using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Domain.Contracts.Common;
using Utils;
using Web.Filters;

namespace Web.Controllers
{
    [AuthorizeGroupAttribute]
    public class MetController : Controller
    {
        protected readonly ILogger _logger;

        protected MetController(ILogger logger)
        {
            _logger = logger;
        }

        protected virtual bool ValidateToken(object[] requestParams, string token)
        {
            return ValidateToken(token);
        }

        protected bool ValidateToken(string token)
        {
            try
            {
                if (!String.IsNullOrEmpty(token))
                {
                    var cookieToken = "";
                    var formToken = "";
                    var tokens = token.Split(':');
                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                    AntiForgery.Validate(cookieToken, formToken);
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        protected bool ValidateUrlToken(string token)
        {
            if (String.IsNullOrEmpty(token))
                return false;

            var url = Request.Params.ToString();
            url = url.Substring(0, url.IndexOf("&token"));
            var signature = HashCode.GetHashCodeFromString(url);

            return String.Compare(token, signature, true) == 0;
        }

        protected bool ValidateTimestamp(double timestamp)
        {
            var date = Timers.ConverFormUnixTimestamp(timestamp);
            return date.Minute <= 10;
        }

        protected String GetOperador()
        {
            return User.Identity.Name.Contains("\\") ? User.Identity.Name.Split('\\')[1] : User.Identity.Name;
        }
    }
}
