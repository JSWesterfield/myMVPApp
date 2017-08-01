namespace WebApplication1.Services
{
    public class BaseService
    {
        protected static IDao DataProvider
        {

            get { return Stark.Data.DataProvider.Instance; }
        }

        protected static SqlConnection GetConnection()
        {
            return new System.Data.SqlClient.SqlConnection(
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);


        }

        protected static IPrincipal User { get { return HttpContext.Current.User; } }
    }
}