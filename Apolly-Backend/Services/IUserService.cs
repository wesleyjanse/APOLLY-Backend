using ASP.NET_Core_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_API.Services
{
    public interface IUserService
    {
        Member Authenticate(string username, string password);
    }
}
