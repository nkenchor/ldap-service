namespace Ldap_Service.Domain;

   public partial  class Jwt
   {
      public string Token { get; set; }
      public DateTime ExpirationDate { get; set; }

      public Jwt(string token, DateTime expirationDate)
      {
         Token = token;
         ExpirationDate = expirationDate;
      }
      
   }