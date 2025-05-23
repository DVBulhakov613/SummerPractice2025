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
        public async Task AddAsync(User user, User newUser)
        {
            ArgumentNullException.ThrowIfNull(newUser);

            if (!user.HasPermission(AccessService.PermissionKey.CreateUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати користувачів.");
            }

            await _userRepository.AddAsync(newUser);
        }

        // Read
        public async Task<IEnumerable<User>> GetByCriteriaAsync(User user, Expression<Func<User, bool>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            if (!user.HasPermission(AccessService.PermissionKey.ReadEmployee))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду користувачів.");
            }

            return await _userRepository.GetByCriteriaAsync(filter);
        }

        // Update
        public async Task UpdateAsync(User user, User updatedUser)
        {
            ArgumentNullException.ThrowIfNull(updatedUser);

            if (!user.HasPermission(AccessService.PermissionKey.CreateUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати користувачів.");
            }

            await _userRepository.UpdateAsync(updatedUser);
        }

        // Delete
        public async Task DeleteAsync(User user, User targetUser)
        {
            ArgumentNullException.ThrowIfNull(targetUser);

            if (!user.HasPermission(AccessService.PermissionKey.DeleteUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти користувачів.");
            }

            await _userRepository.DeleteAsync(targetUser);
        }


        // Delete by ID
        public async Task DeleteAsync(User user, string targetUser)
        {
            ArgumentNullException.ThrowIfNull(targetUser);

            if (!user.HasPermission(AccessService.PermissionKey.DeleteUser))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти користувачів.");
            }

            var target = await _userRepository.GetByUsernameAsync(targetUser) 
                ?? throw new ArgumentNullException(nameof(User), "Користувача не знайдено.");
            await _userRepository.DeleteAsync(target);
        }

        public async Task ChangePasswordAsync(User user, uint userId, string newPassword)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (!user.HasPermission(AccessService.PermissionKey.UpdateUser))
                throw new UnauthorizedAccessException("Немає дозволу змінювати паролі користувачів.");

            var target = await _userRepository.GetByIdAsync(userId) 
                ?? throw new InvalidOperationException("Користувача не знайдено");

            target.PasswordHash = PasswordHelper.HashPassword(newPassword);
            await _userRepository.UpdateAsync(target);
        }

    }
}
