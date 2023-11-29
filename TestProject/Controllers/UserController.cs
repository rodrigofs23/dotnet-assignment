using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;

namespace Work.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException("TODO");
        }

        public IActionResult Post(UserModelDto user)
        {
            throw new NotImplementedException("TODO");
        }
        
        public IActionResult Put(UserModelDto user)
        {
            throw new NotImplementedException("TODO");
        }

        public IActionResult Delete(Guid id)
        {
            throw new NotImplementedException("TODO");
        }

    }
}