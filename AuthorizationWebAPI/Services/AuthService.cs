using AuthorizationWebAPI.Models;
using AuthorizationWebAPI.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationWebAPI.Services
{
    public class AuthService:IAuthService
    {
        private readonly ICredentialRepo obj;
        public AuthService(ICredentialRepo _obj)
        {
            obj = _obj;
        }
        
        public string GenerateJSONWebToken(Authenticate userInfo, IConfiguration _config)
        {
            if (userInfo == null)
                return null;
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    null,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {
                return null;
            }

        }
        

        public dynamic AuthenticateUser(Authenticate login)
        {
            if (login == null)
            {
                return null;
            }
            try
            {
                Authenticate user = null;

                Dictionary<string, string> ValidUsersDictionary = obj.GetCredentials();

                if (ValidUsersDictionary == null)
                    return null;
                else
                {
                    if (ValidUsersDictionary.Any(u => u.Key == login.Name && u.Value == login.Password))
                    {
                        user = new Authenticate { Name = login.Name, Password = login.Password };

                    }
                }

                return user;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
