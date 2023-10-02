using System.Linq;
namespace Ldap_Service.Domain;
public class User
   { 
      public string Reference { get; set; }
      public string Username { get; set; }
      public string LastName { get; set; }
      public string FirstName{get;set;}
      public string FullName{get;internal set;} 
      public string Role{get;set;}
      public string Department{get;set;}
      public string[] Groups {get;set;}
      public string Location { get; set; }
      public string Email{get;set;}
      public string Phone{get;set;}
      public bool? IsAdmin { get; set; }
      
     
      public User()
      {
         
      }
      public User(string username, string lastName, string firstName, 
      string role,string department,string[] groups,string location,string email, string phone, bool? isAdmin)
      {
         
         Reference = Guid.NewGuid().ToString();
         Username = username;
         LastName = lastName;
         FirstName = firstName;
         FullName = $"{LastName}, {FirstName}";
         Role = role;
         Department = department;
         Groups = groups;
         Location = location;
         Email = email;
         Phone = phone;
         IsAdmin = isAdmin;
         
      }
     
   }