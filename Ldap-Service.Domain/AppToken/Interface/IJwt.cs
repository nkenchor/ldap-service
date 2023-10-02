namespace Ldap_Service.Domain;

public interface IJwtInfrastructure{
      
      Jwt CreateJwt(User user);  
}