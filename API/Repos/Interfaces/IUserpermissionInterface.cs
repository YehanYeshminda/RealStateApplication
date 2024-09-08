using API.Models;
using API.Repos.Dtos.UserDtos;
using API.Repos.Dtos.UserPermissionDtos;

namespace API.Repos.Interfaces
{
    public interface IUserpermissionInterface
    {
        Task<IEnumerable<Tbluserpermission>> GetAllUPAsync();
        Task<Tbluserpermission> GetUPByIdAsync(int id);
        Task<GetUserPermission> GetUserPermission(SendGetUserPermission sendGetUserPermission, string userId);
        Task<IEnumerable<GetAllUserPermission>> GetAllUserPermissionById(string userId);
        Task UpdateUserPermissionAsync(UpdateUserPermissionMultiDto updateUserPermissionMultiDto);
        Task UpdateUserPermissionDesignationAsync(UpdateUserPermissionMultiDto updateUserPermissionMultiDto);
        Task<Tbluserpermission> GetUserPermissionByLocationEvent(string accessLocation, string eventDone);
        Task<IEnumerable<GetAllUserPermission>> GetDesignationById(string designationId);
    }
}
