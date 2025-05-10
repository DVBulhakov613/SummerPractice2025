using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Person_related.Methods
{
    public class UserMethods
    {
        private readonly UserRepository _userRepository;

        public UserMethods(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Create
        public async Task AddAsync(Employee user, User newUser)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreateUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати користувачів.");
            }

            await _userRepository.AddAsync(newUser);
        }

        // Read
        public async Task<IEnumerable<User>> GetByCustomCriteriaAsync(Employee user, Expression<Func<User, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду користувачів.");
            }

            return await _userRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdateAsync(Employee user, User updatedUser)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreateUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати користувачів.");
            }

            await _userRepository.UpdateAsync(updatedUser);
        }

        // Delete
        public async Task DeleteAsync(Employee user, User targetUser)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeleteUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти користувачів.");
            }

            await _userRepository.DeleteAsync(targetUser);
        }
    }
}
