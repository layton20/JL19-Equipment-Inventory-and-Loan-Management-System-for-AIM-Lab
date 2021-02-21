using ELMS.WEB.Models;
using ELMS.WEB.Repositories.Identity.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Identity.Concrete
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext __Context;

        public UserRepository(ApplicationContext context)
        {
            __Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IList<IdentityUser>> GetAsync()
        {
            return await __Context.Users.ToListAsync();
        }

        public async Task<IdentityUser> GetAsync(string email)
        {
            ; return await __Context.Users.FirstOrDefaultAsync(x => x.Email.ToUpper() == email.ToUpper());
        }

        public async Task<IdentityUser> GetByUIDAsync(Guid uid)
        {
            return await __Context.Users.FindAsync(uid.ToString());
        }
    }
}