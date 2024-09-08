using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        public HistoryController()
        {
            
        }

        [HttpGet]
        public ActionResult GetAllData()
        {
            return Ok();
        }
    }
}
