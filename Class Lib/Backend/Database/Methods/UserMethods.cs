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
            if(newUser == null) { throw new ArgumentNullException(""); }

            if (!user.HasPermission(AccessService.PermissionKey.CreateUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати користувачів.");
            }

            await _userRepository.AddAsync(newUser);
        }

        // Read
        public async Task<IEnumerable<User>> GetByCustomCriteriaAsync(Employee user, Expression<Func<User, bool>> filter)
        {
            if(filter == null) { throw new ArgumentNullException(""); }

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
            if(updatedUser == null) { throw new ArgumentNullException(""); }

            if (!user.HasPermission(AccessService.PermissionKey.CreateUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати користувачів.");
            }

            await _userRepository.UpdateAsync(updatedUser);
        }

        // Delete
        public async Task DeleteAsync(Employee user, User targetUser)
        {
            if(targetUser == null) { throw new ArgumentNullException(""); }

            if (!user.HasPermission(AccessService.PermissionKey.DeleteUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти користувачів.");
            }

            await _userRepository.DeleteAsync(targetUser);
        }


        // Delete by ID
        public async Task DeleteAsync(Employee user, string targetUser)
        {
            if (targetUser == null) { throw new ArgumentNullException(""); }

            if (!user.HasPermission(AccessService.PermissionKey.DeleteUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти користувачів.");
            }

            var users = await _userRepository.Query()
                .Where(x => x.Username == targetUser)
                .ExecuteAsync();
            User target = users.FirstOrDefault();
            if(target == null)
            { throw new ArgumentNullException("Користувача не знайдено."); }

            await _userRepository.DeleteAsync(target);
        }
    }
}
