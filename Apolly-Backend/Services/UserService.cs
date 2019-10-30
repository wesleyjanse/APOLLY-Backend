using ASP.NET_Core_API.Helpers;
using ASP.NET_Core_API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_API.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly ApollyContext _memberContext;
        public UserService(IOptions<AppSettings> appSettings, ApollyContext memberContext)
        {
            _appSettings = appSettings.Value; _memberContext = memberContext;
        }

        public Member Authenticate(string username, string password)
        {
            var user = _memberContext.Members.SingleOrDefault(x => x.Username == username && x.Password == password);
            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwttoken
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("MemberID", user.MemberID.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("Username", user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor); 
            user.Token = tokenHandler.WriteToken(token);
            // remove password before returning
            user.Password = null;
            return user;
        }
    }
}