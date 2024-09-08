using API.Repos.Branch;

namespace API.Repos.Interfaces
{
    public interface IUnitOfWork
    {
        ILeadsInterface leadsInterface { get; }
        ISourceInterface sourceInterface { get; }
        ISupplierInterface supplierInterface { get; }
        IStaffInterface staffInterface { get; }
        ICampaignInterface campaignInterface { get; }
        ICompanyInterface companyInterface { get; }
        IAuthenticationService authenticationService { get; }
        ILeadForwardInterface leadForwardInterface { get; }
        ILeadStatusInterface leadStatusInterface { get; }
        IPreferedContactMethodInterface preferedContactMethodInterface { get; }
        ICustomerInterface customerInterface { get; }
        ILeadAssignInterface leadAssignInterface { get; }
        IIOUInterface iOUInterface { get; }
        IIOUrtnInterface iIOUrtnInterface { get; }
        ILeadLogInterface leadLogInterface { get; }
        IMeetingInterface iMeetingInterface { get; }
        IUserInterface userInterface { get; }
        IContactMethodInterface contactMethodInterface { get; }
        IMediaInterface mediaInterface { get; }
        IUserpermissionInterface userpermissionInterface { get; }
        IExpenseInterface expenseInterface { get; }
        INotificationInterface notificationInterface { get; }
        IPropertyRegisterInterface propertyRegisterInterface { get; }
        IPaymentScheduleInterface paymentScheduleInterface { get; }
        IPropDevInterface propDevInterface { get; }
        IPropAssignInterface propAssignInterface { get; }
        IAdvPaymentInterface advPaymentInterface { get; }
        IAgreementReminderInterface agreementReminderInterface { get; }
        IVendorToServiceInterface vendorToServiceInterface { get; }
        ICallListInterface callListInterface { get; }
        IAccountInterface accountInterface { get; }
        IRsvpInterface rsvpInterface { get; }
        IArchviedLeadInterface archviedLeadInterface { get; }
        IChartInterface chartInterface { get; }
        IBranchService BranchService { get; }
        Task<bool> Complete();
    }
}
