using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RecommendationSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/rating")]
    public class RatingController : Controller
    {
        private IUserService userService;

        public RatingController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            return Json(userService.GetRatings(id));
        }

        public IActionResult Get()
        {
            return Json(userService.GetRatings(null));
        }
    }
}