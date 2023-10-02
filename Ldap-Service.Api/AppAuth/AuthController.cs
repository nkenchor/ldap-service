
using Microsoft.AspNetCore.Authorization;
using System.DirectoryServices;
using Microsoft.AspNetCore.Mvc;
using Ldap_Service.Domain;

namespace Ldap_Service.Api;
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        readonly IJwtInfrastructure _tokenInfrastructure ;
        readonly IAuthInfrastructure _authInfrastructure ; 
        public AuthController(IJwtInfrastructure tokenInfrastructure, IAuthInfrastructure authInfrastructure) {
                _tokenInfrastructure = tokenInfrastructure;
                _authInfrastructure = authInfrastructure;
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<Jwt> GetToken([FromBody] Credential credential)
        {
            try
            {
                    
                    return _tokenInfrastructure.CreateJwt(_authInfrastructure.Login(credential));
            }
            catch(Error e)
            {
                
                return BadRequest( new Error(e.ErrorType,e.Message) );
            }
        }
        
     
    }

