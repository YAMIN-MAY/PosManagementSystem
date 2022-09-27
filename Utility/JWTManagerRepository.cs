using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetTestSolution.Domain.Context;
using NetTestSolution.Domain.Models;

namespace NetTestSolution.Utility
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
		private readonly IConfiguration iconfiguration;

		public JWTManagerRepository(IConfiguration configuration)
		{
			iconfiguration = configuration;
		}

        public string GenerateJWTTokens(AuthenticateRequestModel user)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
				  {
				 new Claim(ClaimTypes.Name, user.Password)
				  }),
					Expires = DateTime.Now.AddDays(1),
					SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
				};
				var token = tokenHandler.CreateToken(tokenDescriptor);
				return tokenHandler.WriteToken(token);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

	}
}
