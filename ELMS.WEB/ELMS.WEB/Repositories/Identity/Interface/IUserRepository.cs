using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Identity.Interface
{
    public interface IUserRepository
    {
        public Task<IList<IdentityUser>> GetAsync();
        public Task<IdentityUser> GetAsync(string email);
        public Task<IdentityUser> GetByUIDAsync(Guid uid);

    }
}