using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Services
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, string userName,
        IList<string> roles);
    }

}
