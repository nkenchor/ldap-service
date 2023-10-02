namespace Ldap_Service.Domain;

public interface ICurrentUserInfrastructure{
    Task<User> SaveCurrentUser(User user);
    Task<User> GetCurrentUser(string reference);
    
}

