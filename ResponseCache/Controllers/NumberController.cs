using Microsoft.AspNetCore.Mvc;
using ResponseCache.Service;

namespace ResponseCache.Controllers
{
    
    public class NumberController : Controller
    {
        [HttpGet("number")]
        public IActionResult Get(NumberService numberService)
        {
            var result = numberService.GetRandomNumber();
            return Ok(result);
        }

        [HttpGet("number/cache/")]
        [ResponseCache(Duration = 5)]
        public IActionResult GetCache(NumberService numberService)
        {
            var result = numberService.GetRandomNumber();
            return Ok(result);
        }

        [HttpGet("number/cache/{userid}")]
        [ResponseCache(Duration = 20, VaryByQueryKeys = new string[] { "userId" })]
        public IActionResult GetCacheWithId(NumberService numberService, int id)
        {
            var result = numberService.GetRandomNumber();
            return Ok(result);
        }
    }
}
