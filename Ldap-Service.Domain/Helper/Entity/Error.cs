namespace Ldap_Service.Domain;


public class Error:Exception
{
    public string Reference{ get; set; }
    public string ErrorType{ get; set; }
    public DateTime TimeStamp{ get; set; }

    
    public Error(string errorType, string errorMessage):base(errorMessage)
    {
        Reference = Guid.NewGuid().ToString();
        ErrorType = errorType;
        TimeStamp = DateTime.Now;
        

    }
}