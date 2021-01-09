using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BoilerPlateDemo_App.Authorization.Users;
using BoilerPlateDemo_App.Editions;

namespace BoilerPlateDemo_App.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
