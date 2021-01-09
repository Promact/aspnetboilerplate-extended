using Abp.MultiTenancy;
using BoilerPlateDemo_App.Authorization.Users;

namespace BoilerPlateDemo_App.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
