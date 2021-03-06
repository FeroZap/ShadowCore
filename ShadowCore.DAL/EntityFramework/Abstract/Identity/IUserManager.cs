﻿using System.Threading.Tasks;
using ShadowCore.Models.EntityFramework.Domain;
using ShadowTools.AutomaticDI.Interfaces;

namespace ShadowCore.DAL.EntityFramework.Abstract.Identity
{
    public interface IUserManager : IScopedLifetime
    {
        Task CreateAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<User> FindByEmailAsync(string email);
    }
}
