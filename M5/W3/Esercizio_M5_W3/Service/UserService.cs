﻿using Esercizio_Pizzeria_In_Forno.Context;
using Esercizio_Pizzeria_In_Forno.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Esercizio_Pizzeria_In_Forno.Service;

namespace Esercizio_Pizzeria_In_Forno.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;


        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Verifica se ci sono ruoli definiti, altrimenti assegna il ruolo di default "User"
            if (user.UserRoles == null || !user.UserRoles.Any())
            {
                var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (defaultRole == null)
                {
                    defaultRole = new Role { Name = "User" };
                    _context.Roles.Add(defaultRole);
                    await _context.SaveChangesAsync();
                }
                user.UserRoles = new List<UserRole> { new UserRole { RoleId = defaultRole.Id, Role = defaultRole } };
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var existingUser = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null) throw new KeyNotFoundException("User not found");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password; // Non criptata

            // Aggiornamento dei ruoli
            _context.UserRoles.RemoveRange(existingUser.UserRoles);
            existingUser.UserRoles = user.UserRoles;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0) throw new ArgumentException("Invalid user ID", nameof(userId));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password); // Password non criptata

            return user;
        }
    }
}
