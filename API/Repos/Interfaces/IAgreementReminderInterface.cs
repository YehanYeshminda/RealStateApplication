using API.Models;

namespace API.Repos.Interfaces
{
    public interface IAgreementReminderInterface
    {
        Task<IEnumerable<TblAgreementReminder>> GetAllAgreementReminder();

        Task<IEnumerable<dynamic>> GetAllAgreementReminderView();
    }
}
