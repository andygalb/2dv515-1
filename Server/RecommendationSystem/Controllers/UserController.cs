using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RecommendationSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Json(userService.GetUsers());
        }
    }
}