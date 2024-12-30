using System.Web;
using System.Web.Mvc;

namespace Partie_Api_Amd_Logique_Metier
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
