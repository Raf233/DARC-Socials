using Darc.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darc.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(UserAuth user);
    }
}
