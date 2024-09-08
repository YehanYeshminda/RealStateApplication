import { NgModule } from '@angular/core';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { ToastrModule } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { CoreModule } from '../core/core.module';
import { TopNavComponent } from '../core/top-nav/top-nav.component';
import { ContentHeaderComponent } from '../core/content-header/content-header.component';
import { SideNavComponent } from '../core/side-nav/side-nav.component';
import { ContentFooterComponent } from '../core/content-footer/content-footer.component';
import { LeadsComponent } from './leads/leads.component';
import { StaffComponent } from './staff/staff.component';
import { CallcenterComponent } from './callcenter/callcenter.component';
import { CallcenterlistComponent } from './callcenter/callcenterlist/callcenterlist.component';
import { LeadslistComponent } from './leads/leadslist/leadslist.component';
import { StafflistComponent } from './staff/stafflist/stafflist.component';
import { LeadsforwardComponent } from './leadsforward/leadsforward.component';
import { LeadsforwardlistComponent } from './leadsforward/leadsforwardlist/leadsforwardlist.component';
import { MeetingscheduleComponent } from './meetingschedule/meetingschedule.component';
import { MeetingupdateComponent } from './meetingupdate/meetingupdate.component';
import { PropertyregistrationComponent } from './propertyregistration/propertyregistration.component';
import { PropertyassignComponent } from './propertyassign/propertyassign.component';
import { PaymentscheduleComponent } from './paymentschedule/paymentschedule.component';
import { AdvancepaymentComponent } from './advancepayment/advancepayment.component';
import { IouComponent } from './iou/iou.component';
import { IoureturnComponent } from './ioureturn/ioureturn.component';
import { PropertydevelopmentComponent } from './propertydevelopment/propertydevelopment.component';
import { CampaindetailsComponent } from './campaindetails/campaindetails.component';
import { AgreementremindersComponent } from './agreementreminders/agreementreminders.component';
import { AdvancepaymentlistComponent } from './advancepayment/advancepaymentlist/advancepaymentlist.component';
import { AgreementreminderslistComponent } from './agreementreminders/agreementreminderslist/agreementreminderslist.component';
import { CampaindetailslistComponent } from './campaindetails/campaindetailslist/campaindetailslist.component';
import { IoulistComponent } from './iou/ioulist/ioulist.component';
import { IoureturnlistComponent } from './ioureturn/ioureturnlist/ioureturnlist.component';
import { MeetingschedulelistComponent } from './meetingschedule/meetingschedulelist/meetingschedulelist.component';
import { MeetingupdatelistComponent } from './meetingupdate/meetingupdatelist/meetingupdatelist.component';
import { PropertyassignlistComponent } from './propertyassign/propertyassignlist/propertyassignlist.component';
import { PropertydevelopmentlistComponent } from './propertydevelopment/propertydevelopmentlist/propertydevelopmentlist.component';
import { PropertyregistrationlistComponent } from './propertyregistration/propertyregistrationlist/propertyregistrationlist.component';
import { PaymentschedulelistComponent } from './paymentschedule/paymentschedulelist/paymentschedulelist.component';
import { CreditpaymentComponent } from './creditpayment/creditpayment.component';
import { CreditpaymentlistComponent } from './creditpayment/creditpaymentlist/creditpaymentlist.component';
import { ExpensesComponent } from './expenses/expenses.component';
import { ExpenseslistComponent } from './expenses/expenseslist/expenseslist.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { VenderregisterComponent } from './venderregister/venderregister.component';
import { VenderregisterlistComponent } from './venderregister/venderregisterlist/venderregisterlist.component';
import { VendertoserviceComponent } from './vendertoservice/vendertoservice.component';
import { VendertoservicelistComponent } from './vendertoservice/vendertoservicelist/vendertoservicelist.component';
import { TypeComponent } from './type/type.component';
import { DynamicFormComponent } from './dynamic-form/dynamic-form.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { LeadSegregationComponent } from './lead-segregation/lead-segregation.component';
import { CustomerComponent } from './customer/customer.component';
import { CustomerlistComponent } from './customer/customerlist/customerlist.component';
import { CityComponent } from './city/city.component';
import { ChartHomeComponent } from './chart-home/chart-home.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CallCenterModalsComponent } from './callcenter/call-center-modals/call-center-modals.component';
import { UserPermissionComponent } from './user-permission/user-permission.component';
import { DynamicExpenseaccountComponent } from './dynamic-expenseaccount/dynamic-expenseaccount.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationslistComponent } from './notifications/notificationslist/notificationslist.component';
import { CallinsightComponent } from './callinsight/callinsight.component';
import { ContactlistsComponent } from './contactlists/contactlists.component';
import { CallCenterApproveModalComponent } from './callcenter/call-center-approve-modal/call-center-approve-modal.component';
import { TabsModule } from 'ngx-bootstrap/tabs';


@NgModule({
  declarations: [
    DashboardComponent,
    HomeComponent,
    LeadsComponent,
    StaffComponent,
    CallcenterComponent,
    CallcenterlistComponent,
    LeadslistComponent,
    StafflistComponent,
    LeadsforwardComponent,
    LeadsforwardlistComponent,
    MeetingscheduleComponent,
    MeetingupdateComponent,
    PropertyregistrationComponent,
    PropertyassignComponent,
    PaymentscheduleComponent,
    AdvancepaymentComponent,
    IouComponent,
    IoureturnComponent,
    PropertydevelopmentComponent,
    CampaindetailsComponent,
    AgreementremindersComponent,
    AdvancepaymentlistComponent,
    AgreementreminderslistComponent,
    CampaindetailslistComponent,
    IoulistComponent,
    IoureturnlistComponent,
    MeetingschedulelistComponent,
    MeetingupdatelistComponent,
    PropertyassignlistComponent,
    PropertydevelopmentlistComponent,
    PropertyregistrationlistComponent,
    PaymentschedulelistComponent,
    CreditpaymentComponent,
    CreditpaymentlistComponent,
    ExpensesComponent,
    ExpenseslistComponent,
    VenderregisterComponent,
    VenderregisterlistComponent,
    VendertoserviceComponent,
    VendertoservicelistComponent,
    TypeComponent,
    DynamicFormComponent,
    LeadSegregationComponent,
    CustomerComponent,
    CustomerlistComponent,
    CityComponent,
    ChartHomeComponent,
    CallCenterModalsComponent,
    UserPermissionComponent,
    DynamicExpenseaccountComponent,
    NotificationsComponent,
    NotificationslistComponent,
    CallinsightComponent,
    ContactlistsComponent,
    CallCenterApproveModalComponent,
  ],
  exports: [
    SideNavComponent,
    TopNavComponent,
    ContentHeaderComponent,
    ContentFooterComponent,
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
    CoreModule,
    ReactiveFormsModule,
    FormsModule,
    ModalModule.forRoot(),
    NgxSpinnerModule,
    FullCalendarModule,
    TabsModule.forRoot()
    // ToastrModule.forRoot({
    // 	preventDuplicates: true,
    // 	timeOut: 2000,
    // 	positionClass: 'toast-bottom-right',
    // }),
  ],
})
export class DashboardModule { }
