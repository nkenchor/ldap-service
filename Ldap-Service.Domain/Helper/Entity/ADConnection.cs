using System.DirectoryServices.Protocols;
using System.Net;
namespace Ldap_Service.Domain;
public class ADConnection
   {
      
      public string HostIP { get; set; }
      public int Port { get; set; }
      public int AuthenticationType { get; set; }
      public int ProtocolVersion { get; set; }
      public string DomainName { get; set; }
      public string Suffix{get;set;}
      public string GroupCode { get; set; }

    
   }