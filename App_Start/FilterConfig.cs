using System.Web;
using System.Web.Mvc;

namespace _20112255.CLDV6212
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
