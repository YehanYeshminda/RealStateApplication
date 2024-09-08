using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API.Models
{
    public partial class CRMContext : DbContext
    {
        public CRMContext()
        {
        }

        public CRMContext(DbContextOptions<CRMContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CPropertyRegisterList> CPropertyRegisterLists { get; set; } = null!;
        public virtual DbSet<TblAdvPayment> TblAdvPayments { get; set; } = null!;
        public virtual DbSet<TblAgreementReminder> TblAgreementReminders { get; set; } = null!;
        public virtual DbSet<TblAgreementtype> TblAgreementtypes { get; set; } = null!;
        public virtual DbSet<TblApprovedBy> TblApprovedBies { get; set; } = null!;
        public virtual DbSet<TblBranchControl> TblBranchControls { get; set; } = null!;
        public virtual DbSet<TblBulkcreditpaymentd> TblBulkcreditpaymentds { get; set; } = null!;
        public virtual DbSet<TblBulkcreditpaymenth> TblBulkcreditpaymenths { get; set; } = null!;
        public virtual DbSet<TblCallInsight> TblCallInsights { get; set; } = null!;
        public virtual DbSet<TblCampainH> TblCampainHs { get; set; } = null!;
        public virtual DbSet<TblCampainMedium> TblCampainMedia { get; set; } = null!;
        public virtual DbSet<TblCitytype> TblCitytypes { get; set; } = null!;
        public virtual DbSet<TblCustomerRequirement> TblCustomerRequirements { get; set; } = null!;
        public virtual DbSet<TblDesignationPermission> TblDesignationPermissions { get; set; } = null!;
        public virtual DbSet<TblDesignationtype> TblDesignationtypes { get; set; } = null!;
        public virtual DbSet<TblDndLead> TblDndLeads { get; set; } = null!;
        public virtual DbSet<TblIou> TblIous { get; set; } = null!;
        public virtual DbSet<TblIourtn> TblIourtns { get; set; } = null!;
        public virtual DbSet<TblIssuedTo> TblIssuedTos { get; set; } = null!;
        public virtual DbSet<TblLeadStatus> TblLeadStatuses { get; set; } = null!;
        public virtual DbSet<TblLeadforward> TblLeadforwards { get; set; } = null!;
        public virtual DbSet<TblLeadlog> TblLeadlogs { get; set; } = null!;
        public virtual DbSet<TblMediaLink> TblMediaLinks { get; set; } = null!;
        public virtual DbSet<TblMedium> TblMedia { get; set; } = null!;
        public virtual DbSet<TblMeeting> TblMeetings { get; set; } = null!;
        public virtual DbSet<TblNotification> TblNotifications { get; set; } = null!;
        public virtual DbSet<TblPlanToDo> TblPlanToDos { get; set; } = null!;
        public virtual DbSet<TblPreferedContactMethod> TblPreferedContactMethods { get; set; } = null!;
        public virtual DbSet<TblPymentschedule> TblPymentschedules { get; set; } = null!;
        public virtual DbSet<TblRsvpType> TblRsvpTypes { get; set; } = null!;
        public virtual DbSet<TblSale> TblSales { get; set; } = null!;
        public virtual DbSet<TblServicetype> TblServicetypes { get; set; } = null!;
        public virtual DbSet<TblStaffperformance> TblStaffperformances { get; set; } = null!;
        public virtual DbSet<TblSupplier> TblSuppliers { get; set; } = null!;
        public virtual DbSet<TblSupportStaff> TblSupportStaffs { get; set; } = null!;
        public virtual DbSet<TblVcampaign> TblVcampaigns { get; set; } = null!;
        public virtual DbSet<TblVmeeting> TblVmeetings { get; set; } = null!;
        public virtual DbSet<TblVstaff> TblVstaffs { get; set; } = null!;
        public virtual DbSet<Tblbank> Tblbanks { get; set; } = null!;
        public virtual DbSet<Tblbankbranch> Tblbankbranches { get; set; } = null!;
        public virtual DbSet<Tblbranch> Tblbranches { get; set; } = null!;
        public virtual DbSet<Tblcompany> Tblcompanies { get; set; } = null!;
        public virtual DbSet<Tblcompanydetail> Tblcompanydetails { get; set; } = null!;
        public virtual DbSet<Tblcontrol> Tblcontrols { get; set; } = null!;
        public virtual DbSet<Tblcurrentacc> Tblcurrentaccs { get; set; } = null!;
        public virtual DbSet<Tblcustomer> Tblcustomers { get; set; } = null!;
        public virtual DbSet<Tblemaincat> Tblemaincats { get; set; } = null!;
        public virtual DbSet<Tblesubcat> Tblesubcats { get; set; } = null!;
        public virtual DbSet<Tblexpense> Tblexpenses { get; set; } = null!;
        public virtual DbSet<Tblexpensesaccount> Tblexpensesaccounts { get; set; } = null!;
        public virtual DbSet<Tbllead> Tblleads { get; set; } = null!;
        public virtual DbSet<TblleadAssign> TblleadAssigns { get; set; } = null!;
        public virtual DbSet<Tblleadlogview> Tblleadlogviews { get; set; } = null!;
        public virtual DbSet<Tblprioritytype> Tblprioritytypes { get; set; } = null!;
        public virtual DbSet<Tblpropassign> Tblpropassigns { get; set; } = null!;
        public virtual DbSet<Tblpropdev> Tblpropdevs { get; set; } = null!;
        public virtual DbSet<TblpropertyCategory> TblpropertyCategories { get; set; } = null!;
        public virtual DbSet<TblpropertySubCategory> TblpropertySubCategories { get; set; } = null!;
        public virtual DbSet<Tblpropertyregister> Tblpropertyregisters { get; set; } = null!;
        public virtual DbSet<Tblpropertytype> Tblpropertytypes { get; set; } = null!;
        public virtual DbSet<Tblreccheque> Tblreccheques { get; set; } = null!;
        public virtual DbSet<Tblsource> Tblsources { get; set; } = null!;
        public virtual DbSet<Tblstaff> Tblstaffs { get; set; } = null!;
        public virtual DbSet<Tbluser> Tblusers { get; set; } = null!;
        public virtual DbSet<Tblusericon> Tblusericons { get; set; } = null!;
        public virtual DbSet<Tbluserlevel> Tbluserlevels { get; set; } = null!;
        public virtual DbSet<Tbluserpermission> Tbluserpermissions { get; set; } = null!;
        public virtual DbSet<TblvAdvPayment> TblvAdvPayments { get; set; } = null!;
        public virtual DbSet<TblvAgreementReminder> TblvAgreementReminders { get; set; } = null!;
        public virtual DbSet<TblvExpense> TblvExpenses { get; set; } = null!;
        public virtual DbSet<TblvIou> TblvIous { get; set; } = null!;
        public virtual DbSet<TblvIourtn> TblvIourtns { get; set; } = null!;
        public virtual DbSet<TblvNotification> TblvNotifications { get; set; } = null!;
        public virtual DbSet<TblvPropAssign> TblvPropAssigns { get; set; } = null!;
        public virtual DbSet<Tblvendertoservice> Tblvendertoservices { get; set; } = null!;
        public virtual DbSet<Tblvvendor> Tblvvendors { get; set; } = null!;
        public virtual DbSet<VExepnsesAccount> VExepnsesAccounts { get; set; } = null!;
        public virtual DbSet<VLeadForwardList> VLeadForwardLists { get; set; } = null!;
        public virtual DbSet<VLeadList> VLeadLists { get; set; } = null!;
        public virtual DbSet<VPropertyDevelopmentList> VPropertyDevelopmentLists { get; set; } = null!;
        public virtual DbSet<VPropertyRegisterList> VPropertyRegisterLists { get; set; } = null!;
        public virtual DbSet<VPurchasePayment> VPurchasePayments { get; set; } = null!;
        public virtual DbSet<VVendorToServiceList> VVendorToServiceLists { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-D3N6FAF\\SQLEXPRESS01;Database=CRM;User Id=sa;Password=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CPropertyRegisterList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("cPropertyRegisterList");

                entity.Property(e => e.AddBy).HasMaxLength(100);

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Anualcostforbuyer)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("anualcostforbuyer");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Contacttype).HasColumnName("contacttype");

                entity.Property(e => e.Costanually)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("costanually");

                entity.Property(e => e.Dateofpurchorrent)
                    .HasColumnType("datetime")
                    .HasColumnName("dateofpurchorrent");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("deposit");

                entity.Property(e => e.Geolocation)
                    .HasMaxLength(150)
                    .HasColumnName("geolocation");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("id");

                entity.Property(e => e.Mainimg)
                    .HasMaxLength(150)
                    .HasColumnName("mainimg");

                entity.Property(e => e.Minsellingprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("minsellingprice");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(100)
                    .HasColumnName("nationality");

                entity.Property(e => e.Othercost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("othercost");

                entity.Property(e => e.Otherimg)
                    .HasMaxLength(200)
                    .HasColumnName("otherimg");

                entity.Property(e => e.Paymentscheduleno)
                    .HasMaxLength(50)
                    .HasColumnName("paymentscheduleno");

                entity.Property(e => e.Propertname)
                    .HasMaxLength(100)
                    .HasColumnName("propertname");

                entity.Property(e => e.PropertyCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertyCat");

                entity.Property(e => e.PropertySubCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertySubCat");

                entity.Property(e => e.Propertytype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertytype");

                entity.Property(e => e.Renewdate)
                    .HasColumnType("datetime")
                    .HasColumnName("renewdate");

                entity.Property(e => e.Rulesregulations)
                    .HasMaxLength(1000)
                    .HasColumnName("rulesregulations");

                entity.Property(e => e.Sellingprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("sellingprice");

                entity.Property(e => e.Socialmedia).HasColumnName("socialmedia");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName).HasMaxLength(45);

                entity.Property(e => e.Venderpaymentdate)
                    .HasColumnType("datetime")
                    .HasColumnName("venderpaymentdate");
            });

            modelBuilder.Entity<TblAdvPayment>(entity =>
            {
                entity.ToTable("tblAdvPayment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.Cardbank).HasColumnName("cardbank");

                entity.Property(e => e.Cardpaid)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("cardpaid");

                entity.Property(e => e.Cashpaid)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("cashpaid");

                entity.Property(e => e.Chequeno)
                    .HasMaxLength(50)
                    .HasColumnName("chequeno");

                entity.Property(e => e.Chequepaid)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("chequepaid");

                entity.Property(e => e.Customer).HasColumnName("customer");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Paymentfor)
                    .HasMaxLength(50)
                    .HasColumnName("paymentfor");

                entity.Property(e => e.Salesby).HasColumnName("salesby");
            });

            modelBuilder.Entity<TblAgreementReminder>(entity =>
            {
                entity.ToTable("tblAgreementReminder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Agreementtype).HasColumnName("agreementtype");

                entity.Property(e => e.Custcode).HasColumnName("custcode");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Enddate)
                    .HasColumnType("datetime")
                    .HasColumnName("enddate");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Remindon)
                    .HasColumnType("datetime")
                    .HasColumnName("remindon");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<TblAgreementtype>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblAgree__516F0395096988CD");

                entity.ToTable("tblAgreementtype");

                entity.HasIndex(e => e.TypeName, "Id_AgreementtypeType")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblApprovedBy>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblAppro__516F0395B169DD56");

                entity.ToTable("tblApprovedBy");

                entity.HasIndex(e => e.TypeName, "Id_ApprovedByType")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblBranchControl>(entity =>
            {
                entity.HasKey(e => new { e.Cid, e.BrId })
                    .HasName("PK__tblBranchControl__0E6E26BF");

                entity.ToTable("tblBranchControl");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.BrId).HasColumnName("BrID");

                entity.Property(e => e.ChqId).HasColumnName("ChqID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Ppno).HasColumnName("PPNO");
            });

            modelBuilder.Entity<TblBulkcreditpaymentd>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblBulkcreditpaymentd");

                entity.Property(e => e.Advance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CashPay)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ChequePay)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Credit)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.InvDate).HasColumnType("datetime");

                entity.Property(e => e.InvoiceNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Rtnnote)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RTNNote")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.VoucherNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblBulkcreditpaymenth>(entity =>
            {
                entity.HasKey(e => e.VoucherNo)
                    .HasName("PK__tblBulkc__3AD31D6E86970988");

                entity.ToTable("tblBulkcreditpaymenth");

                entity.Property(e => e.VoucherNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Balance)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.BrId).HasColumnName("BrID");

                entity.Property(e => e.CashPaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ChequeId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ChequeID");

                entity.Property(e => e.ChequePaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Date1).HasColumnType("datetime");

                entity.Property(e => e.RDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rDate");

                entity.Property(e => e.Rtnid).HasColumnName("RTNID");

                entity.Property(e => e.Rtnpaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("RTNPaid")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<TblCallInsight>(entity =>
            {
                entity.ToTable("tblCallInsights");

                entity.Property(e => e.AddOn).HasColumnType("datetime");

                entity.Property(e => e.AssignedOn).HasColumnType("datetime");

                entity.Property(e => e.AssignedTo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CallEndedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("callEndedOn");

                entity.Property(e => e.CalledOn)
                    .HasColumnType("datetime")
                    .HasColumnName("calledOn");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNo2)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCampainH>(entity =>
            {
                entity.HasKey(e => e.No);

                entity.ToTable("tblCampainH");

                entity.Property(e => e.No)
                    .HasMaxLength(100)
                    .HasColumnName("no");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Datefrom)
                    .HasColumnType("datetime")
                    .HasColumnName("datefrom");

                entity.Property(e => e.Dateto)
                    .HasColumnType("datetime")
                    .HasColumnName("dateto");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Totalcost)
                    .HasMaxLength(100)
                    .HasColumnName("totalcost");
            });

            modelBuilder.Entity<TblCampainMedium>(entity =>
            {
                entity.ToTable("tblCampainMedia");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Campainno)
                    .HasMaxLength(100)
                    .HasColumnName("campainno");

                entity.Property(e => e.Mediaid)
                    .HasMaxLength(200)
                    .HasColumnName("mediaid");
            });

            modelBuilder.Entity<TblCitytype>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblCityt__516F03955728FED6");

                entity.ToTable("tblCitytype");

                entity.HasIndex(e => e.TypeName, "Id_Citytype")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblCustomerRequirement>(entity =>
            {
                entity.ToTable("tblCustomerRequirements");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Custid).HasColumnName("custid");

                entity.Property(e => e.Requirementid).HasColumnName("requirementid");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<TblDesignationPermission>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblDesignationPermission");

                entity.Property(e => e.Accesslocation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("accesslocation");

                entity.Property(e => e.Event)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("event");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TblDesignationtype>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblDesig__516F03958569D658");

                entity.ToTable("tblDesignationtype");

                entity.HasIndex(e => e.TypeName, "Id_DesignationType")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblDndLead>(entity =>
            {
                entity.ToTable("tblDndLeads");

                entity.Property(e => e.AddOn).HasColumnType("datetime");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.Assignon).HasColumnType("datetime");

                entity.Property(e => e.Comments).HasMaxLength(300);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.LeadId).HasMaxLength(50);

                entity.Property(e => e.LeadName).HasMaxLength(100);

                entity.Property(e => e.Otherno).HasMaxLength(25);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.Recievedon).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblIou>(entity =>
            {
                entity.ToTable("tblIOU");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Approvedby).HasColumnName("approvedby");

                entity.Property(e => e.Branchid).HasColumnName("branchid");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Issueto).HasColumnName("issueto");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");

                entity.Property(e => e.Returned).HasColumnName("returned");

                entity.Property(e => e.Returnon)
                    .HasColumnType("datetime")
                    .HasColumnName("returnon");

                entity.Property(e => e.Value)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("value");
            });

            modelBuilder.Entity<TblIourtn>(entity =>
            {
                entity.HasKey(e => e.Rtnid)
                    .HasName("PK_tnlIOUrtn");

                entity.ToTable("tblIOUrtn");

                entity.Property(e => e.Rtnid).HasColumnName("rtnid");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Brid).HasColumnName("brid");

                entity.Property(e => e.Desc)
                    .HasMaxLength(150)
                    .HasColumnName("desc");

                entity.Property(e => e.Iouid).HasColumnName("IOUid");

                entity.Property(e => e.Retnon)
                    .HasColumnType("datetime")
                    .HasColumnName("retnon");
            });

            modelBuilder.Entity<TblIssuedTo>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblIssue__516F0395BB272052");

                entity.ToTable("tblIssuedTo");

                entity.HasIndex(e => e.TypeName, "Id_IssuedToType")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblLeadStatus>(entity =>
            {
                entity.ToTable("tblLeadStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Leadstatus)
                    .HasMaxLength(50)
                    .HasColumnName("leadstatus");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .HasColumnName("remark");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<TblLeadforward>(entity =>
            {
                entity.ToTable("tblLeadforward");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Forwardfromid)
                    .HasMaxLength(50)
                    .HasColumnName("forwardfromid");

                entity.Property(e => e.Forwardstaffid)
                    .HasMaxLength(50)
                    .HasColumnName("forwardstaffid");

                entity.Property(e => e.Leadid)
                    .HasMaxLength(20)
                    .HasColumnName("leadid");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");
            });

            modelBuilder.Entity<TblLeadlog>(entity =>
            {
                entity.ToTable("tblLeadlog");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Leadid)
                    .HasMaxLength(100)
                    .HasColumnName("leadid");

                entity.Property(e => e.Log)
                    .HasMaxLength(1000)
                    .HasColumnName("log");
            });

            modelBuilder.Entity<TblMediaLink>(entity =>
            {
                entity.ToTable("tblMediaLink");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Campainno)
                    .HasMaxLength(100)
                    .HasColumnName("campainno");

                entity.Property(e => e.Medialink)
                    .HasMaxLength(200)
                    .HasColumnName("medialink");
            });

            modelBuilder.Entity<TblMedium>(entity =>
            {
                entity.ToTable("tblMedia");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Media).HasMaxLength(45);

                entity.Property(e => e.Remark).HasMaxLength(100);
            });

            modelBuilder.Entity<TblMeeting>(entity =>
            {
                entity.ToTable("tblMeeting");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Conclusion)
                    .HasMaxLength(100)
                    .HasColumnName("conclusion");

                entity.Property(e => e.Custid).HasColumnName("custid");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Meetdate)
                    .HasColumnType("datetime")
                    .HasColumnName("meetdate");

                entity.Property(e => e.Meettime)
                    .HasMaxLength(20)
                    .HasColumnName("meettime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Venue)
                    .HasMaxLength(100)
                    .HasColumnName("venue");
            });

            modelBuilder.Entity<TblNotification>(entity =>
            {
                entity.ToTable("tblNotification");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Forwardto)
                    .HasMaxLength(50)
                    .HasColumnName("forwardto");

                entity.Property(e => e.From)
                    .HasColumnType("datetime")
                    .HasColumnName("from");

                entity.Property(e => e.Message)
                    .HasMaxLength(250)
                    .HasColumnName("message");

                entity.Property(e => e.Notify).HasColumnName("notify");

                entity.Property(e => e.Priorityid).HasColumnName("priorityid");

                entity.Property(e => e.Snoozeon)
                    .HasColumnType("datetime")
                    .HasColumnName("snoozeon");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Time)
                    .HasMaxLength(20)
                    .HasColumnName("time");
            });

            modelBuilder.Entity<TblPlanToDo>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("tblPlanToDo");

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblPreferedContactMethod>(entity =>
            {
                entity.ToTable("tblPreferedContactMethod");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.ContactMethod).HasMaxLength(45);

                entity.Property(e => e.Remark).HasMaxLength(100);
            });

            modelBuilder.Entity<TblPymentschedule>(entity =>
            {
                entity.HasKey(e => e.PaymentScheduleNo);

                entity.ToTable("tblPymentschedule");

                entity.Property(e => e.PaymentScheduleNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Paidon)
                    .HasColumnType("datetime")
                    .HasColumnName("paidon");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");

                entity.Property(e => e.Renewevery)
                    .HasMaxLength(50)
                    .HasColumnName("renewevery");

                entity.Property(e => e.Renewstatus)
                    .HasMaxLength(50)
                    .HasColumnName("renewstatus");

                entity.Property(e => e.Rxpaccount)
                    .HasMaxLength(50)
                    .HasColumnName("rxpaccount");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Venderid).HasColumnName("venderid");
            });

            modelBuilder.Entity<TblRsvpType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_rsvpType");

                entity.ToTable("tblRsvpType");

                entity.Property(e => e.Cid).HasDefaultValueSql("((1))");

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblSale>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblSales__516F0395F2DED652");

                entity.ToTable("tblSales");

                entity.HasIndex(e => e.TypeName, "Id_SalesType")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblServicetype>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tblServi__516F03952B22C00D");

                entity.ToTable("tblServicetype");

                entity.HasIndex(e => e.TypeName, "Id_ItemType")
                    .IsUnique();

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblStaffperformance>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblStaffperformances");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.StaffName).HasMaxLength(100);
            });

            modelBuilder.Entity<TblSupplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId)
                    .HasName("PK__tblSuppl__4BE66694AB5E9ED5");

                entity.ToTable("tblSupplier");

                entity.HasIndex(e => e.SupplierName, "IDX_SupplierName")
                    .IsUnique();

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.Address)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Cid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VatNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblSupportStaff>(entity =>
            {
                entity.ToTable("tblSupportStaff");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Meetid).HasColumnName("meetid");

                entity.Property(e => e.Staffid).HasColumnName("staffid");
            });

            modelBuilder.Entity<TblVcampaign>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblVcampaign");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Datefrom)
                    .HasColumnType("datetime")
                    .HasColumnName("datefrom");

                entity.Property(e => e.Dateto)
                    .HasColumnType("datetime")
                    .HasColumnName("dateto");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.No)
                    .HasMaxLength(100)
                    .HasColumnName("no");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Totalcost)
                    .HasMaxLength(100)
                    .HasColumnName("totalcost");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TblVmeeting>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblVMeeting");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Conclusion)
                    .HasMaxLength(100)
                    .HasColumnName("conclusion");

                entity.Property(e => e.CustName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Expr1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Meetdate)
                    .HasColumnType("datetime")
                    .HasColumnName("meetdate");

                entity.Property(e => e.Meettime)
                    .HasMaxLength(20)
                    .HasColumnName("meettime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Venue)
                    .HasMaxLength(100)
                    .HasColumnName("venue");
            });

            modelBuilder.Entity<TblVstaff>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblVstaffs");

                entity.Property(e => e.Addby)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Designation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("designation");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Mobileno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("mobileno");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Parentid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("parentid");

                entity.Property(e => e.Passport)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("passport");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Userid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userid");

                entity.Property(e => e.Userimage)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userimage");

                entity.Property(e => e.VisaIssuedate)
                    .HasColumnType("datetime")
                    .HasColumnName("visaIssuedate");
            });

            modelBuilder.Entity<Tblbank>(entity =>
            {
                entity.HasKey(e => e.BankId)
                    .HasName("PK__tblbanks__AA08CB33E95BF969");

                entity.ToTable("tblbanks");

                entity.Property(e => e.BankId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BankID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tblbankbranch>(entity =>
            {
                entity.ToTable("tblbankbranch");

                entity.HasIndex(e => new { e.Brid, e.Bankid }, "idxPK")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Bankid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BANKID");

                entity.Property(e => e.BranchName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Brid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BRID");

                entity.Property(e => e.ContactPerson)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("tel");
            });

            modelBuilder.Entity<Tblbranch>(entity =>
            {
                entity.HasKey(e => e.BrId)
                    .HasName("PK__tblbranches__7C8480AE");

                entity.ToTable("tblbranches");

                entity.HasIndex(e => new { e.Cid, e.BranchName }, "UQ__tblbranches__7D78A4E7")
                    .IsUnique();

                entity.Property(e => e.BrId)
                    .ValueGeneratedNever()
                    .HasColumnName("BrID");

                entity.Property(e => e.BranchName).HasMaxLength(150);

                entity.Property(e => e.Brcode)
                    .HasMaxLength(3)
                    .HasColumnName("BRCODE");

                entity.Property(e => e.CashBook)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.CashBookH)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Cid).HasColumnName("CID");

                entity.Property(e => e.Email).HasMaxLength(45);

                entity.Property(e => e.Fax).HasMaxLength(45);

                entity.Property(e => e.Phone).HasMaxLength(45);

                entity.Property(e => e.PrintAddress).HasMaxLength(450);

                entity.Property(e => e.PrintTitle).HasMaxLength(450);
            });

            modelBuilder.Entity<Tblcompany>(entity =>
            {
                entity.ToTable("tblcompany");

                entity.HasIndex(e => e.CompanyName, "UQ__tblcompany__08EA5793")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(450);

                entity.Property(e => e.BarcodeTitle)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CashBookH)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.CompanyLogo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(145);

                entity.Property(e => e.Fax).HasMaxLength(145);

                entity.Property(e => e.Phone).HasMaxLength(145);

                entity.Property(e => e.ServiceCharge)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Web).HasMaxLength(145);
            });

            modelBuilder.Entity<Tblcompanydetail>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("tblcompanydetails");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Fax).HasMaxLength(100);

                entity.Property(e => e.Logo).HasMaxLength(100);

                entity.Property(e => e.Mlname)
                    .HasMaxLength(100)
                    .HasColumnName("MLName");

                entity.Property(e => e.Mobile).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.RegNo).HasMaxLength(100);

                entity.Property(e => e.TaxMethod).HasMaxLength(100);

                entity.Property(e => e.VatNo).HasMaxLength(100);

                entity.Property(e => e.Website).HasMaxLength(100);
            });

            modelBuilder.Entity<Tblcontrol>(entity =>
            {
                entity.ToTable("tblcontrol");

                entity.Property(e => e.AdvrptNo).HasColumnName("ADVRptNo");

                entity.Property(e => e.BankTr).HasColumnName("BankTR");

                entity.Property(e => e.BarCode).HasDefaultValueSql("((0))");

                entity.Property(e => e.CallStats).HasColumnName("call_stats");

                entity.Property(e => e.CallsLeftStats).HasColumnName("calls_left_stats");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.Grnid).HasColumnName("GRNID");

                entity.Property(e => e.LeadStats).HasColumnName("lead_stats");

                entity.Property(e => e.PurchaseReturnId).HasColumnName("PurchaseReturnID");
            });

            modelBuilder.Entity<Tblcurrentacc>(entity =>
            {
                entity.ToTable("tblcurrentacc");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AccNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.Code)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Date1).HasColumnType("datetime");

                entity.Property(e => e.Instruction)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RetDed)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Status)
                    .HasColumnType("decimal(1, 0)")
                    .HasColumnName("status");
            });

            modelBuilder.Entity<Tblcustomer>(entity =>
            {
                entity.HasKey(e => e.CustId);

                entity.ToTable("tblcustomer");

                entity.HasIndex(e => e.CustName, "ID_CustName")
                    .IsUnique();

                entity.Property(e => e.CustId).HasColumnName("CustID");

                entity.Property(e => e.CardNo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ContPerson)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreditLimit)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.CustAddress)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CustCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustMobile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CustName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CustPhone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Points)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("points")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TotRetCheque)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("totRetCheque");

                entity.Property(e => e.VatNo)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tblemaincat>(entity =>
            {
                entity.ToTable("tblemaincat");

                entity.HasIndex(e => e.MainCategory, "idxMainCategory")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.MainCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");
            });

            modelBuilder.Entity<Tblesubcat>(entity =>
            {
                entity.ToTable("tblesubcat");

                entity.HasIndex(e => e.SubCategory, "idxSubCategory")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");

                entity.Property(e => e.SubCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tblexpense>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ReceiptNo })
                    .HasName("PK__tblexpen__5ED45B0DB830E681");

                entity.ToTable("tblexpenses");

                entity.HasIndex(e => e.ReceiptNo, "U_ReceiptNo")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.ReceiptNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AuthBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BrId).HasColumnName("BrID");

                entity.Property(e => e.CashPaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ChequeNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ChequePaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Cid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cid");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.MainCatId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MainCatID");

                entity.Property(e => e.NetTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Paid).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rDate");

                entity.Property(e => e.Status)
                    .HasColumnType("decimal(1, 0)")
                    .HasColumnName("status");

                entity.Property(e => e.SubcatId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SubcatID");

                entity.Property(e => e.SupplierId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SupplierID");

                entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UniqueID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VDate)
                    .HasColumnType("datetime")
                    .HasColumnName("vDate");

                entity.Property(e => e.Vat)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VAT");

                entity.Property(e => e.Vatp)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VATP");
            });

            modelBuilder.Entity<Tblexpensesaccount>(entity =>
            {
                entity.ToTable("tblexpensesaccount");

                entity.HasIndex(e => new { e.MainCatId, e.SubCatId }, "idxtblexpensesaccount")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MainCatId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MainCatID");

                entity.Property(e => e.SubCatId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SubCatID");
            });

            modelBuilder.Entity<Tbllead>(entity =>
            {
                entity.HasKey(e => e.Leadno);

                entity.ToTable("tblleads");

                entity.Property(e => e.Leadno)
                    .HasMaxLength(20)
                    .HasColumnName("leadno");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.Assigned).HasColumnName("assigned");

                entity.Property(e => e.Assignon)
                    .HasColumnType("datetime")
                    .HasColumnName("assignon");

                entity.Property(e => e.Attending)
                    .HasMaxLength(100)
                    .HasColumnName("attending")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Called).HasColumnName("called");

                entity.Property(e => e.Campainid)
                    .HasMaxLength(50)
                    .HasColumnName("campainid");

                entity.Property(e => e.Comments)
                    .HasMaxLength(300)
                    .HasColumnName("comments");

                entity.Property(e => e.ContactMethod)
                    .HasColumnName("contactMethod")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CrossSegmentLead)
                    .HasColumnName("crossSegmentLead")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.Importance).HasColumnName("importance");

                entity.Property(e => e.InterestedIn)
                    .HasMaxLength(200)
                    .HasColumnName("interestedIn");

                entity.Property(e => e.IsInterested)
                    .HasColumnName("isInterested")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLost)
                    .HasColumnName("isLost")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Otherno)
                    .HasMaxLength(200)
                    .HasColumnName("otherno");

                entity.Property(e => e.Phone)
                    .HasMaxLength(200)
                    .HasColumnName("phone");

                entity.Property(e => e.PlanToDo)
                    .HasColumnName("planToDo")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlanToDoWhen)
                    .HasColumnType("datetime")
                    .HasColumnName("planToDoWhen");

                entity.Property(e => e.Recievedon)
                    .HasColumnType("datetime")
                    .HasColumnName("recievedon");

                entity.Property(e => e.RsvpTypeId)
                    .HasColumnName("rsvpTypeId")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sourceid).HasColumnName("sourceid");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<TblleadAssign>(entity =>
            {
                entity.ToTable("tblleadAssign");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Leadid)
                    .HasMaxLength(100)
                    .HasColumnName("leadid");

                entity.Property(e => e.Staffid).HasColumnName("staffid");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Tblleadlogview>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblleadlogview");

                entity.Property(e => e.Addby)
                    .HasMaxLength(100)
                    .HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Leadid)
                    .HasMaxLength(100)
                    .HasColumnName("leadid");

                entity.Property(e => e.Log)
                    .HasMaxLength(500)
                    .HasColumnName("log");
            });

            modelBuilder.Entity<Tblprioritytype>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("tblprioritytype");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.TypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<Tblpropassign>(entity =>
            {
                entity.ToTable("tblpropassign");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Advnotno)
                    .HasMaxLength(50)
                    .HasColumnName("advnotno");

                entity.Property(e => e.Customerid).HasColumnName("customerid");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.Salesperson).HasColumnName("salesperson");

                entity.Property(e => e.Validtill)
                    .HasColumnType("date")
                    .HasColumnName("validtill");
            });

            modelBuilder.Entity<Tblpropdev>(entity =>
            {
                entity.ToTable("tblpropdev");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Approvedby).HasColumnName("approvedby");

                entity.Property(e => e.Bankid).HasColumnName("bankid");

                entity.Property(e => e.Banktransfer)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("banktransfer");

                entity.Property(e => e.Cashpaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("cashpaid");

                entity.Property(e => e.Chequeid).HasColumnName("chequeid");

                entity.Property(e => e.Chequepaid)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("chequepaid");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Expenseaccount).HasColumnName("expenseaccount");

                entity.Property(e => e.Propertyno)
                    .HasMaxLength(50)
                    .HasColumnName("propertyno");

                entity.Property(e => e.Propname)
                    .HasMaxLength(100)
                    .HasColumnName("propname");

                entity.Property(e => e.Vender).HasColumnName("vender");
            });

            modelBuilder.Entity<TblpropertyCategory>(entity =>
            {
                entity.ToTable("tblpropertyCategory");

                entity.HasIndex(e => e.PropertyCat, "idxpropertyCat")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.PropertyCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertyCat");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");
            });

            modelBuilder.Entity<TblpropertySubCategory>(entity =>
            {
                entity.ToTable("tblpropertySubCategory");

                entity.HasIndex(e => e.PropertySubCat, "idxpropertySubCat")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.PropertySubCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertySubCat");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");
            });

            modelBuilder.Entity<Tblpropertyregister>(entity =>
            {
                entity.ToTable("tblpropertyregister");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Anualcostforbuyer)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("anualcostforbuyer");

                entity.Property(e => e.Category).HasColumnName("category");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Contacttype).HasColumnName("contacttype");

                entity.Property(e => e.Costanually)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("costanually");

                entity.Property(e => e.Dateofpurchorrent)
                    .HasColumnType("datetime")
                    .HasColumnName("dateofpurchorrent");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("deposit");

                entity.Property(e => e.Geolocation)
                    .HasMaxLength(150)
                    .HasColumnName("geolocation");

                entity.Property(e => e.Mainimg)
                    .HasMaxLength(150)
                    .HasColumnName("mainimg");

                entity.Property(e => e.Minsellingprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("minsellingprice");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(100)
                    .HasColumnName("nationality");

                entity.Property(e => e.Othercost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("othercost");

                entity.Property(e => e.Otherimg)
                    .HasMaxLength(200)
                    .HasColumnName("otherimg");

                entity.Property(e => e.Paymentscheduleno)
                    .HasMaxLength(50)
                    .HasColumnName("paymentscheduleno");

                entity.Property(e => e.Propertname)
                    .HasMaxLength(100)
                    .HasColumnName("propertname");

                entity.Property(e => e.Renewdate)
                    .HasColumnType("datetime")
                    .HasColumnName("renewdate");

                entity.Property(e => e.Rulesregulations)
                    .HasMaxLength(1000)
                    .HasColumnName("rulesregulations");

                entity.Property(e => e.Sellingprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("sellingprice");

                entity.Property(e => e.Socialmedia).HasColumnName("socialmedia");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Subcategory).HasColumnName("subcategory");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Vender).HasColumnName("vender");

                entity.Property(e => e.Venderpaymentdate)
                    .HasColumnType("datetime")
                    .HasColumnName("venderpaymentdate");
            });

            modelBuilder.Entity<Tblpropertytype>(entity =>
            {
                entity.ToTable("tblpropertytype");

                entity.HasIndex(e => e.Propertytype, "idxpropertytype")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.Propertytype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertytype");

                entity.Property(e => e.Remark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");
            });

            modelBuilder.Entity<Tblreccheque>(entity =>
            {
                entity.HasKey(e => e.Chqid)
                    .HasName("PK__tblreccheques__351DDF8C");

                entity.ToTable("tblreccheques");

                entity.Property(e => e.Chqid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("chqid");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Bankdate)
                    .HasColumnType("datetime")
                    .HasColumnName("bankdate");

                entity.Property(e => e.Bankid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("bankid");

                entity.Property(e => e.BrId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BrID");

                entity.Property(e => e.Branchid)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("branchid");

                entity.Property(e => e.Chqno)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("chqno");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.Customercode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("customercode");

                entity.Property(e => e.Deposit).HasColumnType("decimal(1, 0)");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Invno)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("invno");

                entity.Property(e => e.Rdate)
                    .HasColumnType("datetime")
                    .HasColumnName("RDate");

                entity.Property(e => e.Ret).HasColumnType("decimal(1, 0)");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Used)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0.00))");
            });

            modelBuilder.Entity<Tblsource>(entity =>
            {
                entity.ToTable("tblsource");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cid");

                entity.Property(e => e.Remark).HasMaxLength(100);

                entity.Property(e => e.Source).HasMaxLength(100);
            });

            modelBuilder.Entity<Tblstaff>(entity =>
            {
                entity.ToTable("tblstaffs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.CallsMonthlyTarget).HasColumnName("callsMonthlyTarget");

                entity.Property(e => e.Designation)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("designation");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Mobileno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("mobileno");

                entity.Property(e => e.MonthlyTarget).HasColumnName("monthlyTarget");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Parentid).HasColumnName("parentid");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<Tbluser>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("PK__tblusers__1367E606");

                entity.ToTable("tblusers");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Cid).HasColumnName("cid");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("discount")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.Fullaccess)
                    .HasColumnType("decimal(1, 0)")
                    .HasColumnName("fullaccess");

                entity.Property(e => e.Hash)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("hash");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.Loginname)
                    .HasMaxLength(100)
                    .HasColumnName("loginname");

                entity.Property(e => e.Logintime)
                    .HasColumnType("datetime")
                    .HasColumnName("logintime");

                entity.Property(e => e.Logouttime)
                    .HasColumnType("datetime")
                    .HasColumnName("logouttime");

                entity.Property(e => e.Openbalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("openbalance")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.Passport)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("passport");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Status)
                    .HasColumnType("decimal(1, 0)")
                    .HasColumnName("status");

                entity.Property(e => e.Superuser).HasColumnName("superuser");

                entity.Property(e => e.Ud).HasColumnName("ud");

                entity.Property(e => e.Usercode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("usercode");

                entity.Property(e => e.Userimage)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userimage");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");

                entity.Property(e => e.VisaExpiredate)
                    .HasColumnType("datetime")
                    .HasColumnName("visaExpiredate");

                entity.Property(e => e.VisaIssuedate)
                    .HasColumnType("datetime")
                    .HasColumnName("visaIssuedate");
            });

            modelBuilder.Entity<Tblusericon>(entity =>
            {
                entity.ToTable("tblusericons");

                entity.HasIndex(e => new { e.UserIcon, e.UserId }, "UQ__tbluseri__E7B413052DD55283")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Status).HasDefaultValueSql("((2))");

                entity.Property(e => e.UserIcon).HasMaxLength(20);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Tbluserlevel>(entity =>
            {
                entity.ToTable("tbluserlevel");

                entity.HasIndex(e => e.Userlevel, "UKuserlevel")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cid");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Userlevel)
                    .HasMaxLength(100)
                    .HasColumnName("userlevel");
            });

            modelBuilder.Entity<Tbluserpermission>(entity =>
            {
                entity.ToTable("tbluserpermission");

                entity.Property(e => e.Accesslocation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("accesslocation");

                entity.Property(e => e.Event)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("event");
            });

            modelBuilder.Entity<TblvAdvPayment>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvAdvPayment");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cardpaid)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("cardpaid");

                entity.Property(e => e.Cashpaid)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("cashpaid");

                entity.Property(e => e.Chequeno)
                    .HasMaxLength(50)
                    .HasColumnName("chequeno");

                entity.Property(e => e.Chequepaid)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("chequepaid");

                entity.Property(e => e.CustName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Propertname)
                    .HasMaxLength(100)
                    .HasColumnName("propertname");

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            modelBuilder.Entity<TblvAgreementReminder>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvAgreementReminder");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.CustName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Enddate)
                    .HasColumnType("datetime")
                    .HasColumnName("enddate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(100)
                    .HasColumnName("remarks");

                entity.Property(e => e.Remindon)
                    .HasColumnType("datetime")
                    .HasColumnName("remindon");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TypeName).HasMaxLength(45);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TblvExpense>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvExpenses");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.BranchName).HasMaxLength(150);

                entity.Property(e => e.CashPaid).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ChequeNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ChequePaid).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Cid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cid");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.MainCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NetTotal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Paid).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rDate");

                entity.Property(e => e.ReceiptNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnType("decimal(1, 0)")
                    .HasColumnName("status");

                entity.Property(e => e.SubCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");

                entity.Property(e => e.VDate)
                    .HasColumnType("datetime")
                    .HasColumnName("vDate");

                entity.Property(e => e.Vat)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VAT");

                entity.Property(e => e.Vatp)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VATP");
            });

            modelBuilder.Entity<TblvIou>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvIOU");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.BranchName).HasMaxLength(150);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");

                entity.Property(e => e.Returned).HasColumnName("returned");

                entity.Property(e => e.Returnon)
                    .HasColumnType("datetime")
                    .HasColumnName("returnon");

                entity.Property(e => e.TypeName).HasMaxLength(45);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");

                entity.Property(e => e.Value)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("value");
            });

            modelBuilder.Entity<TblvIourtn>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvIOUrtn");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.BranchName).HasMaxLength(150);

                entity.Property(e => e.Desc)
                    .HasMaxLength(150)
                    .HasColumnName("desc");

                entity.Property(e => e.Iouid).HasColumnName("IOUid");

                entity.Property(e => e.Retnon)
                    .HasColumnType("datetime")
                    .HasColumnName("retnon");

                entity.Property(e => e.Rtnid).HasColumnName("rtnid");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TblvNotification>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvNotification");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.ForwardId)
                    .HasMaxLength(50)
                    .HasColumnName("forwardId");

                entity.Property(e => e.Forwardto)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("forwardto");

                entity.Property(e => e.From)
                    .HasColumnType("datetime")
                    .HasColumnName("from");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Message)
                    .HasMaxLength(250)
                    .HasColumnName("message");

                entity.Property(e => e.Notify)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("notify");

                entity.Property(e => e.Priorityid).HasColumnName("priorityid");

                entity.Property(e => e.Snoozeon)
                    .HasColumnType("datetime")
                    .HasColumnName("snoozeon");

                entity.Property(e => e.Time)
                    .HasMaxLength(20)
                    .HasColumnName("time");

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TblvPropAssign>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvPropAssign");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Advnotno)
                    .HasMaxLength(50)
                    .HasColumnName("advnotno");

                entity.Property(e => e.CustName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .HasColumnName("description");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TypeName).HasMaxLength(45);

                entity.Property(e => e.Username)
                    .HasMaxLength(100)
                    .HasColumnName("username");

                entity.Property(e => e.Validtill)
                    .HasColumnType("date")
                    .HasColumnName("validtill");
            });

            modelBuilder.Entity<Tblvendertoservice>(entity =>
            {
                entity.ToTable("tblvendertoservice");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Addby).HasColumnName("addby");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Serviceid).HasColumnName("serviceid");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Venderid).HasColumnName("venderid");
            });

            modelBuilder.Entity<Tblvvendor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("tblvvendor");

                entity.Property(e => e.Address)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.Cid)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Staff)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("staff");

                entity.Property(e => e.Status).HasColumnType("decimal(1, 0)");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VatNo)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VExepnsesAccount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vExepnsesAccount");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MainCatId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MainCatID");

                entity.Property(e => e.MainCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubCatId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SubCatID");

                entity.Property(e => e.SubCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VLeadForwardList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vLeadForwardList");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.ForwardFrom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("forwardFrom");

                entity.Property(e => e.ForwardTo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("forwardTo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Leadid)
                    .HasMaxLength(20)
                    .HasColumnName("leadid");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");
            });

            modelBuilder.Entity<VLeadList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vLeadList");

                entity.Property(e => e.Assignon)
                    .HasColumnType("datetime")
                    .HasColumnName("assignon");

                entity.Property(e => e.Called).HasColumnName("called");

                entity.Property(e => e.Campainid)
                    .HasMaxLength(50)
                    .HasColumnName("campainid");

                entity.Property(e => e.Comments)
                    .HasMaxLength(300)
                    .HasColumnName("comments");

                entity.Property(e => e.ContactMethod).HasMaxLength(45);

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.Importance).HasColumnName("importance");

                entity.Property(e => e.Leadno)
                    .HasMaxLength(20)
                    .HasColumnName("leadno");

                entity.Property(e => e.Leadstatus)
                    .HasMaxLength(50)
                    .HasColumnName("leadstatus");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Otherno)
                    .HasMaxLength(200)
                    .HasColumnName("otherno");

                entity.Property(e => e.Phone)
                    .HasMaxLength(200)
                    .HasColumnName("phone");

                entity.Property(e => e.Recievedon)
                    .HasColumnType("datetime")
                    .HasColumnName("recievedon");

                entity.Property(e => e.Source).HasMaxLength(100);

                entity.Property(e => e.Staffname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("staffname");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<VPropertyDevelopmentList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vPropertyDevelopmentList");

                entity.Property(e => e.AddBy).HasMaxLength(100);

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 7)")
                    .HasColumnName("amount");

                entity.Property(e => e.ApprovedBy).HasMaxLength(100);

                entity.Property(e => e.BankCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Banktransfer)
                    .HasColumnType("decimal(18, 7)")
                    .HasColumnName("banktransfer");

                entity.Property(e => e.Cashpaid)
                    .HasColumnType("decimal(18, 7)")
                    .HasColumnName("cashpaid");

                entity.Property(e => e.ChequeId)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Chequepaid)
                    .HasColumnType("decimal(18, 7)")
                    .HasColumnName("chequepaid");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Expenseaccount).HasColumnName("expenseaccount");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.PropertyName).HasMaxLength(100);

                entity.Property(e => e.Propertyno)
                    .HasMaxLength(50)
                    .HasColumnName("propertyno");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VPropertyRegisterList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vPropertyRegisterList");

                entity.Property(e => e.AddBy).HasMaxLength(100);

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Anualcostforbuyer)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("anualcostforbuyer");

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.Contacttype).HasColumnName("contacttype");

                entity.Property(e => e.Costanually)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("costanually");

                entity.Property(e => e.Dateofpurchorrent)
                    .HasColumnType("datetime")
                    .HasColumnName("dateofpurchorrent");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("deposit");

                entity.Property(e => e.Geolocation)
                    .HasMaxLength(150)
                    .HasColumnName("geolocation");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("id");

                entity.Property(e => e.Mainimg)
                    .HasMaxLength(150)
                    .HasColumnName("mainimg");

                entity.Property(e => e.Minsellingprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("minsellingprice");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(100)
                    .HasColumnName("nationality");

                entity.Property(e => e.Othercost)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("othercost");

                entity.Property(e => e.Otherimg)
                    .HasMaxLength(200)
                    .HasColumnName("otherimg");

                entity.Property(e => e.Paymentscheduleno)
                    .HasMaxLength(50)
                    .HasColumnName("paymentscheduleno");

                entity.Property(e => e.Propertname)
                    .HasMaxLength(100)
                    .HasColumnName("propertname");

                entity.Property(e => e.PropertyCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertyCat");

                entity.Property(e => e.PropertySubCat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertySubCat");

                entity.Property(e => e.Propertytype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("propertytype");

                entity.Property(e => e.Renewdate)
                    .HasColumnType("datetime")
                    .HasColumnName("renewdate");

                entity.Property(e => e.Rulesregulations)
                    .HasMaxLength(1000)
                    .HasColumnName("rulesregulations");

                entity.Property(e => e.Sellingprice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("sellingprice");

                entity.Property(e => e.Socialmedia).HasColumnName("socialmedia");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName).HasMaxLength(45);

                entity.Property(e => e.Venderpaymentdate)
                    .HasColumnType("datetime")
                    .HasColumnName("venderpaymentdate");
            });

            modelBuilder.Entity<VPurchasePayment>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vPurchasePayment");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Paidon)
                    .HasColumnType("datetime")
                    .HasColumnName("paidon");

                entity.Property(e => e.PaymentScheduleNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Reason)
                    .HasMaxLength(100)
                    .HasColumnName("reason");

                entity.Property(e => e.Renewevery)
                    .HasMaxLength(50)
                    .HasColumnName("renewevery");

                entity.Property(e => e.Renewstatus)
                    .HasMaxLength(50)
                    .HasColumnName("renewstatus");

                entity.Property(e => e.Rxpaccount)
                    .HasMaxLength(50)
                    .HasColumnName("rxpaccount");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VVendorToServiceList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vVendorToServiceList");

                entity.Property(e => e.AddBy)
                    .HasMaxLength(100)
                    .HasColumnName("addBy");

                entity.Property(e => e.Addon)
                    .HasColumnType("datetime")
                    .HasColumnName("addon");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.SupplierName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName).HasMaxLength(45);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
