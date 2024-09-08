using API.Models;
using API.Repos.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/branchs")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly CRMContext _db;
        private readonly ResponseDto _response;

        public BranchController(CRMContext context)
        {
            _db = context;
            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tblbranch>>> GetAllBranches()
        {
            try
            {
                var existingBranches = await _db.Tblbranches.ToListAsync();
                return existingBranches;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
