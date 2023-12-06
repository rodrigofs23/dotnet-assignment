using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IRepository<User, Guid> _userRepository;

        public UserController(ILogger<UserController> logger, IRepository<User, Guid> userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _userRepository.Read(id);
            if (user == null)
            {
                _logger.LogInformation("User with ID {UserId} not found", id);
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(UserModelDto userModelDto)
        {
            try
            {
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    UserName = userModelDto.Name,
                    Birthday = userModelDto.Birthdate
                };
                _userRepository.Create(newUser);
                return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(UserModelDto userModelDto)
        {
            var existingUser = _userRepository.Read(userModelDto.Id);
            if (existingUser == null)
            {
                return NotFound();
            }
            existingUser.UserName = userModelDto.Name;
            existingUser.Birthday = userModelDto.Birthdate;
            _userRepository.Update(existingUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var userToDelete = _userRepository.Read(id);
                if (userToDelete == null)
                {
                    return NotFound();
                }
                _userRepository.Remove(userToDelete);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, ex.Message);
            }
        }

    }
}