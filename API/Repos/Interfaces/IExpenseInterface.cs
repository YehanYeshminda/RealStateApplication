using API.Models;

namespace API.Repos.Interfaces
{
    public interface IExpenseInterface
    {
        Task<IEnumerable<Tblexpense>> GetAllExpense();
        Task<IEnumerable<dynamic>> GetAllVExpense();
        Task<Tblexpense> GetExpenseByIdAsync(string id);
        Task<Tblexpensesaccount> GetExpenseaccountByIdAsync(int id);
        Task<Tblemaincat> GetEmaincatByIdAsync(int id);
        Task<Tblesubcat> GetEsubcatByIdAsync(int id);
    }
}
