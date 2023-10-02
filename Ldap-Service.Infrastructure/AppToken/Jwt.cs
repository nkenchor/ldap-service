using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ldap_Service.Domain;
namespace Ldap_Service.Infrastructure;
 
 public  class JwtInfrastructure:IJwtInfrastructure
 {
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserInfrastructure _currentUserInfrastructure;
        public JwtInfrastructure(IConfiguration configuration,ICurrentUserInfrastructure currentUserInfrastructure) 
        {
            _configuration = configuration;
            _currentUserInfrastructure = currentUserInfrastructure;
        }
        public Jwt CreateJwt(User user)
        {
            _currentUserInfrastructure.SaveCurrentUser(user);
            return GenerateJwt(user);
        }
        #region Jwt Private Methods
        Jwt GenerateJwt(User user)
        {
            var token = new Token();
            _configuration.GetSection(nameof(Token)).Bind(token);
            var claims = Claims(user);
            var securityToken = SecurityToken(token,claims);
            var jwt = new Jwt(new JwtSecurityTokenHandler().WriteToken(securityToken),DateTime.UtcNow.AddHours(token.Expiry));
            
            return jwt;
        }
        Claim[] Claims(User user){
            return new[] 
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.Reference),
                new Claim(ClaimTypes.NameIdentifier, user.Username)

            };
        }
        JwtSecurityToken SecurityToken(Token token, Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expirationDate = DateTime.UtcNow.AddHours(token.Expiry);


            return new JwtSecurityToken(audience: token.Audience,
                                                issuer: token.Issuer,
                                                claims: claims,
                                                expires: expirationDate,
                                                signingCredentials: signingCredentials);
        }
        #endregion
 }

