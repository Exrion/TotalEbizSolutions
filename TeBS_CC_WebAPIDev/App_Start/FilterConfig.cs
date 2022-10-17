using System.Web;
using System.Web.Mvc;

namespace TeBS_CC_WebAPIDev
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
