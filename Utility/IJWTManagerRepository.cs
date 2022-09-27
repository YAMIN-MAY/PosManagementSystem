using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NetTestSolution.Domain.Models;

namespace NetTestSolution.Utility
{
    public interface IJWTManagerRepository
    {
        string GenerateJWTTokens(AuthenticateRequestModel user);

    }
}
