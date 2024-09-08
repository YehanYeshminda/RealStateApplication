using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.SourceDto;
using API.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repos.Services
{
    public class SourceService : ISourceInterface
    {
        private readonly CRMContext _db;
        private readonly ICompanyInterface _companyInterface;
        private readonly ResponseDto _response;

        public SourceService(CRMContext context, ICompanyInterface companyInterface)
        {
            _db = context;
            _companyInterface = companyInterface;
            _response = new ResponseDto();
        }

        public async Task<ResponseDto> CreateSourceAsync(CreateSourceDto createSourceDto)
        {
            try
            {
                var existingCompany = await _companyInterface.GetCompanyByIdAsync(Convert.ToInt32(createSourceDto.Cid));

                if (existingCompany == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Company with this Id does not exist";
                    return _response;
                }

                var existingSource = await _db.Tblsources.FirstOrDefaultAsync(x => x.Source == createSourceDto.Source);

                if (existingSource != null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Source with this name already exists";
                    return _response;
                }

                var newSource = new Tblsource
                {
                    Cid = existingCompany.Id.ToString(),
                    Remark = createSourceDto.Remark,
                    Source = createSourceDto.Source,
                    Status = createSourceDto.Status,
                };

                await _db.Tblsources.AddAsync(newSource);
                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Source Saved Successfully";
                _response.Result = newSource;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }


        public async Task<ResponseDto> EditSourceAsync(CreateSourceDto createSourceDto)
        {
            try
            {
                var existingCompany = await _companyInterface.GetCompanyByIdAsync(Convert.ToInt32(createSourceDto.Cid));

                if (existingCompany == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Company with this Id does not exist";
                    return _response;
                }

                var existingSource = await _db.Tblsources.FirstOrDefaultAsync(x => x.Id == createSourceDto.Id);

                if (existingSource == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Source with this Id does not exist";
                    return _response;
                }

                existingSource.Cid = existingCompany.Id.ToString();
                existingSource.Remark = createSourceDto.Remark;
                existingSource.Source = createSourceDto.Source;
                existingSource.Status = createSourceDto.Status;

                await _db.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Source Edited Successfully";
                _response.Result = existingSource;
                return _response;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                return _response;
            }
        }


        public async Task<IEnumerable<Tblsource>> GetAllSourcesAsync()
        {
            return await _db.Tblsources.ToListAsync();
        }

        public async Task<Tblsource> GetSourcesByIdAsync(int id)
        {
            return await _db.Tblsources.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
