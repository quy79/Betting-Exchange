using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BetEx247.Core.Common.Utils
{
    public static class JSONObject
    {
        public static string GetJSONStringFromObject<T>(T obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(obj);
        }

        public static T GetJSONObjectFromString<T>(string value)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(value);
        }
    }
}
