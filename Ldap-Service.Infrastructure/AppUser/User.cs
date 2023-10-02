using Ldap_Service.Domain;
using Microsoft.Extensions.Caching.Distributed;
namespace Ldap_Service.Infrastructure;


public class CurrentUserInfrastructure: ICurrentUserInfrastructure
{
    
    readonly ICacheProvider _cacheProvider;
    
    public CurrentUserInfrastructure(ICacheProvider cacheProvider)
    {
       
        _cacheProvider = cacheProvider;
    }

    public async Task<User> GetCurrentUser(string reference)
    {
        
       return await _cacheProvider.GetFromCache<User>(reference);
        
    }
     public async Task<User> SaveCurrentUser(User user)
    {
        var options = new DistributedCacheEntryOptions(); // create options object
        options.AbsoluteExpiration = DateTime.UtcNow.AddHours(24); // 1 minutes
        await _cacheProvider.SetCache<User>(user.Reference, user,options);
        return user;
    }
}