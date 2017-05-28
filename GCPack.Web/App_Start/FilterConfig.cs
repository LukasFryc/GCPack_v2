using System.Web;
using System.Web.Mvc;
using GCPack.Web.Filters;

namespace GCPack.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new GCAuthentization());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
