namespace Ldap_Service.Domain;
using System.ComponentModel.DataAnnotations;
public class Credential
   {
      [Required]
      public string UserName { get; set; }
      [Required]
      public string Password { get; set; }

      public Credential(string username, string password)
      {
         UserName = username;
         Password = password;
      }
   }