using System.Web.Http;
using WBS.Web.Mappings;

namespace WBS.Web.App_Start
{
    public class Bootstrapper
    {
        public static void Run()
        {
            // Configure Autofac
            AutofacWebapiConfig.Initialize(GlobalConfiguration.Configuration);
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }
    }
}