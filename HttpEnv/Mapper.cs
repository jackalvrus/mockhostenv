using System.Web.Hosting;

namespace HttpEnv
{
    public class Mapper
    {
        public string Map(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }
    }
}
