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
        public async Task AddUserAsync(Employee user, User newUser)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreateUser"))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати користувачів.");
            }

            await _userRepository.AddAsync(newUser);
        }

        // Read
        public async Task<IEnumerable<User>> GetUsersByCustomCriteriaAsync(Employee user, Expression<Func<User, bool>> filter)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "ReadUser"))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду користувачів.");
            }

            return await _userRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdateUserAsync(Employee user, User updatedUser)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdateUser"))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати користувачів.");
            }

            await _userRepository.UpdateAsync(updatedUser);
        }

        // Delete
        public async Task DeleteUserAsync(Employee user, User targetUser)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeleteUser"))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти користувачів.");
            }

            await _userRepository.DeleteAsync(targetUser);
        }
    }
}
