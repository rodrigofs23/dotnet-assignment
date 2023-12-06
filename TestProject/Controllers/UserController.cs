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

        private readonly IRepository<User, Guid> _userRepository;

        public UserController(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = _userRepository.Read(id);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(UserModelDto userModelDto)
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

        [HttpPut]
        public IActionResult Put(UserModelDto userModelDto)
        {
            var existingUser = _userRepository.Read(userModelDto.Id);
            if (existingUser == null)
            existingUser.UserName = userModelDto.Name;
            existingUser.Birthday = userModelDto.Birthdate;
            _userRepository.Update(existingUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var userToDelete = _userRepository.Read(id);
            if (userToDelete == null)
            _userRepository.Remove(userToDelete);
            return NoContent();
        }

    }
}