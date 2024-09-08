namespace API.Repos.Interfaces
{
    public interface IPaymentScheduleInterface
    {
        Task<IEnumerable<dynamic>> GetAllViewPaymentScheduleAsync();
        Task<IEnumerable<dynamic>> GetComboDataVExpenses();
    }
}
