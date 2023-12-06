using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase _mockDatabase;

        public UserRepository(MockDatabase mockDatabase)
        {
            _mockDatabase = mockDatabase ?? throw new ArgumentNullException(nameof(mockDatabase));
        }

        public void Create(User user)
        {
            if (_mockDatabase.Users.ContainsKey(user.UserId))
            {
                throw new ArgumentException("User already exists");
            }
            _mockDatabase.Users.Add(user.UserId, user);
        }

        public User Read(Guid id)
        {
            if (!_mockDatabase.Users.TryGetValue(id, out User user))
            {
                throw new KeyNotFoundException($"User with ID '{id}' not found.");
            }
            return user;
        }

        public void Update(User user)
        {
            if (!_mockDatabase.Users.ContainsKey(user.UserId))
            {
                throw new KeyNotFoundException($"User with ID '{user.UserId}' not found.");
            }
            _mockDatabase.Users[user.UserId] = user;
        }

        public void Remove(User user)
        {
            if (!_mockDatabase.Users.ContainsKey(user.UserId))
            {
                throw new KeyNotFoundException("User not found");
            }
            _mockDatabase.Users.Remove(user.UserId);
        }

    }
}
