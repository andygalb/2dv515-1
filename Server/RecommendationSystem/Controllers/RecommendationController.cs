using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RecommendationSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/recommendation")]
    public class RecommendationController : Controller
    {

        private IUserService userService;
        private IRecommendationService recommendationService;

        public RecommendationController(IUserService userService, IRecommendationService recommendationService)
        {
            this.userService = userService;
            this.recommendationService = recommendationService;
        }


        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            return Json(recommendationService.GetRecommendations(id, userService));
        }
    }
}

