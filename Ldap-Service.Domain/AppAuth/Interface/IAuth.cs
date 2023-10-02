namespace Ldap_Service.Domain;

public interface IAuthInfrastructure{
      
      User Login(Credential credential);
      void Logout(string username);
      
}

