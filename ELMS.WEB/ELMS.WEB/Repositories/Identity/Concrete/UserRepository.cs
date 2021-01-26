﻿using ELMS.WEB.Models;
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
    }
}