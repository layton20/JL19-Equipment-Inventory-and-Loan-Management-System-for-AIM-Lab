﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Repositories.Identity.Interface
{
    public interface IUserRepository
    {
        public Task<IList<IdentityUser>> GetAsync();
    }
}