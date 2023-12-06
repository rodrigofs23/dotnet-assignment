using Work.Database;
using Work.Implementation;

namespace UnitTests;

public class UserRepositoryTests
{
    [Fact]
    public void Create_AddsNewUser_WhenNotExists()
    {
        var mockDatabase = new MockDatabase(new Dictionary<Guid, User>());
        var userRepository = new UserRepository(mockDatabase);
        var newUser = GetUser(Guid.NewGuid(), "John");

        userRepository.Create(newUser);

        Assert.True(mockDatabase.Users.ContainsKey(newUser.UserId));
    }

    [Fact]
    public void Create_ThrowsArgumentException_WhenUserExists()
    {
        var existingUserId = Guid.NewGuid();
        var existingUser = GetUser(existingUserId, "Alice");
        var users = new Dictionary<Guid, User> { { existingUserId, existingUser } };
        var mockDatabase = new MockDatabase(users);
        var userRepository = new UserRepository(mockDatabase);

        var ex = Assert.Throws<ArgumentException>(() => userRepository.Create(existingUser));
        Assert.Equal("User already exists", ex.Message);
    }

    [Fact]
    public void Read_ReturnsUser_WhenUserExists()
    {
        var existingUserId = Guid.NewGuid();
        var existingUser = GetUser(existingUserId, "Alice");
        var users = new Dictionary<Guid, User> { { existingUserId, existingUser } };
        var mockDatabase = new MockDatabase(users);
        var userRepository = new UserRepository(mockDatabase);

        var retrievedUser = userRepository.Read(existingUserId);

        Assert.NotNull(retrievedUser);
        Assert.Equal(existingUser.UserId, retrievedUser.UserId);
    }

    [Fact]
    public void Read_ThrowsKeyNotFoundException_WhenUserNotExists()
    {
        var nonExistingUserId = Guid.NewGuid();
        var mockDatabase = new MockDatabase(new Dictionary<Guid, User>());
        var userRepository = new UserRepository(mockDatabase);

        Assert.Throws<KeyNotFoundException>(() => userRepository.Read(nonExistingUserId));
    }

    [Fact]
    public void Update_UpdatesUser_WhenUserExists()
    {
        var existingUserId = Guid.NewGuid();
        var existingUser = GetUser(existingUserId, "Alice");
        var updatedUser = GetUser(existingUserId, "UpdatedName");
        var users = new Dictionary<Guid, User> { { existingUserId, existingUser } };
        var mockDatabase = new MockDatabase(users);
        var userRepository = new UserRepository(mockDatabase);

        userRepository.Update(updatedUser);

        Assert.Equal(updatedUser.UserName, mockDatabase.Users[existingUserId].UserName);
    }

    [Fact]
    public void Update_ThrowsKeyNotFoundException_WhenUserNotExists()
    {
        var nonExistingUser = GetUser(Guid.NewGuid(), "NotExists");
        var mockDatabase = new MockDatabase(new Dictionary<Guid, User>());
        var userRepository = new UserRepository(mockDatabase);

        Assert.Throws<KeyNotFoundException>(() => userRepository.Update(nonExistingUser));
    }

    [Fact]
    public void Remove_RemovesUser_WhenUserExists()
    {
        var existingUserId = Guid.NewGuid();
        var existingUser = GetUser(existingUserId, "Alice");
        var users = new Dictionary<Guid, User> { { existingUserId, existingUser } };
        var mockDatabase = new MockDatabase(users);
        var userRepository = new UserRepository(mockDatabase);

        userRepository.Remove(existingUser);

        Assert.False(mockDatabase.Users.ContainsKey(existingUserId));
    }

    [Fact]
    public void Remove_ThrowsKeyNotFoundException_WhenUserNotExists()
    {
        var nonExistingUser = GetUser(Guid.NewGuid(), "NotExists");
        var mockDatabase = new MockDatabase(new Dictionary<Guid, User>());
        var userRepository = new UserRepository(mockDatabase);

        Assert.Throws<KeyNotFoundException>(() => userRepository.Remove(nonExistingUser));
    }

    private static User GetUser(Guid existingUserId, String username)
    {
        return new User { UserId = existingUserId, UserName = username, Birthday = DateTime.Now };
    }
}
