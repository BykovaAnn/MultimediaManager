using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MultimediaManager.DAO;

namespace MultimediaManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilesManagement fm = new FilesManagement();
            ReportsManagement rm = new ReportsManagement();
            rm.SavePrevFiles();
            fm.ResetFilesDB();
            fm.BuildFileTree();
        }
    }
}
