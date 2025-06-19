using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
namespace Spark.Web.Utilities;
public class TokenParser
{
    public string[] ClientScope;
    public TokenParser(string token)
    {
       var handler = new JwtSecurityTokenHandler();

        if (handler.CanReadToken(token))
        {
            var jwtToken = handler.ReadJwtToken(token);
            var scopeClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "scope");
            if (scopeClaim != null)
            {
                Console.WriteLine("Scope: " + scopeClaim.Value);
                // If multiple scopes are space-separated, you can split them:
                var scopes = scopeClaim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                ClientScope = scopes;
            }
            else
            {
                ClientScope = [];
            }
        }
        else
        {
            ClientScope = [];
        } 
    }
    
}