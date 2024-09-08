using API.Models;
using API.Repos.Dtos;
using API.Repos.Dtos.SourceDto;

namespace API.Repos.Interfaces
{
    public interface ISourceInterface
    {
        Task<IEnumerable<Tblsource>> GetAllSourcesAsync();
        Task<Tblsource> GetSourcesByIdAsync(int id);
        Task<ResponseDto> CreateSourceAsync(CreateSourceDto createSourceDto);
        Task<ResponseDto> EditSourceAsync(CreateSourceDto createSourceDto);
    }
}
