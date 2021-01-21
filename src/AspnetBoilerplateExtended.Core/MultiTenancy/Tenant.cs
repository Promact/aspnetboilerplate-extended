using Abp.MultiTenancy;
using AspnetBoilerplateExtended.Authorization.Users;

namespace AspnetBoilerplateExtended.MultiTenancy
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
