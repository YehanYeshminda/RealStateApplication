using API.Models;

namespace API.Repos.Interfaces
{
    public interface IMediaInterface
    {
        Task<IEnumerable<TblMedium>> GetAllMedia();
        Task<TblMedium> GetMediaWithId(int id);

        Task<TblMediaLink> GetMedialinkWithId(int id);
    }
}
