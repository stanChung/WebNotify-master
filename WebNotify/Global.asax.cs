using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebNotify
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //訂閱Service Broker
            System.Data.SqlClient.SqlDependency.Start(
                System.Configuration.ConfigurationManager.ConnectionStrings["THICOM_SQL"].ConnectionString);
        }

        protected void Application_End()
        {
            //關閉Service Broker的連接
            System.Data.SqlClient.SqlDependency.Stop(
                System.Configuration.ConfigurationManager.ConnectionStrings["THICOM_SQL"].ConnectionString);
        }
    }
}
