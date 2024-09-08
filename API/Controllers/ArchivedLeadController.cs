using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.ArchivedLead;
using API.Repos.Interfaces;
using API.Repos.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/archivedLeads")]
    [ApiController]
    public class ArchivedLeadController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CRMContext _context;
        private readonly IConfiguration _configuration;
        private readonly ResponseDto _response;
        
        public ArchivedLeadController(IUnitOfWork unitOfWork, CRMContext context, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _configuration = configuration;
            _response = new ResponseDto();
        }

        [HttpPost("GetAllArchivedLeads")]
        public ResponseDto GetAllArchivedLeads(ArchivedLeadGetAllDto archivedLeadGetAllDto)
        {
            var authResponse = _unitOfWork.authenticationService.ValidateAuthentication(archivedLeadGetAllDto.authDto);

            if (!authResponse.IsSuccess)
            {
                _response.Message = authResponse.Message;
                _response.IsSuccess = authResponse.IsSuccess;
                _response.Result = authResponse.Result;
                return _response;
            }

            try
            {
                LeadsService leadsService = new LeadsService(_context, _configuration);

                var archivedLeads = leadsService.GetAllDndLeads();
                
                var totalCount = archivedLeads.Count();
                var totalPages = (int)Math.Ceiling((decimal)totalCount / totalCount);
                var leadsPerPage = archivedLeads.Skip((archivedLeadGetAllDto.Page - 1) * archivedLeadGetAllDto.PageSize).Take(archivedLeadGetAllDto.PageSize).ToList();

                _response.IsSuccess = true;
                _response.Message = "";
                _response.Result = new
                {
                    data = leadsPerPage,
                    totalCountPages = totalPages,
                    totalData = totalCount
                };

                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = "Error while getting archived leads. " + ex.Message;
                return _response;
            }
        }
    }
}
