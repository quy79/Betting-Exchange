using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;

namespace BetEx247.MvcGlobalisation
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        // To only allow *supported* cultures as the first part of the route, instead of  anything in the format xx or xx-xx comment the lower method
        // and uncomment this one, and make CultureManager.CultureIsSupported public.
        //public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        //{
        //    if (!values.ContainsKey(parameterName))
        //        return false;
        //    string potentialCultureName = (string)values[parameterName];
        //    return CultureManager.CultureIsSupported(potentialCultureName);
        //}

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey(parameterName))
                return false;
            string potentialCultureName = (string)values[parameterName];
            return CultureFormatChecker.FormattedAsCulture(potentialCultureName);
        }
    }
}
