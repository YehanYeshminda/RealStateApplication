using API.Models;
using API.Repos.Branch;
using API.Repos.Chart;
using API.Repos.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace API.Repos.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CRMContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UnitOfWork(CRMContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _db = context;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }


        public ILeadStatusInterface leadStatusInterface => new LeadStatusService(_db);
        public ILeadsInterface leadsInterface => new LeadsService(_db, _configuration);

        public ISourceInterface sourceInterface => new SourceService(_db, companyInterface);

        public IStaffInterface staffInterface => new StaffService(_db, _configuration, _webHostEnvironment);

        public ICampaignInterface campaignInterface => new CompaignService(_db);

        public ICompanyInterface companyInterface => new CompanyService(_db);
        public IAuthenticationService authenticationService => new AuthenticationService(_db);

        public ISupplierInterface supplierInterface => new SupplierService(_db);

        public ILeadForwardInterface leadForwardInterface => new LeadForwardService(_db, _configuration);

        public IPreferedContactMethodInterface preferedContactMethodInterface => new PreferedContactMethodService(_db);
        public ICustomerInterface customerInterface =>  new CustomerService(_db);

        public ILeadAssignInterface leadAssignInterface => new LeadAssignService(_db);
        public IIOUInterface iOUInterface => new IouService(_db);

        public IIOUrtnInterface iIOUrtnInterface => new IOUrtnService(_db);

        public ILeadLogInterface leadLogInterface => new LeadLogService(_db, _configuration);
        public IMeetingInterface iMeetingInterface => new MeetingService(_db);

        public IUserInterface userInterface => new UserService(_db);

        public IContactMethodInterface contactMethodInterface => new ContactMethodService(_db);

        public IMediaInterface mediaInterface => new MediaService(_db);

        public IUserpermissionInterface userpermissionInterface => new UserpermissionService(_db, _configuration);

        public IExpenseInterface expenseInterface => new ExpenseService(_db);

        public INotificationInterface notificationInterface => new NotificationService(_db, _configuration);

        public IPropertyRegisterInterface propertyRegisterInterface => new PropertyRegisterService(_db, _configuration);
        public IPaymentScheduleInterface paymentScheduleInterface => new PaymentService(_configuration);

        public IPropDevInterface propDevInterface => new PropDevService(_db, _configuration);

        public IPropAssignInterface propAssignInterface => new PropAssignService(_db);

        public IAdvPaymentInterface advPaymentInterface => new AdvPaymentService(_db);

        public IAgreementReminderInterface agreementReminderInterface => new AgreementReminderService(_db);

        public IVendorToServiceInterface vendorToServiceInterface => new VendorToService(_db, _configuration);

        public ICallListInterface callListInterface => new CallListService(_db, _configuration);

        public IAccountInterface accountInterface => new AccountServices(_db);

        public IRsvpInterface rsvpInterface => new RsvpService(_configuration);
        public IArchviedLeadInterface archviedLeadInterface => new ArchviedLeadService(_db);

        public IChartInterface chartInterface => new ChartService(_configuration);
        public IBranchService BranchService => new BranchService(_db);

        public async Task<bool> Complete()
        {
            return await _db.SaveChangesAsync() > 0;
        }

    }
}
