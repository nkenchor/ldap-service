using System.Xml.Linq;
using System;
using System.DirectoryServices.Protocols;
using System.Net;
using Ldap_Service.Domain;
namespace Ldap_Service.Infrastructure;

public class AuthInfrastructure:IAuthInfrastructure
{
   
    readonly IConfiguration _configuration;
    readonly ADConnection _connection;
    public AuthInfrastructure(IConfiguration configuration, ADConnection connection)
    {
       _configuration = configuration;
       _configuration.GetSection(nameof(ADConnection)).Bind(_connection = connection);
    }
    public User Login(Credential credential)
    {
        return GetUser(Connect(credential), credential);
    }
    public void Logout(string reference)
    {
            throw new NotImplementedException();
    }
   
    #region Authentication Private Methods
     LdapConnection Connect(Credential credential)
    {
            
            
        
            try
            {
                var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_connection.HostIP,_connection.Port));
                
                ldapConnection.AuthType = (AuthType)_connection.AuthenticationType;
                ldapConnection.SessionOptions.ProtocolVersion = _connection.ProtocolVersion;
                ldapConnection.Bind(new NetworkCredential($"{credential.UserName}@{_connection.DomainName}.{_connection.Suffix}",$"{credential.Password}"));
                return ldapConnection;
                
                
            }
            catch (LdapException e)
            {
                throw new Error("LDAP_ERROR",e.Message);
            }
            catch (Exception e)
            {
                throw new Error("SERVER ERROR",e.Message);
            }
    }
     User GetUser(LdapConnection ldapConnection,Credential credential)
    {
               
            var ldapFilter = $"(sAMAccountName={credential.UserName})";
            var attributesToReturn = new string[] {"memberOf", "sn","givenName","title","department","st","mobile","mail" };
           
            var searchRequest = new SearchRequest($"dc={_connection.DomainName},dc={_connection.Suffix}", ldapFilter, SearchScope.Subtree, attributesToReturn);

            try
            {
                
                        using (ldapConnection)
                        {
                            var entry = ((SearchResponse)ldapConnection.SendRequest(searchRequest)).Entries[0];
                            var groups = entry.Attributes["memberOf"][0].ToString()?.Replace("CN=","").Split(",")
                            .Where(item=>item.ToLower().Contains(_connection.GroupCode.ToLower())).ToArray();
                            var isAdmin = groups?.Where(item => item.ToLower().Contains("admin")).Any();
                            return new User (credential.UserName,entry.Attributes["sn"][0].ToString() ?? "",entry.Attributes["givenName"][0].ToString()?? "",
                            entry.Attributes["title"][0].ToString()?? "",entry.Attributes["department"][0].ToString()?? "",
                            groups,entry.Attributes["st"][0].ToString()?? "",
                            entry.Attributes["mail"][0].ToString()?? "",entry.Attributes["mobile"][0].ToString()?? "",
                            isAdmin);
                        }
                
                
            }
            catch (LdapException e)
            {
                throw new Error("LDAP_ERROR",e.Message);
            }
            catch (Exception e)
            {
                throw new Error("SERVER ERROR",e.Message);
            }
        }
    #endregion
}