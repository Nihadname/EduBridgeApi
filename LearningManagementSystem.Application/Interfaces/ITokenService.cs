using LearningManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Application.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(string SecretKey, string Audience, string Issuer, AppUser existUser, IList<string> roles);

    }
}
