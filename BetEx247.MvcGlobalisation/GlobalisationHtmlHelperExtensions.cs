using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BetEx247.MvcGlobalisation
{
    public static class GlobalisationHtmlHelperExtensions
    {
        static void AddOtherValues(RouteData routeData, RouteValueDictionary destinationRoute)
        {
            foreach (var routeInformation in routeData.Values)
            {
                if (routeInformation.Key == GlobalisedRoute.CultureKey)
                    continue; //Do not re-add, it will throw, this is the old value anyway.
                destinationRoute.Add(routeInformation.Key, routeInformation.Value);
            }
        }

        public static MvcHtmlString GlobalisedRouteLink(this HtmlHelper htmlHelper, string linkText, string targetCultureName, RouteData routeData)
        {
            RouteValueDictionary globalisedRouteData = new RouteValueDictionary();
            globalisedRouteData.Add(GlobalisedRoute.CultureKey, targetCultureName);
            AddOtherValues(routeData, globalisedRouteData);
            return htmlHelper.RouteLink(linkText, globalisedRouteData);
        }
    }
}
